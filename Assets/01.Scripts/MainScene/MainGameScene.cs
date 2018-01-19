using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameScene : MonoBehaviour {

    public MainGameUI GameUI;
    public TileMap _tileMap;
    //public Player _testPlayer;
	// Use this for initialization
	void Start ()
    {
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
        MessageSystem.Instance.ProcessMessage();
    }
    void Init()
    {
        _tileMap.Init();
        GameManager.Instance.SetMap(_tileMap);

        CreateCharacter("Player", "character01").BecomeViewer();
        CreateCharacter("Monster", "character02");
        //player.BecomeViewer();
    }
    Character CreateCharacter(string fileName,string resourceName)
    {
        //캐릭터 생성
        string filePath = "Prefabs/CharacterFrame/Character";
        GameObject charPrefabs = Resources.Load<GameObject>(filePath);
        GameObject charGameObject = GameObject.Instantiate(charPrefabs);
        charGameObject.transform.SetParent(_tileMap.transform);
        charGameObject.transform.localPosition = Vector3.zero;
        Character character = charGameObject.GetComponent<Player>();
        switch (fileName)
        {
            case "Player":
                character = charGameObject.AddComponent<Player>();
                break;
            case "Monster":
                character = charGameObject.AddComponent<Monster>();
                break;
        }
        character.Init(resourceName);

        Slider hpGuage = GameUI.CreateHPSlider();
        character.LinkHPGuage(hpGuage);

        Slider coolTimeGuage = GameUI.CreateCoolTimeSlider();
        character.LinkCoolTimeGuage(coolTimeGuage);

        return character;
    }
}