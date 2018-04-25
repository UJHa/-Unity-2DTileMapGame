using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct sPosition
{
    public int x;
    public int y;
}
public class MainGameScene : MonoBehaviour {

    public MainGameUI GameUI;
    public TileMap _tileMap;
	// Use this for initialization
	void Start ()
    {
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
        MessageSystem.Instance.ProcessMessage();
        if (Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene("ResultScene");
            GameDataManager.Instance.SetPlayer(_player);
        }
    }
    Character _player;
    void Init()
    {
        _tileMap.Init();
        GameManager.Instance.SetMap(_tileMap);

        _player = CreateCharacter("Player", "character01");
        _player.BecomeViewer();
        for (int i = 0; i < 10; i++)
        {
            CreateCharacter("Monster", "character02");
        }
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
                charGameObject.name = "Player";
                character = charGameObject.AddComponent<Player>();
                break;
            case "Monster":
                charGameObject.name = "Monster";
                character = charGameObject.AddComponent<Monster>();
                break;
        }
        character.Init(resourceName);

        Slider hpGuage = GameUI.CreateHPSlider();
        character.LinkHPGuage(hpGuage);

        Slider coolTimeGuage = GameUI.CreateCoolTimeSlider();
        character.LinkActionCoolTimeGuage(coolTimeGuage);

        Text textLevel = GameUI.CreateLevelText();
        character.LinkTextLevel(textLevel);

        Button atkButton = GameUI.CreateAtkButton();
        character.LinkAtkButton(atkButton);

        Button waitButton = GameUI.CreateWaitButton();
        character.LinkWaitButton(waitButton);

        character.SetCanvasLayer(eTileLayer.MIDDLE);
        return character;
    }
}