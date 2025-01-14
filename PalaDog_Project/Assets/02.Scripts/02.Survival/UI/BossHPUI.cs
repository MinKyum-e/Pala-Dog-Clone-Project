using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class BossHPUI : MonoBehaviour
{

    private static BossHPUI instance;
    [SerializeField]
    private Actor target;

    public Slider hpBar;
    public Slider hpBarBack;
    float last_hp;
    float cur_hp;
    bool back_hp_hit = false;

    public GameObject HpLineFolder;
    public GameObject HpLineFolderBack;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            target = GameObject.Find("EnemyBase").GetComponent<Actor>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static BossHPUI Instance 
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void setinit()
    {
        last_hp = -1;
        cur_hp = target.cur_status.HP;
        hpBar.value = 1.0f;
        hpBarBack.value = 1.0f;
    }



    public float scale_X;
    void Start()
    {
        scale_X = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (last_hp != target.status.HP)
        {
            last_hp = target.status.HP;
            GetHpBoost();
        }


        float ratio = ((float)target.cur_status.HP / (float)target.status.HP);
        if (ratio >= 0)
        {
            hpBar.value = Mathf.Lerp(hpBar.value, ratio, Time.deltaTime * 5f);
        }
        else
        {
            hpBar.value = Mathf.Lerp(hpBar.value, 0, Time.deltaTime * 5f);
        }
    }


    public void SetTarget(GameObject target)
    {
        this.target = target.GetComponent<Actor>();
        setinit();
    }

    public void GetHpBoost()
    {
        float scaleX = (2000f / target.cur_status.HP) / (target.status.HP / target.cur_status.HP);
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        HpLineFolderBack.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        foreach (Transform child in HpLineFolderBack.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
        HpLineFolderBack.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);

    }
}
