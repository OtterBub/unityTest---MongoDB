using UnityEngine;
using System.Collections;

public class logoController : MonoBehaviour {

    public GameObject firstLogo;
    public GameObject secondLogo;
    public GameObject fadePanel;
    public GameObject logoPanel;
    public GameObject tutorialCtrl;

	void Start () {
        Invoke("changeLogo", 1.5f);
	}

    void changeLogo()
    {
        secondLogo.SetActive(true);
        firstLogo.SetActive(false);
        Invoke("fade", 2.0f);
    }

    void fade()
    {
        fadePanel.GetComponent<Animator>().SetTrigger("fade");

        if (!PlayerPrefsPlus.GetBool("tutorial"))
        {
            Invoke("displayTutorial", 1.0f);
        }
        else
        {
            Invoke("gotoMain", 1.0f);
        }
    }

    void displayTutorial()
    {
        tutorialCtrl.SetActive(true);
    }

    void gotoMain()
    {
        Application.LoadLevel("main");
    }
}
