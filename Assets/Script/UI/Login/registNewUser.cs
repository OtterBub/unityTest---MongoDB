using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class registNewUser : MonoBehaviour, IPointerUpHandler
{
    public InputField idField;
    public InputField pwField;
    public InputField pw2Field;

    public GameObject nowPage;
    public GameObject nextPage;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (nextPage)
        {
            Framework.Instance.movePage(nowPage, nextPage);
        }
    }
}
