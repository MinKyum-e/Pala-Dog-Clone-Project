
using System.Collections.Generic;
using UnityEngine;



public class PoolManager : MonoBehaviour
{
    //��������� ������ ����
    public GameObject[] prefabs;

    public Dictionary<int, int> index_dict; //index -> prefab index

    public GameObject player;

    int[] sort_order;



    //Ǯ ����� �ϴ� ����Ʈ��
    public List<GameObject>[] pools;
    private void Awake()
    {
        if (gameObject.name == "EnemyPool" && GameObject.Find("EnemyPool") != gameObject)
        {
            Destroy(gameObject);
        }
        if(gameObject.name == "MinionPool"  && GameObject.Find("MinionPool") != gameObject)
        {
            Destroy(gameObject);
        }
        
        if (gameObject.name == "MinionPool")
        {
            pools = new List<GameObject>[prefabs.Length + 1];
        }
        else
        {
            pools = new List<GameObject>[prefabs.Length ];
        }
        

        for (int i=0;i<pools.Length;i++)
        {
            pools[i] =new List<GameObject>();
        }
        index_dict = new Dictionary<int, int>();
        sort_order = new int[prefabs.Length];
        DontDestroyOnLoad(gameObject);

        
    }
    public void Start()
    {

        if (gameObject.name == "MinionPool")
        {
            pools[pools.Length - 1].Add(GameObject.Find("Player"));
        }

        
        for (int i = 0; i < prefabs.Length; i++)
        {
            int id = prefabs[i].gameObject.GetComponent<Actor>().ID;
            index_dict[id] = i;
        }

        SetPool();

    } 
    public int GetUnitCount(int id)
    {
        return pools[index_dict[id]].Count;
    }

    public void SetPool()
    {
        for(int i=0;i<prefabs.Length;i++)
        {

            for(int j=0;j<5;j++)
            {
                if (prefabs[i].name.Contains("Boss"))
                    break;
                var new_object = Instantiate(prefabs[i], transform);
                var sr = new_object.GetComponent<SpriteRenderer>();


                new_object.SetActive(false);

                int prev = sr.sortingOrder;
                sr.sortingOrder = sr.sortingOrder + sort_order[i];
                new_object.transform.position = new Vector3(-100, -100 + 0.4f, 1f - (prev / 1000) - (sort_order[i] / 20f));
                sort_order[i] += 10;
                pools[i].Add(new_object);
            }
         
        }
      
    }

    public GameObject Get(int ID, Vector3 spawnPoint)
    {
        GameObject select = null;


        for(int i=0;i< pools[index_dict[ID]].Count;i++)
        {
            if (!pools[index_dict[ID]][i].activeSelf)
            {
                select = pools[index_dict[ID]][i];
                var sr = select.GetComponent<SpriteRenderer>();
                int prev = sr.sortingOrder;
                select.transform.position = new Vector3(spawnPoint.x, spawnPoint.y + 0.4f, select.transform.position.z);
                select.SetActive(true);
                break;
            }
        }
        if (select == null)
        {
            select = Instantiate(prefabs[index_dict[ID]], transform);
            var sr = select.GetComponent<SpriteRenderer>();

            int prev = sr.sortingOrder;
            sr.sortingOrder = sr.sortingOrder + sort_order[index_dict[ID]];
            select.transform.position = new Vector3(spawnPoint.x, spawnPoint.y + 0.4f, 1f - (prev/1000) - (sort_order[index_dict[ID]] / 20f));
            sort_order[index_dict[ID]] += 10;



            pools[index_dict[ID]].Add(select);
        }

        return select;
    }

    public void ResetPool()
    {
        for (int i = 0; i < sort_order.Length; i++)
            sort_order[i] = 0;
        for(int i=0;i<pools.Length;i++)
        {
            foreach(GameObject item in pools[i])
            {
                Destroy(item.gameObject);
            }
        }
    }

}
