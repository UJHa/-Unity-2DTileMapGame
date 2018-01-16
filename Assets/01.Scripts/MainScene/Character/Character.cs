using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MapObject {

    public enum eMoveDirection
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }

    protected GameObject _characterView;
    protected int _tileX = 0;
    protected int _tileY = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Init(string viewName)
    {
        //View를 붙인다.
        string filePath = "Prefabs/CharacterView/" + viewName;
        GameObject characterViewPrefabs = Resources.Load<GameObject>(filePath);
        _characterView = GameObject.Instantiate(characterViewPrefabs);
        _characterView.transform.SetParent(transform);
        _characterView.transform.localPosition = Vector3.zero;
        _characterView.transform.localScale = Vector3.one;

        TileMap map = GameManager.Instance.GetMap();
        _tileX = Random.Range(1, map.GetWidth() - 2);
        _tileY = Random.Range(1, map.GetHeight() - 2);
        //TileCell tileCell = map.GetTileCell(_tileX, _tileY);
        //tileCell.AddObject(eTileLayer.MIDDLE, this);
        map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
    }

    override public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _tileLayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());

        _characterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _characterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }
}
