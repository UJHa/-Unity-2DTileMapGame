using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour {
    public GameObject infoText;
    public TitleTileMap _tileMap;
    // Use this for initialization
    void Start () {
        Init();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
            SceneManager.LoadScene("MainScene");
        UpdateInfoText();
    }

    void Init()
    {
        _tileMap.Init();
        GameManager.Instance.SetMap(_tileMap);

        CreateCharacter("Monster", "character01");
        CreateCharacter("Monster", "character02");
        CreateCharacter("Monster", "character02");
        CreateCharacter("Monster", "character02");
        CreateCharacter("Monster", "character03");
        CreateCharacter("Monster", "character03");
        CreateCharacter("Monster", "character03");
    }


    Character CreateCharacter(string fileName, string resourceName)
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

        return character;
    }

    float _renderTime = 0.5f;
    float _renderDeltaTime = 0.0f;
    bool _isRender = true;
    void UpdateInfoText()
    {
        if (_renderDeltaTime < _renderTime)
        {
            infoText.SetActive(_isRender);
            _renderDeltaTime += Time.deltaTime;
        }
        else
        {
            _isRender = !_isRender;
            _renderDeltaTime = 0.0f;
        }
    }
}
