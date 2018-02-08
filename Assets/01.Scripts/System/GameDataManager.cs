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
    int _number;
    public void SetNumber(int num)
    {
        _number = num;
    }
    public int GetNumber() { return _number; }
}
