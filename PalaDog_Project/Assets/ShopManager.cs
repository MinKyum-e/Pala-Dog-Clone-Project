using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private List<int> unlocked_evoluation_unit_list;//��ȭ �ر� unit_list
    private List<int> unlocked_ingame_unit_list;//�ΰ��ӿ��� ���� ��� unit �ر� list

    private void Awake()
    {
        unlocked_ingame_unit_list = new List<int>();
        unlocked_evoluation_unit_list = new List<int>(); //��������ÿ� ����ǰ� �����ߴٸ� �̰� �ٲٱ�
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
}
