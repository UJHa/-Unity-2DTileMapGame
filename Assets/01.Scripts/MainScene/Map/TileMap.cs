using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //InitSpriteList();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //SpriteList
    protected Sprite[] _spriteArray;
    public void Init()
    {
        _spriteArray = Resources.LoadAll<Sprite>("Sprites/MapSprite");
        CreateMap();
    }
    //Tile
    public GameObject TileObjectPrefabs;

    protected int _width;
    protected int _height;

    protected TileCell[,] _tileCellList;

    public int GetWidth()
    {
        return _width;
    }
    public int GetHeight()
    {
        return _height;
    }
    public TileCell GetTileCell(int tileX, int tileY)
    {
        if (0 <= tileX && tileX < _width && 0 <= tileY && tileY < _height)
            return _tileCellList[tileY, tileX];
        return null;
    }
    protected Vector3 _tileScale = Vector3.one;
    public Vector3 GetLocalScale()
    {
        return _tileScale;
    }

    virtual public void CreateMap()
    {
        //CreateTiles();
        CreateRandomMaze();
    }

    void CreateTiles()
    {
        float scale = 1.0f; // 화면에 보이는 크기 배수
        float tileSize = 32.0f * scale;
        _tileScale = new Vector3(scale, scale);

        //1층
        TextAsset scriptAsset = Resources.Load<TextAsset>("Data/Map1Data_layer1");
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

                _tileCellList[y,x] = new TileCell();
                GetTileCell(x, y).Init();
                //setPosition 합치기
                GetTileCell(x, y).SetPosition(x * tileSize / 100.0f, - y * tileSize / 100.0f);
                GetTileCell(x, y).SetTilePosition(x, y);

                GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
            }
        }
        //2층
        scriptAsset = Resources.Load<TextAsset>("Data/Map1Data_layer2");
        records = scriptAsset.text.Split('\n');
        for (int y = 0; y < _height; y++)
        {
            int line = y + 2;
            string[] token = records[line].Split(',');
            for (int x = 0; x < _width; x++)
            {
                int spriteIndex = int.Parse(token[x]);
                if (0 <= spriteIndex)
                {
                    GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                    tileGameObject.transform.SetParent(transform);
                    tileGameObject.transform.localScale = _tileScale;
                    tileGameObject.transform.localPosition = Vector3.zero;

                    TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                    tileObject.Init(_spriteArray[spriteIndex], x, y);
                    tileObject.SetCanMove(false);
                    GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
                }
            }
        }
    }

    void CreateRandomMaze()
    {
        float scale = 1.0f; // 화면에 보이는 크기 배수
        float tileSize = 32.0f * scale;
        _tileScale = new Vector3(scale, scale);
        //1층
        TextAsset scriptAsset = Resources.Load<TextAsset>("Data/Map1Data_layer1");
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
                GetTileCell(x, y).SetPosition(x * tileSize / 100.0f, -y * tileSize / 100.0f);
                GetTileCell(x, y).SetTilePosition(x, y);

                GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
            }
        }
        //2층
        {
            int interval = 2;
            //기둥!
            for (int y = 0; y < _height; y++)
            {
                if (y % interval == 0) 
                {
                    for (int x = 0; x < _width; x++)
                    {
                        if (x % interval == 0)
                        {
                            int spriteIndex = 127;
                            GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                            tileGameObject.transform.SetParent(transform);
                            tileGameObject.transform.localScale = _tileScale;
                            tileGameObject.transform.localPosition = Vector3.zero;

                            TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                            tileObject.Init(_spriteArray[spriteIndex], x, y);
                            tileObject.SetCanMove(false);
                            GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
                        }
                    }
                }
            }
            //가지치기
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if(false==GetTileCell(x,y).CanMove())
                    {
                        sPosition tilePosition;
                        tilePosition.x = x;
                        tilePosition.y = y;
                        // 연결 안된 블록이면
                        if(false == IsConnectedCell(tilePosition))
                        {
                            //랜덤한 방향으로 연결될때까지 이어준다
                            eMoveDirection direction = (eMoveDirection)Random.Range(1, (int)eMoveDirection.DOWN + 1);

                            sPosition curPosition;
                            curPosition.x = x;
                            curPosition.y = y;
                            while(false == IsConnectedCell(curPosition))
                            {
                                int i = 0;
                                while(i < interval-1)
                                {
                                    curPosition = GetDirectionTilePosition((eMoveDirection)direction, curPosition);
                                    if (0 <= curPosition.x && curPosition.x < _width && 0 <= curPosition.y && curPosition.y < _height)
                                    {
                                        int spriteIndex = 127;
                                        GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                                        tileGameObject.transform.SetParent(transform);
                                        tileGameObject.transform.localScale = _tileScale;
                                        tileGameObject.transform.localPosition = Vector3.zero;

                                        TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                                        tileObject.Init(_spriteArray[spriteIndex], curPosition.x, curPosition.y);
                                        tileObject.SetCanMove(false);
                                        GetTileCell(curPosition.x, curPosition.y).AddObject(eTileLayer.GROUND, tileObject);
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            //스프라이트 부드럽게
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    sPosition tilePosition;
                    tilePosition.x = x;
                    tilePosition.y = y;
                    if (true == IsConnectedCell(tilePosition))
                    {

                    }
                }
            }
        }
    }
    private bool IsConnectedCell(sPosition position)
    {
        //주변에 하나라도 붙은 블럭 있음녀 연결된 블럭
        for (int direction = (int)eMoveDirection.LEFT; direction < (int)eMoveDirection.DOWN+1; direction++)
        {
            sPosition curPosition;
            curPosition.x = position.x;
            curPosition.y = position.y;
            sPosition searchPosition = GetDirectionTilePosition((eMoveDirection)direction, curPosition);
            if (0 <= searchPosition.x && searchPosition.x < _width && 0 <= searchPosition.y && searchPosition.y < _height)
            {
                if(false == GetTileCell(searchPosition.x, searchPosition.y).IsPathfindable())
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }
    sPosition GetDirectionTilePosition(eMoveDirection direction, sPosition position)
    {
        switch (direction)
        {
            case eMoveDirection.LEFT: position.x--; break;
            case eMoveDirection.RIGHT: position.x++; break;
            case eMoveDirection.UP: position.y--; break;
            case eMoveDirection.DOWN: position.y++; break;
        }
        return position;
    }
    //Move
    public bool CanMoveTile(int tileX, int tileY)
    {
        if (tileX < 0 || _width <= tileX)
            return false;
        if (tileY < 0 || _height <= tileY)
            return false;

        TileCell tileCell = GetTileCell(tileX, tileY);
        return tileCell.CanMove();
    }

    public List<MapObject> GetCollisionList(int tileX, int tileY)
    {
        if (tileX < 0 || _width <= tileX)
            return null;
        if (tileY < 0 || _height <= tileY)
            return null;
        TileCell tileCell = GetTileCell(tileX, tileY);
        return tileCell.GetCollisionList();
    }

    public void ResetObject(int tileX, int tileY, MapObject mapObject)
    {
        TileCell tileCell = GetTileCell(tileX, tileY);
        tileCell.RemoveObject(mapObject);
    }
    public void SetObject(int tileX, int tileY, MapObject mapObject, eTileLayer layer)
    {
        TileCell tileCell = GetTileCell(tileX, tileY);
        tileCell.AddObject(layer, mapObject);
    }

    //pathFinding
    public void ResetVisit()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                //GetTileCell(x, y).SetVisit(false);
                GetTileCell(x, y).ResetPathFinding();
            }
        }
    }
}