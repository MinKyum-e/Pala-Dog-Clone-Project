
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private UnitUnlock locker;
    private Transform canvas;// UI�� �ҼӵǾ� �ִ� �ֻ�� canvas transform
    private Transform previousParent; // �ش� ������Ʈ�� ������ �ҼӵǾ� �վ��� �θ� transform
    private RectTransform rect;// UI ��ġ ��� ���� RectTransform
    private CanvasGroup canvasGroup; //UI�� ���İ��� ��ȣ�ۿ� ��� ���� Canvasgroup
    private PoolManager poolManager;
    private CircleCollider2D auraCollider;
    private Transform food_text_transform;


    public int minion_idx;
    public int requisite_food;
    ShopItemInfo spawn_info;


    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        poolManager = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
        auraCollider = GameObject.FindGameObjectWithTag("Aura").GetComponent<CircleCollider2D>();
        locker = GetComponent<UnitUnlock>();
        food_text_transform = transform.GetChild(0);

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if(locker.is_lock == false )
        {
            previousParent = transform.parent;
            transform.SetParent(canvas);
            transform.SetAsLastSibling();//���� �տ� ���̵��� ������ �ڽ����� ����
            food_text_transform.gameObject.SetActive(false);

            canvasGroup.alpha = 0.0f;
            canvasGroup.blocksRaycasts = false;
            FadeEffect.Instance.gameObject.SetActive(true);
            Time.timeScale = 0.1f;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(locker.is_lock == false)
        {
            float player_y = Camera.main.WorldToScreenPoint(Player.Instance.transform.position).y;
            rect.position = new Vector3(eventData.position.x, player_y + 70f, 0);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            float leftBound = auraCollider.bounds.min.x;
            float rightBount = auraCollider.bounds.max.x;
            if (worldPosition.x >= leftBound && worldPosition.x <= rightBount)
            {
                canvasGroup.alpha = 0.6f;
            }
            else
            {
                canvasGroup.alpha = 0f;
            }
        }
       
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(locker.is_lock == false)
        {
            Vector3 spawnPoint;
            spawnPoint = Camera.main.ScreenToWorldPoint(eventData.position);
            float leftBound = auraCollider.bounds.min.x;
            float rightBount = auraCollider.bounds.max.x;
            //��ȯ�� �ƽ� �ڽ�Ʈ Ȯ��
            if (spawnPoint.x >= leftBound && spawnPoint.x <= rightBount && GameManager.Instance.CheckCost(Parser.minion_status_dict[minion_idx].cost) == true && GameManager.Instance.GetFood() >=requisite_food)
            {
                Transform playerTransform = Player.Instance.transform;

                //spawnPoint.y = Mathf.Clamp(spawnPoint.y, playerTransform.position.y + yMin, playerTransform.position.y + yMax);
                spawnPoint.y = playerTransform.position.y;
                spawnPoint.z = playerTransform.position.z;

                Minion minion = poolManager.Get(minion_idx).GetComponent<Minion>();
                minion.transform.position = new Vector3(spawnPoint.x, spawnPoint.y + 0.4f, Random.Range(-1, 1));
                minion.tag = "Minion";
                minion.GetComponent<SpriteRenderer>().sortingOrder = 4;

                GameManager.Instance.UpdateFood(-requisite_food);
                GameManager.Instance.UpdateCost(minion.cost);
            }


            if (transform.parent == canvas)
            {
                transform.SetParent(previousParent);
                rect.position = previousParent.GetComponentInParent<RectTransform>().position;
                food_text_transform.gameObject.SetActive(true);

            }


            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
            FadeEffect.Instance.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        
    }
}
