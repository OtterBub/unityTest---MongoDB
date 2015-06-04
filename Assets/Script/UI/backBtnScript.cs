using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class backBtnScript : MonoBehaviour {

    GameObject connectedPanel;

    public void connectPanel(GameObject panel)
    {
        connectedPanel = panel;
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => panel.GetComponent<slidePanel>().backPanel());
    }
    public void useButton()
    {
        connectedPanel.GetComponent<slidePanel>().backPanel();
    }

    public void useButtonWhenExit()
    {
        connectedPanel.GetComponent<slidePanel>().exitPanel();
    }
}
