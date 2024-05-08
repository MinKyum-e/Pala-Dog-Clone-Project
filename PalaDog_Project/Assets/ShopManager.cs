using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
        Evolution
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

public class ShopManager : MonoBehaviour
{
    private static ShopManager instance = null;

    private List<int> unlocked_evoluation_unit_list;//��ȭ �ر� unit_list
    private List<int> unlocked_ingame_unit_list;//�ΰ��ӿ��� ���� ��� unit �ر� list

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
            instance = this;
            unlocked_ingame_unit_list = new List<int>();
            unlocked_evoluation_unit_list = new List<int>(); //��������ÿ� ����ǰ� �����ߴٸ� �̰� �ٲٱ�
            DontDestroyOnLoad(gameObject);
        }
        else
        {//�� �̵��ƴµ� ���� �Ŵ����� ������ ��� �ڽ��� ����
            Destroy(gameObject);
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
