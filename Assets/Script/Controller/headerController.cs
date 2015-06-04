using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class headerController : MonoBehaviour {

    public float updateTime = 0.5f;
    public GameObject[] headerList;

    int lastIndex = 0;

	void Start () {
        Invoke("updateHeader", updateTime);
	}

    void updateHeader()
    {
        if (globalSetup.nowDisplayIndex != lastIndex)
        {
            lastIndex = globalSetup.nowDisplayIndex;
            for (int i = 0; i < headerList.Length; ++i)
            {
               headerList[i].SetActive(i == lastIndex);
            }
        }
        Invoke("updateHeader", updateTime);
    }
}
