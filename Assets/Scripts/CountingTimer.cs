using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingTimer : AbstractView
{
    [SerializeField]
    Text m_WorkoutMinValue;
    [SerializeField]
    Text m_WorkoutSecValue;
    [SerializeField]
    Text m_RestTimeMinValue;
    [SerializeField]
    Text m_RestTimeSecValue;
    [SerializeField]
    Text m_RepeatValue;
    [SerializeField]
    Button m_Quit;
    [SerializeField]
    Button m_Pause;
    [SerializeField]
    Button m_Continue;
    [SerializeField]
    Button m_Complete;
    [SerializeField]
    Button m_OK;
    [SerializeField]
    Image m_Progress;
    [SerializeField]
    Text m_Time;
    [SerializeField]
    Text m_Content;
    [SerializeField]
    Text m_Status;
    [SerializeField]
    AudioClip m_NoisyTone;
    [SerializeField]
    AudioClip m_LowKey;
    [SerializeField]
    AudioSource m_AudioSource;

    enum CountingState
    {
        Counting,
        Pause,
        Alert,
        WaitingUser,
        Complete,
    }

    enum CountingValue
    {
        Workout,
        RestTime,
        Setting,
    }

    TimerData mCurrentTimerData = null;
    int mCurrentSet = 0;
    ICountingTimerListener mCountingTimerListener = null;
    CountingState mCurrentCountingState = CountingState.Counting;
    CountingValue mCurrentCountingValue = CountingValue.Workout;
    float mCountingTimeSumValue;
    float mCountingTimeLeftValue;
    int mAlertTimes;
    float mNextAlertTime;
    bool mWaitingOK = false;

    public void SetTimerData(TimerData data)
    {
        mCurrentTimerData = data;

        m_WorkoutMinValue.text = mCurrentTimerData.WorkoutMin.ToString();
        m_WorkoutSecValue.text = mCurrentTimerData.WorkoutSec.ToString();
        m_RestTimeMinValue.text = mCurrentTimerData.RestTimeMin.ToString();
        m_RestTimeSecValue.text = mCurrentTimerData.RestTimeSec.ToString();

        m_Complete.onClick.AddListener(OnClickComplete);
        m_Quit.onClick.AddListener(OnClickQuit);
        m_Pause.onClick.AddListener(OnClickPause);
        m_Continue.onClick.AddListener(OnClickContinue);
        m_OK.onClick.AddListener(OnClickOK);
    }

    public void SetListener(ICountingTimerListener listener)
    {
        mCountingTimerListener = listener;
    }

    public void StartCounting()
    {
        mCurrentSet = 1;

        SetCountingState(CountingState.Counting);
        SetCountingValue(CountingValue.Setting);
    }

    private void Update()
    {
        switch(mCurrentCountingState)
        {
            case CountingState.Counting:
                mCountingTimeLeftValue -= Time.deltaTime;

                if (mCountingTimeLeftValue <= 0)
                    TimeIsUp();
                break;
            case CountingState.Alert:
                if (Time.time >= mNextAlertTime)
                    Alert();
                break;
        }

        UpdateTimeText();
    }

    void OnClickComplete()
    {
        mCountingTimerListener.OnClickComplete();
    }

    void OnClickQuit()
    {
        mCountingTimerListener.OnClickQuit();
    }

    void OnClickPause()
    {
        SetCountingState(CountingState.Pause);
    }

    void OnClickContinue()
    {
        SetCountingState(CountingState.Counting);
    }

    void OnClickOK()
    {
        if (mWaitingOK)
            mWaitingOK = false;
        AlertOK();
    }

    void UpdateTimeText()
    {
        switch(mCurrentCountingValue)
        {
            case CountingValue.Setting:
                m_Time.text = string.Format("{0}", (int)mCountingTimeLeftValue);
                break;
            default:
                m_Time.text = string.Format("{0:00}:{1:00}", (int)(mCountingTimeLeftValue / 60f), (int)(mCountingTimeLeftValue % 60f));
                break;
        }
        m_Progress.fillAmount = mCountingTimeLeftValue / mCountingTimeSumValue;
    }

    void TimeIsUp()
    {
        switch (mCurrentCountingValue)
        {
            case CountingValue.Setting:
                SetCountingValue(CountingValue.Workout);
                break;
            case CountingValue.RestTime:
                if(mCountingTimeSumValue == 0)
                    SetCountingValue(CountingValue.Setting);
                else
                    SetCountingState(CountingState.Alert);
                break;
            case CountingValue.Workout:
                SetCountingState(CountingState.Alert);
                break;
        }
    }

    void SetCountingState(CountingState state)
    {
        mCurrentCountingState = state;

        m_RepeatValue.text = (mCurrentTimerData.Repeat - mCurrentSet+1).ToString();

        m_Quit.gameObject.SetActive(true);
        m_OK.gameObject.SetActive(false);
        m_Pause.gameObject.SetActive(false);
        m_Complete.gameObject.SetActive(false);
        m_Continue.gameObject.SetActive(false);
        m_Status.text = string.Empty;
        switch (mCurrentCountingState)
        {
            case CountingState.Complete:
                m_Quit.gameObject.SetActive(false);
                m_Complete.gameObject.SetActive(true);
                break;
            case CountingState.Pause:
                m_Status.text = "Pausing";
                m_Continue.gameObject.SetActive(true);
                break;
            case CountingState.Alert:
                mAlertTimes = (int)mCurrentTimerData.AlertTimes;
                mWaitingOK = mCurrentTimerData.ManuallyMode;
                m_Pause.interactable = false;
                m_Pause.gameObject.SetActive(true);
                m_OK.gameObject.SetActive(true);
                Alert();
                break;
            default:
                m_Pause.interactable = true;
                m_Pause.gameObject.SetActive(true);
                break;
        }
    }

    void AlertOK()
    {
        if (mWaitingOK)
            return;

        switch (mCurrentCountingValue)
        {
            case CountingValue.RestTime:
                SetCountingValue(CountingValue.Setting);
                SetCountingState(CountingState.Counting);
                break;
            case CountingValue.Workout:
                if (mCurrentSet < mCurrentTimerData.Repeat)
                {
                    mCurrentSet++;
                    SetCountingValue(CountingValue.RestTime);
                    SetCountingState(CountingState.Counting);
                }
                else
                {
                    SetCountingState(CountingState.Complete);
                }
                break;
        }
    }

    void Alert()
    {
        if(mAlertTimes == 0)
        {
            AlertOK();
        }
        else
        {
            switch (mCurrentTimerData.AlertType)
            {
                case AlertType.Vibrate:
                    Handheld.Vibrate();
                    break;
                case AlertType.LowKey:
                    m_AudioSource.PlayOneShot(m_LowKey);
                    break;
                case AlertType.NoisyTone:
                    m_AudioSource.PlayOneShot(m_NoisyTone);
                    break;
            }
            mNextAlertTime = Time.time + 3f;
            mAlertTimes--;
        }
    }

    void SetCountingValue(CountingValue value)
    {
        mCurrentCountingValue = value;

        mCountingTimeSumValue = 0f;
        switch (mCurrentCountingValue)
        {
            case CountingValue.Setting:
                m_Content.text = "Seconds for next set";
                if (mCurrentTimerData.WorkoutDuration != WorkoutDuration.None)
                    mCountingTimeSumValue = (float)mCurrentTimerData.WorkoutDuration;
                break;
            case CountingValue.RestTime:
                m_Content.text = "Resting Time";
                mCountingTimeSumValue = mCurrentTimerData.RestTimeMin * 60f + mCurrentTimerData.RestTimeSec;
                break;
            case CountingValue.Workout:
                m_Content.text = string.Format("Set {0}", mCurrentSet);
                mCountingTimeSumValue = mCurrentTimerData.WorkoutMin * 60f + mCurrentTimerData.WorkoutSec;
                break;
        }
        mCountingTimeLeftValue = mCountingTimeSumValue;

        m_Progress.fillAmount = 1f;
    }
}

public class TimerData
{
    public int WorkoutMin;
    public int WorkoutSec;
    public int RestTimeMin;
    public int RestTimeSec;
    public int Repeat;
    public bool ManuallyMode;
    public AlertType AlertType;
    public AlertTimes AlertTimes;
    public WorkoutDuration WorkoutDuration;
}