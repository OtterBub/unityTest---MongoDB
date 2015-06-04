using UnityEngine;
using System.Collections;

public class scaleRetio : MonoBehaviour {

	void Start () {
        GetComponent<RectTransform>().localScale = new Vector3(globalSetup.widthRatio, globalSetup.heightRatio,1);
	}
	
}
