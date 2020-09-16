using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractView : MonoBehaviour, IView
{
    [SerializeField]
    GameObject m_Root;

    public virtual void Hide()
    {
        m_Root.SetActive(false);
    }

    public virtual void Show()
    {
        m_Root.SetActive(true);
    }
}
