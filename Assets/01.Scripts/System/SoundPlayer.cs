using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    //Singleton
    static SoundPlayer _instance;
    public static SoundPlayer Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<SoundPlayer>();
                if (null == _instance)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "SoundPlayer";
                    _instance = gameObject.AddComponent<SoundPlayer>();
                    DontDestroyOnLoad(gameObject);
                }
            }
            return _instance;
        }
    }
    AudioSource _audioSource;
    public void PlayEffect(string soundName)
    {
        string filePath = "Sounds/Effects/" + soundName;
        AudioClip clip = Resources.Load<AudioClip>(filePath);
        if (null != clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
