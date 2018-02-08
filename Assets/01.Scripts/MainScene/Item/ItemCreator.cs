using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator
{
    //Singleton
    static ItemCreator _instance;
    public static ItemCreator Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new ItemCreator();
                _instance.Init();
            }
            return _instance;
        }
    }
    Sprite[] _spriteArray;
    GameObject _itemPrefabs;
    void Init()
    {
        //string filePath = "Prefabs/ItemView/item_coin";
        string filePath = "Prefabs/ItemView/itemFrame";

        _itemPrefabs = Resources.Load<GameObject>(filePath);
        _spriteArray = Resources.LoadAll<Sprite>("Sprites/item_sprites");
    }

    public void CreateItem(int itemIndex, int tileX, int tileY)
    {
        _itemPrefabs.GetComponent<SpriteRenderer>().sprite = _spriteArray[itemIndex];
        GameObject itemObj = GameObject.Instantiate(_itemPrefabs);
        itemObj.transform.SetParent(GameManager.Instance.GetMap().transform);
        itemObj.transform.localPosition = Vector3.zero;
        itemObj.transform.localScale = GameManager.Instance.GetMap().GetLocalScale();
        Item item = itemObj.AddComponent<Item>();

        item.Init(tileX, tileY);
    }
}
