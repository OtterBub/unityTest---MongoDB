using UnityEngine;
using System.Collections;

public class resizeRetio : MonoBehaviour {

	void Start () {
        RectTransform rectTran = GetComponent<RectTransform>();
        if (rectTran)
        {
            float width = rectTran.rect.width * globalSetup.widthRatio;
            float height = rectTran.rect.height * globalSetup.heightRatio;
            rectTran.sizeDelta = new Vector2(width, height);
        }
	}
	
}
