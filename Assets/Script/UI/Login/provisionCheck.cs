using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class provisionCheck : MonoBehaviour, IPointerUpHandler
{
    public GameObject nextPanel;
    public Toggle agreeToggle;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (nextPanel)
        {
            if (agreeToggle.isOn)
            {
                Transform parentObj = gameObject.transform.parent.parent;
                parentObj.SendMessage("movePanel", nextPanel, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                //약관에 동의해야 합니다.
            }
        }
    }
}
