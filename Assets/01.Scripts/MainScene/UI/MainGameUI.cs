using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{

    void Start()
    {

    }
    void Update()
    {

    }
    public GameObject HPGuagePrefabs;
    public Slider CreateHPSlider()
    {
        GameObject hpObject = GameObject.Instantiate(HPGuagePrefabs);
        Slider slider = hpObject.GetComponent<Slider>();

        return slider;
    }
    public GameObject coolTimeGuagePrefabs;
    public Slider CreateCoolTimeSlider()
    {
        GameObject coolTimeObject = GameObject.Instantiate(coolTimeGuagePrefabs);
        Slider slider = coolTimeObject.GetComponent<Slider>();

        return slider;
    }
    public Button button;
    //button action
    public void OnAttack()
    {
        Debug.Log("그만눌러!");
        Character target = GameManager.Instance.targetCharacter;

        MessageParam msgParam = new MessageParam();
        msgParam.sender = null;
        msgParam.receiver = target;
        msgParam.attackPoint = 100;
        msgParam.message = "Attack";

        MessageSystem.Instance.Send(msgParam);
    }
}