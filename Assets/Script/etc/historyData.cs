using UnityEngine;
using System.Collections;

public class historyData : MonoBehaviour {

    public string transition;

	void Start () {
        if (transition == "")
            transition = "slide";
	}
}
