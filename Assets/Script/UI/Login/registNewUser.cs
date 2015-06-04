using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class registNewUser : MonoBehaviour, IPointerUpHandler
{
    public InputField idField;
    public InputField pwField;
    public InputField pw2Field;

    public GameObject nextPanel;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (nextPanel)
        {
            Transform parentObj = gameObject.transform.parent.parent;
            parentObj.SendMessage("movePanel", nextPanel, SendMessageOptions.DontRequireReceiver);
        }
    }
}
