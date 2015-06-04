using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class metroBtnController : MonoBehaviour {
    public GameObject loginPanel;

	void Start () {
	
	}

    void Update()
    {

    }

    public void popupMenu(string menu,GameObject btn)
    {
        switch (menu)
        {
            case "login":
            {
                //팝업창 생성
                GameObject login = (GameObject)Instantiate(loginPanel);
                RectTransform loginRect = login.GetComponent<RectTransform>();

                //사이즈 설정
                loginRect.sizeDelta = new Vector2(globalSetup.contentWidth, globalSetup.contentHeight);

                //위치 설정
                loginRect.SetParent(transform.parent);
                loginRect.offsetMin = new Vector2(0, 0);
                loginRect.offsetMax = new Vector2(0, 0);

                //백 버튼과 연결
                globalSetup.backBtn.SetActive(true);
                globalSetup.backBtn.SendMessage("connectPanel", login, SendMessageOptions.DontRequireReceiver);

                //기타 처리
                globalSetup.isExistSlidePanel = true;
            }
            break;
        }
    }
	

}
