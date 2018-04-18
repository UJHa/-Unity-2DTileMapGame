using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager
{
    //Singleton
    static GameDataManager _instance;
    public static GameDataManager Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new GameDataManager();
                _instance.Init();
            }
            return _instance;
        }
    }
    void Init()
    {
        
    }
    Character _player;
    public void SetPlayer(Character player)
    {
        _player = player;
    }
    public Character GetPlayer()
    {
        return _player;
    }
}
