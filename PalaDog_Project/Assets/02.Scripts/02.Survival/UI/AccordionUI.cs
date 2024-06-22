using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AccordionUI: MonoBehaviour
{
    private RectTransform content;  // ������ ����
    private bool isExpanded = false; // Ȯ�� ����
    [SerializeField]
    private List<Transform> items;

    private void Awake()
    {
        content = GetComponent<RectTransform>();
        int cnt = transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            var c = transform.GetChild(i);
            if (c.name != "Toggle")
            {
                items.Add(c);
            }
        }
    }

    public void Toggle()
    {
        isExpanded = !isExpanded;

        if (isExpanded)
        {
            Expand();
        }
        else
        {
            Collapse();
        }
    }

    private void Expand()
    {
        // �������� ������
        DOTween.To(() => content.anchoredPosition, x => content.anchoredPosition = x, new Vector2(0,0), 0.2f)
            .SetEase(Ease.OutSine);
    }

    private void Collapse()
    {
        // �������� ����
        DOTween.To(() => content.anchoredPosition, x => content.anchoredPosition = x, new Vector2(-160,  0), 0.2f)
            .SetEase(Ease.OutSine);

    }
}
