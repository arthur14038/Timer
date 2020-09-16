using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollTimer : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    RectTransform m_Content;
    [SerializeField]
    Text m_Value;
    [SerializeField]
    ScrollRect m_ScrollRect;
    [SerializeField]
    Text m_Unit;
    bool m_NeedCheckVelocity = false;
    bool m_StartAnchor = false;
    float m_targetPosition = 0f;
    float m_Count = 0f;
    int mMinimun = 0;

    public Action<int> OnValueChanged = delegate { };

    public int Value { get; private set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_ScrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    public void SetCount(int count, int min = 0)
    {
        m_Count = count;
        mMinimun = min;

        for (int i = 0; i < m_Count; ++i)
        {
            var value = Instantiate<Text>(m_Value, m_Content);
            value.text = (i + mMinimun).ToString();
            value.name = m_Value.name + " " + value.text;

            value.gameObject.SetActive(true);
        }
    }

    public void SetValue(int value)
    {
        Value = value - mMinimun;
        if (!m_StartAnchor)
            m_ScrollRect.verticalNormalizedPosition = 1f - (Value / (m_Count-1));
    }

    public void SetUnit(string unitName)
    {
        m_Unit.text = unitName;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_StartAnchor)
        {
            m_ScrollRect.verticalNormalizedPosition = Mathf.Lerp(m_ScrollRect.verticalNormalizedPosition, m_targetPosition, 0.5f);
            if (Mathf.Abs(m_ScrollRect.verticalNormalizedPosition - m_targetPosition) < 0.0001f)
                m_StartAnchor = false;
        }
    }

    void OnScrollValueChanged(Vector2 value)
    {
        Value = Mathf.Clamp((int)((1 - value.y) * (m_Count - 1) + 0.5f), 0, (int)(m_Count - 1));

        if (m_NeedCheckVelocity)
        {
            if (m_ScrollRect.velocity.magnitude < 125f)
            {
                m_targetPosition = 1f - (Value / (m_Count - 1));
                m_StartAnchor = true;
                m_NeedCheckVelocity = false;
                OnValueChanged(Value + mMinimun);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_StartAnchor = false;
        m_NeedCheckVelocity = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_NeedCheckVelocity = true;
    }
}
