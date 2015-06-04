using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class moveNextPanel : MonoBehaviour, IPointerUpHandler {

    public int parentDepth = 2;
    public GameObject nextPanel;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (nextPanel)
        {
            Transform parentObj = gameObject.transform;
            for (int i = 0; i < parentDepth; ++i)
            {
                parentObj = parentObj.parent;
            }
            parentObj.SendMessage("movePanel", nextPanel, SendMessageOptions.DontRequireReceiver);
        }
    }

}
