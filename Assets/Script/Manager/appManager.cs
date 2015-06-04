using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class appManager : MonoBehaviour {

    public GameObject backBtn;

	void Start () {
        Init();
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    void Init()
    {
        globalSetup.backBtn = backBtn;
    }
}
