using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BetterToggleGroup : ToggleGroup
{
    [SerializeField]
    Toggle[] m_AllToggles;

    public delegate void ChangedEventHandler(int index);
    public event ChangedEventHandler OnChange;

    protected override void Start()
    {
        foreach (Toggle toggle in m_AllToggles)
        {
            toggle.onValueChanged.AddListener((isSelected) => {
                if (!isSelected)
                {
                    return;
                }
                var activeToggle = Active();
                DoOnChange(activeToggle);
            });
        }
    }

    public Toggle Active()
    {
        return ActiveToggles().FirstOrDefault();
    }

    public void SetToggleActive(int index)
    {
        m_AllToggles[index].isOn = true;
    }

    protected virtual void DoOnChange(Toggle newactive)
    {
        int index = -1;

        for(int i = 0; i < m_AllToggles.Length; ++i)
            if(m_AllToggles[i] == newactive)
            {
                index = i;
                break;
            }    

        if(index >=0 )
            OnChange?.Invoke(index);
    }
}
