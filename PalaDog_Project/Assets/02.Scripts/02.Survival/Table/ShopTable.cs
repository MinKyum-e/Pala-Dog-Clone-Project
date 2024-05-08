namespace ShopEnums {
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



public struct ShopTable 
{
    public int index; //���� ���� �ε���
    public string name; //
    public int group;
    public ShopEnums.ListType grade;
    public int prelist;
    public ShopEnums.GoodsType goodsType;
    public int goodsValue;
    public int value;
}
