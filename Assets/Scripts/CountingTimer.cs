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
    Button m_Resume;
    [SerializeField]
    Button m_Continue;
    [SerializeField]
    Button m_Complete;
    [SerializeField]
    Button m_OK;

    public void SetTimerData(TimerData data)
    {

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