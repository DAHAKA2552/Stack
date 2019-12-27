﻿using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private Component[] managers;
    [SerializeField] private Camera camera;

    void Awake()
    {
        InitializationComponents(managers);
    }
    void Start()
    {
        camera = Camera.main;
    }

    void InitializationComponents(Component[] pref)
    {
        for (int i = 0; i < pref.Length; i++) 
        {
            if(pref[i] != null)
            {
                CreateComponents(pref[i]);
            }
        }
    }

    void CreateComponents(Component pref)
    {
        var obj = Instantiate(pref, transform);
        obj.name = pref.name;
    }

    public IEnumerator MoveCamera()
    {
        Vector3 newCameraPosition;
        newCameraPosition = new Vector3(camera.transform.position.x, camera.transform.position.y + 0.25f, camera.transform.position.z);
        while(camera.transform.position  != newCameraPosition)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, newCameraPosition, Time.deltaTime);
            yield return null;
        }
    }

    public void NewGame()
    {
        LevelManager.Instance.CreateStartPlatform();
        PlayerPrefs.SetInt("Score", 0);
    }
}
