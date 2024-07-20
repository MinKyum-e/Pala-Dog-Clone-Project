
using ShopEnums;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace ShopEnums
{
    public enum ListType
    {
        Enforce,
        Unlock,
        Spawn,
    }

    public enum GoodsType
    {
        Gold,
        Food,
    }

    public enum UnLockType
    {
        InGameUnit,
        Evolution,
    }

    public enum EnforceType
    { 
        Aura = 1001,
        MAX_Cost = 1002,
        Unit_LvL = 1003,
        Gain_Food = 1004,
    }

}

public struct ShopItemInfo
{
    public int idx;
    public string name; //
    public int group;
    public ShopEnums.ListType list_type;
    public int prelist;
    public ShopEnums.GoodsType goods_type;
    public int goods_value;
    public int etc_value;
}
[Serializable]
public struct ShopEnforceEntry
{
    public int price;
    public int value;
}


public class ShopManager : MonoBehaviour
{
    private static ShopManager instance = null;

    private List<int> unlocked_evoluation_unit_list;//��ȭ �ر� unit_list
    public List<int> unlocked_ingame_unit_list;//�ΰ��ӿ��� ���� ��� unit �ر� list
    [SerializeField]
    private Dictionary<EnforceType, List<ShopEnforceEntry>> ingame_enforce_list;
    [SerializeField]
    private Dictionary<EnforceType, int> ingame_enforce_max_lvl;
    public PoolManager minion_poolManager;


