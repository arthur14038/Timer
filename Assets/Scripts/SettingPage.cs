using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AlertType
{
    NoisyTone,
    LowKey,
    Vibrate
}

public enum AlertTimes
{
    Once = 1,
    Twice = 2,
    Forever = -1
}

public enum WorkoutDuration
{
    ThreeSec = 3,
    FiveSce = 5,
    None = -1
}

public class SettingPage : AbstractView
{
    [SerializeField]
    Button m_Back;
    [SerializeField]
    Button m_Save;
    [SerializeField]
    Toggle m_ManuallyMode;
    [SerializeField]
    BetterToggleGroup m_AlertTypeToggleGroup;
    [SerializeField]
    BetterToggleGroup m_AlertTimesToggleGroup;
    [SerializeField]
    BetterToggleGroup m_WorkoutDurationToggleGroup;

    bool mCurrentManuallyMode = false;
    AlertType mCurrentAlertType = AlertType.NoisyTone;
    AlertTimes mCurrentAlertTimes = AlertTimes.Once;
    WorkoutDuration mCurrentWorkoutDuration = WorkoutDuration.ThreeSec;
    ISettingPageListener mSettingPageListener = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Back.onClick.AddListener(OnClickBack);
        m_Save.onClick.AddListener(OnClickSave);

        m_AlertTypeToggleGroup.OnChange += OnAlertTypeChanged;
        m_AlertTimesToggleGroup.OnChange += OnAlertTimesChanged;
        m_WorkoutDurationToggleGroup.OnChange += OnWorkoutDurationChanged;
        m_ManuallyMode.onValueChanged.AddListener(OnManuallyModeValueChanged);
    }

    private void OnEnable()
    {
        mCurrentManuallyMode = SettingData.ManuallyMode;
        mCurrentAlertType = (AlertType)SettingData.AlertType;
        mCurrentAlertTimes = (AlertTimes)SettingData.AlertTimes;
        mCurrentWorkoutDuration = (WorkoutDuration)SettingData.WorkoutDuration;

        m_ManuallyMode.isOn = mCurrentManuallyMode;
        m_AlertTypeToggleGroup.SetToggleActive(GetAlertTypeIndex(mCurrentAlertType));
        m_AlertTimesToggleGroup.SetToggleActive(GetAlertTimesIndex(mCurrentAlertTimes));
        m_WorkoutDurationToggleGroup.SetToggleActive(GetWorkoutDurationIndex(mCurrentWorkoutDuration));
    }

    private void OnDisable()
    {
        
    }

    public void SetListener(ISettingPageListener listener)
    {
        mSettingPageListener = listener;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnManuallyModeValueChanged(bool value)
    {
        mCurrentManuallyMode = value;
    }

    void OnAlertTypeChanged(int index)
    {
        mCurrentAlertType = (AlertType)index;
    }

    void OnAlertTimesChanged(int index)
    {
        switch (index)
        {
            case 0:
                mCurrentAlertTimes = AlertTimes.Once;
                break;
            case 1:
                mCurrentAlertTimes = AlertTimes.Twice;
                break;
            case 2:
                mCurrentAlertTimes = AlertTimes.Forever;
                break;
        }
    }

    void OnWorkoutDurationChanged(int index)
    {
        switch (index)
        {
            case 0:
                mCurrentWorkoutDuration = WorkoutDuration.ThreeSec;
                break;
            case 1:
                mCurrentWorkoutDuration = WorkoutDuration.FiveSce;
                break;
            case 2:
                mCurrentWorkoutDuration = WorkoutDuration.None;
                break;
        }
    }

    void OnClickBack()
    {
        mSettingPageListener.OnClickBack();
    }

    void OnClickSave()
    {
        SettingData.ManuallyMode = mCurrentManuallyMode;
        SettingData.AlertType = (int)mCurrentAlertType;
        SettingData.AlertTimes = (int)mCurrentAlertTimes;
        SettingData.WorkoutDuration = (int)mCurrentWorkoutDuration;

        mSettingPageListener.OnClickSave();
    }

    int GetAlertTypeIndex(AlertType type)
    {
        return (int)type;
    }

    int GetAlertTimesIndex(AlertTimes type)
    {
        switch(type)
        {
            case AlertTimes.Twice:
                return 1;
            case AlertTimes.Forever:
                return 2;
            case AlertTimes.Once:
            default:
                return 0;
        }
    }

    int GetWorkoutDurationIndex(WorkoutDuration type)
    {
        switch (type)
        {
            case WorkoutDuration.FiveSce:
                return 1;
            case WorkoutDuration.None:
                return 2;
            case WorkoutDuration.ThreeSec:
            default:
                return 0;
        }
    }
}
