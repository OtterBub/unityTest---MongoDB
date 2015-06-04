using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FooterNavController : MonoBehaviour {

    public GameObject[] menuBtn;
    public GameObject contentPanel;

	void Start () {
	
	}


    public void onSelect(int index)
    {
        contentPanel.SendMessage("slideIndex", index);
    }

    public void highlightMenu(int index)
    {
        for (int i = 0; i < menuBtn.Length; ++i)
        {
            if (i == index)
            {
                menuBtn[i].GetComponent<Image>().color = new Color(0.27f,0.86f,0.86f,1.0f);
            }
            else
            {
                menuBtn[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }
}
