using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour {
    public Text playerInfoText;
    // Use this for initialization
    void Start () {
        playerInfoText.text = GameDataManager.Instance.GetPlayer().GetHP().ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
