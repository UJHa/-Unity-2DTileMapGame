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
}