    public static ShopManager Instance
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
    private void Awake()
    {
        if (null == instance) //���� ����ó�������Ҷ���
        {
            minion_poolManager = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
            instance = this;
            unlocked_ingame_unit_list = new List<int>();
            unlocked_evoluation_unit_list = new List<int>(); //��������ÿ� ����ǰ� �����ߴٸ� �̰� �ٲٱ�
            ingame_enforce_list = new Dictionary<EnforceType, List<ShopEnforceEntry>>();
            ingame_enforce_max_lvl = new Dictionary<EnforceType, int>();    
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {//�� �̵��ƴµ� ���� �Ŵ����� ������ ��� �ڽ��� ����
            Destroy(gameObject);
        
        }
    }
    private void Start()
    {
        //���� enforce����Ʈ �޾ƿ���
        int enforce_len = Parser.shop_item_info_dict.Count;
        foreach (var item in Parser.shop_item_info_dict)
        {
            if (item.Value.list_type != ShopEnums.ListType.Enforce) continue;


            EnforceType type = (EnforceType)item.Value.group;
            ShopEnforceEntry e;
            e.value = item.Value.etc_value;
            e.price = item.Value.goods_value;

            if (ingame_enforce_max_lvl.ContainsKey(type))
            {
                ingame_enforce_list[type].Add(e);
                ingame_enforce_max_lvl[type]++;
            }
            else
            {
                ingame_enforce_list[type] = new List<ShopEnforceEntry> { e };
                ingame_enforce_max_lvl[type] = 1;
            }
        }
    }

    public int GetEnforceMaxLvL(EnforceType type)
    {
        print(type);
        if (!ingame_enforce_max_lvl.ContainsKey(type))
        {
            print("EnforceIngameBase : containskey error");
            
            return -1;
        }

        return ingame_enforce_max_lvl[type];
    }

    public int GetEnforcePrice(EnforceType type, int lvl)
    {
        if (!ingame_enforce_list.ContainsKey(type))
        {
            print("EnforceIngameBase : containskey error");
        }

        return ingame_enforce_list[type][lvl].price;
    }
    public int GetEnforceValue(EnforceType type, int lvl)
    {
        if (!ingame_enforce_list.ContainsKey(type))
        {
            print("EnforceIngameBase : containskey error");
        }

        return ingame_enforce_list[type][lvl].value;
    }

    /// <summary>
    /// �ΰ��� �ڿ� ���׷��̵�(����, �ڽ�Ʈ, �ķ�, �ķ� ȹ��ӵ�)
    /// param : EnforceType, upgrade lvl
    /// return : ���� ���׷��̵� ����
    /// </summary>
    /// <param name="unlock_type"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public void EnforceIngameBase(EnforceType type, int lvl)
    {
        if (!ingame_enforce_list.ContainsKey(type))
        {
            print("EnforceIngameBase : containskey error");
        }

        switch(type)
        {
            case EnforceType.Aura:
                Player.Instance.SetAuraRange(ingame_enforce_list[type][lvl].value, lvl);
                break;
            case EnforceType.Unit_LvL:
            {
                GameManager.Instance.Unit_LvL = ingame_enforce_list[type][lvl].value;
                for (int i = 0; i < minion_poolManager.pools.Length; i++)
                {
                    foreach (GameObject item in minion_poolManager.pools[i])
                    {
                       if(item.tag != "Player")
                            {
                                Minion minion = item.GetComponent<Minion>();
                                float HP_ratio = Parser.minion_status_dict[minion.actor.ID + GameManager.Instance.Unit_LvL].common.HP / minion.actor.status.HP;
                                float moveSpeed_ratio = Parser.minion_status_dict[minion.actor.ID + GameManager.Instance.Unit_LvL].common.moveSpeed / minion.actor.status.moveSpeed;
                                float atk_ratio = Parser.minion_status_dict[minion.actor.ID + GameManager.Instance.Unit_LvL].common.atk / minion.actor.status.atk;
                                float atkRange_ratio = Parser.minion_status_dict[minion.actor.ID + GameManager.Instance.Unit_LvL].common.atkRange / minion.actor.status.atkRange;
                                float atkSpeed_ratio = Parser.minion_status_dict[minion.actor.ID + GameManager.Instance.Unit_LvL].common.atkSpeed / minion.actor.status.atkSpeed;

                                minion.actor.cur_status.HP = minion.actor.cur_status.HP * HP_ratio;
                                minion.actor.cur_status.moveSpeed = minion.actor.cur_status.moveSpeed * moveSpeed_ratio;
                                minion.actor.cur_status.atk = minion.actor.cur_status.atk * atk_ratio;
                                minion.actor.cur_status.atkRange = minion.actor.cur_status.atkRange * atkRange_ratio;
                                minion.actor.cur_status.atkSpeed = minion.actor.cur_status.atkSpeed * atkSpeed_ratio;

                                minion.actor.status = Parser.minion_status_dict[minion.actor.ID + GameManager.Instance.Unit_LvL].common;
                            }
                      

                        }
                }
                    

                break;
            }
            case EnforceType.MAX_Cost:
            {
                GameManager.Instance.MAX_COST = ingame_enforce_list[type][lvl].value;
                break;
            }
            case EnforceType.Gain_Food:
                GameManager.Instance.food_per_time = ingame_enforce_list[type][lvl].value;
                break;
            default:
                print("EnfroceType error");
                break;
        }
    }
        
    /// <summary>
    /// unlock�� item�� shopindex�� shopmanager unlocked list�� �߰�
    /// �߰� ������ true
    /// </summary>
    /// <param name="unlock_type">unlock �� item�� type</param>
    /// <param name="index">shop index</param>
    /// <returns></returns>
    public bool AddToUnLockList(ShopEnums.UnLockType unlock_type, int index) 
    {
        switch(unlock_type)
        {
            case ShopEnums.UnLockType.InGameUnit:
                
                unlocked_ingame_unit_list.Add(index);
                break;
            case ShopEnums.UnLockType.Evolution:
                unlocked_evoluation_unit_list.Add(index);
                break;
            default:
                return false;
        }
        return true;
    }
    /// <summary>
    /// unlock type�� �´� list���� prerequisite �����ϴ��� Ȯ��. 
    /// true : prerequisite ����
    /// </summary>
    /// <param name=""></param>
    /// <param name="prerequisite"></param>
    /// <returns></returns>
    public bool CheckPrerequisite(ShopEnums.UnLockType unlock_type, int prerequisite)
    {
        switch (unlock_type)
        {
            case ShopEnums.UnLockType.InGameUnit:
                return unlocked_ingame_unit_list.Contains(prerequisite);
            case ShopEnums.UnLockType.Evolution:
                return unlocked_evoluation_unit_list.Contains(prerequisite);
            default:
                return false;
        }
    }

    public void ClearInGameUnlockedList()
    {
        unlocked_ingame_unit_list.Clear();
    }
}
