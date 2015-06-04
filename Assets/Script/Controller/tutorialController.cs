using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class tutorialController : MonoBehaviour {

    public GameObject logoPanel;
    public GameObject tutorialPanel;
    public int imageCnt;

    public GameObject handlePrefab;
    public GameObject baseImage;

    GameObject imagePanel;
    GameObject handlePanel;

    GameObject[] handle;
    GameObject[] images;
    float handleWidth;
    float panelWidth;
    float panelHeight;

    bool isSliding = false;
    int nowDisplayIndex = 0;
    AnySwipe swipe;

	void Start () {
        swipe = GetComponent<AnySwipe>();
        imagePanel = tutorialPanel.transform.FindChild("ImagePanel").gameObject;
        handlePanel = tutorialPanel.transform.FindChild("HandlePanel").gameObject;

        images = new GameObject[10];
        panelWidth = tutorialPanel.GetComponent<RectTransform>().rect.width;
        panelHeight = tutorialPanel.GetComponent<RectTransform>().rect.height;

        handleWidth = handlePrefab.GetComponent<RectTransform>().rect.width * globalSetup.widthRatio;
        
        //시작위치: padding * (이미지개수-1)의 절반을 음수로
        float padding = 25;
        int startX = (int)-((padding * (imageCnt - 1)) * 0.5f);

        handle = new GameObject[10]; //핸들 최대 개수는 10개
        
        for (int i = 0; i < imageCnt; ++i)
        {
            //이미지 생성
            images[i] = (GameObject)Instantiate(baseImage);
            RectTransform imageRectTran = images[i].GetComponent<RectTransform>();

            images[i].name = "tutorialImage";
            imageRectTran.SetParent(imagePanel.transform);
            imageRectTran.localPosition = new Vector3(panelWidth * i, 0, 0);
            imageRectTran.sizeDelta = new Vector2(panelWidth, panelHeight);
            imageRectTran.localScale = new Vector3(1, 1, 1);

            StringBuilder sb = new StringBuilder("tutorial");
            StartCoroutine(downLoadResource(sb.AppendFormat("{0}", i + 1).ToString(), ".png",
                images[i].GetComponent<Image>()));

            //핸들 생성
            handle[i] = (GameObject)Instantiate(handlePrefab);
            RectTransform handleRectTran = handle[i].GetComponent<RectTransform>();

            handle[i].name = "handle";
            handleRectTran.SetParent(handlePanel.transform);
            handleRectTran.sizeDelta = new Vector2(handleWidth, handleWidth);
            handleRectTran.localPosition = new Vector3(startX + (i * padding), 0, 0);
        }

        updateHandle(0);
        logoPanel.SetActive(false);
        tutorialPanel.SetActive(true);
	}

    void Update()
    {
        if (swipe.IsSwiped())
        {
            if (swipe.CheckForSwipe(Vector2.right))
            {
                movePrev();
            }
            else if (swipe.CheckForSwipe(-Vector2.right))
            {
                moveNext();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator downLoadResource(string directory, string type, Image targetImage)
    {
        StringBuilder sb = new StringBuilder("http://imagetest.meteor.com/");
        sb.Append(directory);
        sb.Append(type);
        WWW www = new WWW(sb.ToString());
        yield return www;

        Texture2D tex = www.texture;
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        targetImage.sprite = newSprite;
        targetImage.color = new Color(1, 1, 1, 1);

        Destroy(targetImage.transform.FindChild("ajaxLoader").gameObject);
    }

    void moveReady()
    {
        isSliding = false;
    }

    public void moveNext()
    {
        if (isSliding == false)
        {
            if (nowDisplayIndex < imageCnt - 1)
            {
                Vector3 nowPos = imagePanel.GetComponent<RectTransform>().position;
                Vector3 nextPos = new Vector3(nowPos.x - Screen.width, nowPos.y, 0);

                iTween.MoveTo(imagePanel, iTween.Hash("onCompletetarget", gameObject, "onComplete", "moveReady", "easetype", "easeOutBack", "position", nextPos, "time", 0.5f));

                isSliding = true;
                nowDisplayIndex++;
                updateHandle(nowDisplayIndex);
            }
            else
            {
                PlayerPrefsPlus.SetBool("tutorial", true);
                Application.LoadLevel("main");
            }
        }
    }
    public void movePrev()
    {
        if (isSliding == false && nowDisplayIndex > 0)
        {
            Vector3 nowPos = imagePanel.GetComponent<RectTransform>().position;
            Vector3 prevPos = new Vector3(nowPos.x + Screen.width, nowPos.y, 0);

            iTween.MoveTo(imagePanel, iTween.Hash("onCompletetarget", gameObject, "onComplete", "moveReady", "easetype", "easeOutBack", "position", prevPos, "time", 0.5f));

            isSliding = true;
            nowDisplayIndex--;
            updateHandle(nowDisplayIndex);
        }
    }

    void updateHandle(int index)
    {
        for (int i = 0; i < imageCnt; ++i)
        {
            if (i == index)
            {
                handle[i].GetComponent<Image>().color = new Color(0, 0, 0, 1);
            }
            else
            {
                handle[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }
}
