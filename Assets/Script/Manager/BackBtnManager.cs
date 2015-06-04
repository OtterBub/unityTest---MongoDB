using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackBtnManager : MonoBehaviour {

    //싱글톤 패턴 셋팅
    static BackBtnManager _instance = null;

    public static BackBtnManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("BackManager == null");
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        updateBtn();
        Invoke("updateBtn", 0.25f);
    }

    //변수 선언

    //내부 함수
    void updateBtn()
    {
        if (HistoryManager.Instance.isEmptyHistory())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        Invoke("updateBtn", 0.25f);
    }

    //공용 함수
    public void useBackBtn()
    {

        if (!Framework.Instance.isTranslating())
        {
            int historyCnt = HistoryManager.Instance.getHistoryCnt();
            if (historyCnt > 0)
            {
                GameObject history = HistoryManager.Instance.gameObject;
                GameObject lastPage = history.transform.GetChild(history.transform.childCount - 1).gameObject;

                //만약 현재 페이지가 FirstPage일 경우 가장 최근의 MainPage까지 히스토리 삭제 후 Back한다.
                if (Framework.Instance.getNowPage().CompareTag("FirstPage"))
                {
                    for (int i = 0; i < history.transform.childCount; ++i)
                    {
                        GameObject historyPage = history.transform.GetChild(history.transform.childCount - 1 - i).gameObject;
                        if (!historyPage.CompareTag("MainPage"))
                        {
                            Destroy(historyPage);
                        }
                        else
                        {
                            lastPage = historyPage;
                            break;
                        }
                    }
                }

                historyData transitionData = lastPage.GetComponent<historyData>();
                string transition = transitionData.transition;

                Destroy(transitionData);
                Framework.Instance.movePage(Framework.Instance.getNowPage(), lastPage, false, transition, true);
                Destroy(lastPage);
            }
            else
            {
                //토스트 메세지 후 어플 종료
                Application.Quit();
            }
        }
    }
}
