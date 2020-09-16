using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingData
{
    public static int WorkoutMin
    {
        get
        {
            return PlayerPrefs.GetInt("WorkoutMin", 0);
        }
        set
        {
            PlayerPrefs.SetInt("WorkoutMin", value);
        }
    }

    public static int WorkoutSec
    {
        get
        {
            return PlayerPrefs.GetInt("WorkoutSec", 0);
        }
        set
        {
            PlayerPrefs.SetInt("WorkoutSec", value);
        }
    }

    public static int RestTimeMin
    {
        get
        {
            return PlayerPrefs.GetInt("RestTimeMin", 0);
        }
        set
        {
            PlayerPrefs.SetInt("RestTimeMin", value);
        }
    }

    public static int RestTimeSec
    {
        get
        {
            return PlayerPrefs.GetInt("RestTimeSec", 0);
        }
        set
        {
            PlayerPrefs.SetInt("RestTimeSec", value);
        }
    }
    public static int RepeatTimes
    {
        get
        {
            return PlayerPrefs.GetInt("RepeatTimes", 1);
        }
        set
        {
            PlayerPrefs.SetInt("RepeatTimes", value);
        }
    }

    public static bool ManuallyMode
    {
        get
        {
            return PlayerPrefs.GetInt("ManuallyMode", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("ManuallyMode", value ? 1: 0);
        }
    }

    public static int AlertType
    {
        get
        {
            return PlayerPrefs.GetInt("AlertType", 0);
        }
        set
        {
            PlayerPrefs.SetInt("AlertType", value);
        }
    }

    public static int AlertTimes
    {
        get
        {
            return PlayerPrefs.GetInt("AlertTimes", 1);
        }
        set
        {
            PlayerPrefs.SetInt("AlertTimes", value);
        }
    }

    public static int WorkoutDuration
    {
        get
        {
            return PlayerPrefs.GetInt("WorkoutDuration", 3);
        }
        set
        {
            PlayerPrefs.SetInt("WorkoutDuration", value);
        }
    }

    public static TimerData CurrentTimerData
    {
        get
        {
            TimerData data = new TimerData();

            data.WorkoutMin = WorkoutMin;
            data.WorkoutSec = WorkoutSec;
            data.RestTimeMin = RestTimeMin;
            data.RestTimeSec = RestTimeSec;
            data.Repeat = RepeatTimes;
            data.ManuallyMode = ManuallyMode;
            data.AlertType = (AlertType)AlertType;
            data.AlertTimes = (AlertTimes)AlertTimes;
            data.WorkoutDuration = (WorkoutDuration)WorkoutDuration;

            return data;
        }
    }
}
