using UnityEngine;
using System.Collections;

public class AppUtility : MonoBehaviour {

    //싱글톤 패턴 셋팅
    static AppUtility _instance = null;

    public static AppUtility Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("AppUtility is null");
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    //공용 함수
    public void initPageTransform(GameObject page, GameObject parent)
    {
        RectTransform pageRect = page.GetComponent<RectTransform>();
        RectTransform parentRect = parent.GetComponent<RectTransform>();

        pageRect.sizeDelta = new Vector2(parentRect.rect.width * globalSetup.widthRatio, parentRect.rect.height * globalSetup.heightRatio);
        //pageRect.sizeDelta = new Vector2(parentRect.rect.width, parentRect.rect.height);

        pageRect.SetParent(parentRect);
        pageRect.offsetMax = new Vector2(0, 0);
        pageRect.offsetMin = new Vector2(0, 0);
        pageRect.localPosition = new Vector2(0, 0);
        pageRect.localScale = new Vector3(1, 1, 1);
    }
}
