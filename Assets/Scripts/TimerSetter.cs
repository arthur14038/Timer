using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SettingTimerCategory
{
    Workout,
    RestTime,
    Repeat,
}

public class TimerSetter : AbstractView
{
    [SerializeField]
    ScrollTimer m_Min;
    [SerializeField]
    ScrollTimer m_Sec;
    [SerializeField]
    ScrollTimer m_Set;
    [SerializeField]
    RectTransform m_ScrollTimerRoot;
    [SerializeField]
    Toggle m_Workout;
    [SerializeField]
    Toggle m_RestTime;
    [SerializeField]
    Toggle m_Repeat;
    [SerializeField]
    Text m_WorkoutMinCount;
    [SerializeField]
    Text m_WorkoutSecCount;
    [SerializeField]
    Text m_RestTimeMinCount;
    [SerializeField]
    Text m_RestTimeSecCount;
    [SerializeField]
    Text m_RepeatTimesCount;
    [SerializeField]
    Button m_Setting;
    [SerializeField]
    Button m_Start;
    [SerializeField]
    RectTransform m_RestTimeGroup;
    [SerializeField]
    RectTransform m_NoRestText;

    SettingTimerCategory mSettingTimerCategory = SettingTimerCategory.Workout;
    ITimerSetterListener mTimerSetterListener = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Setting.onClick.AddListener(OnClickSetting);
        m_Start.onClick.AddListener(OnClickStart);

        m_Workout.onValueChanged.AddListener(OnWorkoutToggleValueChanged);
        m_RestTime.onValueChanged.AddListener(OnRestTimeToggleValueChanged);
        m_Repeat.onValueChanged.AddListener(OnRepeatToggleValueChanged);

        m_Min.OnValueChanged = OnMinValueChanged;
        m_Sec.OnValueChanged = OnSecValueChanged;
        m_Set.OnValueChanged = OnSetValueChanged;

        m_Min.SetCount(60);
        m_Sec.SetCount(60);
        m_Set.SetCount(99, 1);

        m_RestTimeMinCount.text = SettingData.RestTimeMin.ToString();
        m_RestTimeSecCount.text = SettingData.RestTimeSec.ToString();
        m_WorkoutMinCount.text = SettingData.WorkoutMin.ToString();
        m_WorkoutSecCount.text = SettingData.WorkoutSec.ToString();
        m_RepeatTimesCount.text = SettingData.RepeatTimes.ToString();

        UpdateCategory();
    }

    public void SetListener(ITimerSetterListener listener)
    {
        mTimerSetterListener = listener;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnWorkoutToggleValueChanged(bool value)
    {
        if (value)
        {
            mSettingTimerCategory = SettingTimerCategory.Workout;
            UpdateCategory();
        }
    }
    void OnRestTimeToggleValueChanged(bool value)
    {
        if (value)
        {
            mSettingTimerCategory = SettingTimerCategory.RestTime;
            UpdateCategory();
        }
    }
    void OnRepeatToggleValueChanged(bool value)
    {
        if (value)
        {
            mSettingTimerCategory = SettingTimerCategory.Repeat;
            UpdateCategory();
        }
    }

    void UpdateCategory()
    {
        switch (mSettingTimerCategory)
        {
            case SettingTimerCategory.RestTime:
            case SettingTimerCategory.Workout:
                m_Min.gameObject.SetActive(true);
                m_Sec.gameObject.SetActive(true);
                m_Set.gameObject.SetActive(false);
                break;
            case SettingTimerCategory.Repeat:
                m_Min.gameObject.SetActive(false);
                m_Sec.gameObject.SetActive(false);
                m_Set.gameObject.SetActive(true);
                break;
        }
        switch (mSettingTimerCategory)
        {
            case SettingTimerCategory.RestTime:
                m_Min.SetValue(SettingData.RestTimeMin);
                m_Sec.SetValue(SettingData.RestTimeSec);

                m_RestTimeMinCount.text = SettingData.RestTimeMin.ToString();
                m_RestTimeSecCount.text = SettingData.RestTimeSec.ToString();
                break;
            case SettingTimerCategory.Workout:
                m_Min.SetValue(SettingData.WorkoutMin);
                m_Sec.SetValue(SettingData.WorkoutSec);

                m_WorkoutMinCount.text = SettingData.WorkoutMin.ToString();
                m_WorkoutSecCount.text = SettingData.WorkoutSec.ToString();
                break;
            case SettingTimerCategory.Repeat:
                m_Set.SetValue(SettingData.RepeatTimes);

                m_RepeatTimesCount.text = SettingData.RepeatTimes.ToString();
                break;
        }

        if (SettingData.WorkoutMin == 0 && SettingData.WorkoutSec == 0)
        {
            m_Start.interactable = false;
            m_WorkoutMinCount.color = Color.red;
            m_WorkoutSecCount.color = Color.red;
        }
        else
        {
            m_Start.interactable = true;
            m_WorkoutMinCount.color = Color.black;
            m_WorkoutSecCount.color = Color.black;
        }

        if(SettingData.RestTimeMin == 0 && SettingData.RestTimeSec == 0)
        {
            m_RestTimeGroup.gameObject.SetActive(false);
            m_NoRestText.gameObject.SetActive(true);
        }else
        {
            m_RestTimeGroup.gameObject.SetActive(true);
            m_NoRestText.gameObject.SetActive(false);
        }
    }

    void OnMinValueChanged(int value)
    {
        switch(mSettingTimerCategory)
        {
            case SettingTimerCategory.Workout:
                SettingData.WorkoutMin = value;
                break;
            case SettingTimerCategory.RestTime:
                SettingData.RestTimeMin = value;
                break;
        }
        UpdateCategory();
    }

    void OnSecValueChanged(int value)
    {
        switch (mSettingTimerCategory)
        {
            case SettingTimerCategory.Workout:
                SettingData.WorkoutSec = value;
                break;
            case SettingTimerCategory.RestTime:
                SettingData.RestTimeSec = value;
                break;
        }
        UpdateCategory();
    }

    void OnSetValueChanged(int value)
    {
        switch (mSettingTimerCategory)
        {
            case SettingTimerCategory.Repeat:
                SettingData.RepeatTimes = value;
                break;
        }
        UpdateCategory();
    }

    void OnClickSetting()
    {
        mTimerSetterListener.OnClickSetting();
    }

    void OnClickStart()
    {
        mTimerSetterListener.OnClickStart();
    }
}
