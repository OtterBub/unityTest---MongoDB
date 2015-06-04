using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class moveNextPage : MonoBehaviour, IPointerUpHandler {

    public GameObject nextPage;
    public bool isReverse = false;
    public bool isSaveHistory = true;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (nextPage)
        {
            GameObject nowPage = Framework.Instance.getNowPage();
            if (nowPage)
            {
                Framework.Instance.movePage(nowPage, nextPage, isSaveHistory, "slide", isReverse);
            }
            else
            {
                nowPage = transform.parent.parent.gameObject;
                Framework.Instance.movePage(nowPage, nextPage, isSaveHistory, "slide", isReverse);
            }
        }
    }

}
