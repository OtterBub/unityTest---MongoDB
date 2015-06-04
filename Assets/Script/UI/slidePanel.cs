using UnityEngine;
using System.Collections;

public class slidePanel : MonoBehaviour {

    int nowIndex = 0;
    int panelNum = 1;
    bool isSliding = false;
    bool isBack = false;
    bool isExit = false;

    ArrayList slidePanels;

	void Start () {
        slidePanels = new ArrayList();
        slidePanels.Add(transform.GetChild(0).gameObject);
	}
	
	void Update () {
	}

    public void movePanel(GameObject newPanel)
    {
        if (!isSliding)
        {
            //새로운 패널 생성
            GameObject panel = (GameObject)Instantiate(newPanel);
            slidePanels.Add(panel);

            //패널 위치, 크기 설정
            RectTransform rectTran = panel.GetComponent<RectTransform>();
            rectTran.sizeDelta = new Vector2(globalSetup.contentWidth, globalSetup.contentHeight);

            rectTran.SetParent(transform);
            rectTran.offsetMax = new Vector2(0, 0);
            rectTran.offsetMin = new Vector2(0, 0);
            rectTran.localPosition = new Vector2(Screen.width * (slidePanels.Count-1), 0);

            //슬라이드
            Vector3 nowPos = gameObject.GetComponent<RectTransform>().position;
            Vector3 nextPos = new Vector3(nowPos.x - Screen.width, nowPos.y, 0);

            iTween.MoveTo(gameObject,
                iTween.Hash("onComplete", "slideReady", "easetype", "easeOutCubic",
                "position", nextPos, "time", 0.5f));

            isSliding = true;
        }
    }

    public void backPanel()
    {
        if (!isSliding)
        {
            //뒤로 슬라이드
            Vector3 nowPos = gameObject.GetComponent<RectTransform>().position;
            Vector3 prevPos = new Vector3(nowPos.x + Screen.width, nowPos.y, 0);

            iTween.MoveTo(gameObject, iTween.Hash("onComplete", "slideReady", "easetype", "easeOutCubic",
                "position", prevPos, "time", 0.5f));

            isSliding = true;
            isBack = true;

            GameObject currentPanel = (GameObject)slidePanels[slidePanels.Count-1];
            if (slidePanels.Count <= 1) 
            {
                isExit = true;
            }
            else if (currentPanel.GetComponent<slideStartPanel>() != null)
            {
                isExit = true;
                ((GameObject)slidePanels[slidePanels.Count - 2]).SetActive(false);
            }
        }
    }

    void slideReady()
    {
        isSliding = false;

        if (isBack)
        {
            //뒤로 가기였을 경우 앞에 붙어 있는 패널 삭제
            int slidePanelsLength = slidePanels.Count - 1;
            Destroy((GameObject)slidePanels[slidePanelsLength]);
            slidePanels.RemoveAt(slidePanelsLength);
            isBack = false;

            if (isExit)
            {
                exitPanel();
            }
        }
    }

    public void exitPanel()
    {
        while (slidePanels.Count > 0)
        {
            int slidePanelsLength = slidePanels.Count - 1;
            Destroy((GameObject)slidePanels[slidePanelsLength]);
            slidePanels.RemoveAt(slidePanelsLength);
        }
        globalSetup.isExistSlidePanel = false;
        globalSetup.backBtn.SetActive(false);
        Destroy(gameObject);
    }

}
