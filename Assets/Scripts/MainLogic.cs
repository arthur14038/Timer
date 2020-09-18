using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLogic : MonoBehaviour, ITimerSetterListener, ISettingPageListener, ICountingTimerListener
{
    [SerializeField]
    TimerSetter m_TimerSetter;
    [SerializeField]
    SettingPage m_SettingPage;
    [SerializeField]
    CountingTimer m_CountingTimer;

    public void OnClickBack()
    {
        m_SettingPage.Hide();
        m_TimerSetter.Show();
    }

    public void OnClickComplete()
    {
        m_CountingTimer.Hide();
        m_TimerSetter.Show();
    }

    public void OnClickQuit()
    {
        m_CountingTimer.Hide();
        m_TimerSetter.Show();
    }

    public void OnClickSave()
    {

    }

    public void OnClickSetting()
    {
        m_TimerSetter.Hide();
        m_SettingPage.Show();
    }

    public void OnClickStart()
    {
        m_CountingTimer.SetTimerData(SettingData.CurrentTimerData);

        m_TimerSetter.Hide();
        m_CountingTimer.Show();

        m_CountingTimer.StartCounting();
    }

    private void Start()
    {
        m_TimerSetter.Show();
        m_SettingPage.Hide();
        m_CountingTimer.Hide();

        m_TimerSetter.SetListener(this);
        m_SettingPage.SetListener(this);
        m_CountingTimer.SetListener(this);
    }
}
