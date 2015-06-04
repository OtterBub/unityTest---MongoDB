using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class footerBtn : MonoBehaviour, IPointerUpHandler
{
    public int index;
    public GameObject footerNav;
    public GameObject nextPage;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Framework.Instance.isTranslating())
        {
            if (nextPage && globalSetup.footerIndex != index)
            {
                GameObject nowPage = Framework.Instance.getNowPage();
                if (nowPage)
                {
                    if (globalSetup.footerIndex < index)
                    {
                        Framework.Instance.movePage(nowPage, nextPage, false, "slide", false);
                    }
                    else
                    {
                        Framework.Instance.movePage(nowPage, nextPage, false, "slide", true);
                    }
                    globalSetup.footerIndex = index;
                    footerNav.SendMessage("highlightMenu", index, SendMessageOptions.DontRequireReceiver);
                    HistoryManager.Instance.clearHistory();
                }
            }
        }
    }
}
