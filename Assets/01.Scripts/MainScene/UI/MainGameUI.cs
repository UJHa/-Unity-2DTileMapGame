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
    public GameObject levelTextPrefabs;
    public Text CreateLevelText()
    {
        GameObject textObject = GameObject.Instantiate(levelTextPrefabs);
        Text text = textObject.GetComponent<Text>();

        return text;
    }
    public GameObject atkButtonPrefabs;
    public Button CreateAtkButton()
    {
        GameObject btnObject = GameObject.Instantiate(atkButtonPrefabs);
        Button button = btnObject.GetComponent<Button>();

        return button;
    }
    public GameObject waitButtonPrefabs;
    public Button CreateWaitButton()
    {
        GameObject btnObject = GameObject.Instantiate(waitButtonPrefabs);
        Button button = btnObject.GetComponent<Button>();

        return button;
    }
}