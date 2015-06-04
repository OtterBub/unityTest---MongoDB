using UnityEngine;
using System.Collections;

public class HistoryManager : MonoBehaviour {

    //싱글톤 패턴 셋팅
    static HistoryManager _instance = null;

    public static HistoryManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("HistoryManager == null");
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        Invoke("updateHistory", 0.25f);
    }

    //변수 선언
    public int historyCnt = 0;

    //내부 함수
    void updateHistory()
    {
        _instance.historyCnt = _instance.transform.childCount;
        Invoke("updateHistory", 0.25f);
    }

    //공용 함수
    public bool isEmptyHistory()
    {
        if (_instance.historyCnt <= 0)
            return true;
        return false;
    }
    public int getHistoryCnt()
    {
        return _instance.historyCnt;
    }
    public void clearHistory()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
