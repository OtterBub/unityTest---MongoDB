using UnityEngine;
using System.Collections;

public class Framework : MonoBehaviour {

    //변수 선언
    public RectTransform appMain;
    bool isMovingPage = false;
    bool isSavePage = true;
    GameObject nowPage;
    GameObject oldPage;

    public GameObject[] menuPage;

    //싱글톤 패턴 셋팅
    static Framework _instance = null;

    public static Framework Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("framework isn't init");
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        nowPage = appMain.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackBtnManager.Instance.useBackBtn();
        }
    }

    void endMovePage()
    {
        _instance.isMovingPage = false;

        //AppMain 패널 및 현재 페이지 위치 초기화
        RectTransform contentTran = _instance.nowPage.transform.FindChild("Content").GetComponent<RectTransform>();
        _instance.appMain.localPosition = Vector2.zero;
        _instance.nowPage.GetComponent<RectTransform>().localPosition = Vector2.zero;
        contentTran.localPosition = new Vector3(0,contentTran.localPosition.y,0);

        //oldPage는 히스토리로 이동
        if (_instance.isSavePage)
        {
            AppUtility.Instance.initPageTransform(oldPage, HistoryManager.Instance.gameObject);
        }
        else
        {
            Destroy(oldPage);
        }
    }

    //공용 함수
    public void movePage(GameObject currentPage, GameObject newPage, bool isSaveNowPageInHistory = true, string transition = "slide", bool reverseTransition = false)
    {
        if (!_instance.isMovingPage)
        {
            //새로운 페이지 생성
            GameObject page = (GameObject)Instantiate(newPage);
            RectTransform pageRect = page.GetComponent<RectTransform>();
            _instance.nowPage = page;
            _instance.oldPage = currentPage;

            //히스토리 데이터 저장
            _instance.isSavePage = isSaveNowPageInHistory;
            if (isSaveNowPageInHistory)
            {
                currentPage.AddComponent<historyData>().transition = transition;
            }

            //페이지 사이즈, 위치 설정
            AppUtility.Instance.initPageTransform(page, appMain.gameObject);

            //슬라이드
            if (transition == "slide")
            {
                Vector3 nextPos;
                if (!reverseTransition)
                {
                    pageRect.localPosition = new Vector2(pageRect.rect.width, 0);
                    nextPos = new Vector3(-pageRect.rect.width, 0, 0);
                }
                else
                {
                    pageRect.localPosition = new Vector2(-pageRect.rect.width, 0);
                    nextPos = new Vector3(pageRect.rect.width, 0, 0);
                }
                iTween.MoveTo(_instance.appMain.gameObject,
                    iTween.Hash("onComplete", "endMovePage", "oncompletetarget", _instance.gameObject, "easetype", "easeOutCubic",
                    "position", nextPos, "time", 0.3f, "islocal", true));
            }
            _instance.isMovingPage = true;
        }        
    }

    public GameObject getNowPage()
    {
        return _instance.nowPage;
    }

    public GameObject getMenuPage(int index)
    {
        return menuPage[index];
    }

    public bool isTranslating()
    {
        return isMovingPage;
    }
}
