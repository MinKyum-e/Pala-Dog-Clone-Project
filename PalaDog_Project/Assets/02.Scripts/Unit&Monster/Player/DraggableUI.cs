
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;// UI�� �ҼӵǾ� �ִ� �ֻ�� canvas transform
    private Transform previousParent; // �ش� ������Ʈ�� ������ �ҼӵǾ� �վ��� �θ� transform
    private RectTransform rect;// UI ��ġ ��� ���� RectTransform
    private CanvasGroup canvasGroup; //UI�� ���İ��� ��ȣ�ۿ� ��� ���� Canvasgroup


    public int unit_idx;

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
        Vector3 spawnPoint;
        spawnPoint = Camera.main.ScreenToWorldPoint( eventData.position );
        Transform playerTransform = GameManager.Instance.player.transform;

        //spawnPoint.y = Mathf.Clamp(spawnPoint.y, playerTransform.position.y + yMin, playerTransform.position.y + yMax);
        spawnPoint.y = playerTransform.position.y;
        spawnPoint.z = GameManager.Instance.player.transform.position.z;


        GameObject unit = GameManager.Instance.pool.Get(unit_idx);
        unit.transform.position = spawnPoint;
        unit.tag = "Unit";
        unit.GetComponent<SpriteRenderer>().sortingOrder = 6;
        /* if (spawnPoint.y > playerTransform.position.y +  yMedian)
             unit.GetComponent<SpriteRenderer>().sortingOrder = 4;
         else
             unit.GetComponent<SpriteRenderer>().sortingOrder=6;*/


        if (transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponentInParent<RectTransform>().position;
            
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
