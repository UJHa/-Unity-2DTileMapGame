using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTileMap : TileMap
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    override public void CreateMap()
    {
        float scale = 7.0f;
        float tileSize = 32.0f * scale;
        _tileScale = new Vector3(scale, scale);

        //1층
        TextAsset scriptAsset = Resources.Load<TextAsset>("Data/Map1TitleSceneData_layer1");
        string[] records = scriptAsset.text.Split('\n');

        {
            string[] token = records[0].Split(',');
            _width = int.Parse(token[1]);
            _height = int.Parse(token[2]);
        }

        _tileCellList = new TileCell[_height, _width];
        for (int y = 0; y < _height; y++)
        {
            int line = y + 2;
            string[] token = records[line].Split(',');
            for (int x = 0; x < _width; x++)
            {
                int spriteIndex = int.Parse(token[x]);

                GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                tileGameObject.transform.SetParent(transform);
                tileGameObject.transform.localScale = _tileScale;
                tileGameObject.transform.localPosition = Vector3.zero;

                TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                tileObject.Init(_spriteArray[spriteIndex], x, y);

                _tileCellList[y, x] = new TileCell();
                GetTileCell(x, y).Init();
                //setPosition 합치기
                float pixelWidth = _width * tileSize / 100.0f;
                float pixelHeight = _height * tileSize / 100.0f;
                //GetTileCell(x, y).SetPosition(x * tileSize / 100.0f, -y * tileSize / 100.0f);
                GetTileCell(x, y).SetPosition(x * tileSize / 100.0f - pixelWidth / 2.0f, - y * tileSize / 100.0f + pixelHeight / 2.0f);
                GetTileCell(x, y).SetTilePosition(x, y);

                GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
            }
        }
    }
}
