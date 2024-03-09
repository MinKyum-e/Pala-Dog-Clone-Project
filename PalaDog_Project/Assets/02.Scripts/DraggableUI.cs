
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;// UI�� �ҼӵǾ� �ִ� �ֻ�� canvas transform
    private Transform previousParent; // �ش� ������Ʈ�� ������ �ҼӵǾ� �վ��� �θ� transform
    private RectTransform rect;// UI ��ġ ��� ���� RectTransform
    private CanvasGroup canvasGroup; //UI�� ���İ��� ��ȣ�ۿ� ��� ���� Canvasgroup

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;

        transform.SetParent(canvas);
        transform.SetAsLastSibling();//���� �տ� ���̵��� ������ �ڽ����� ����

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponentInParent<RectTransform>().position;
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
