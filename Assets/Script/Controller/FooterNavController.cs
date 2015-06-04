using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class footerBtnData
{
    public string name;
    public string thumbnail;
    public string url;
    public int menu;

    footerBtnData()
    {
        name = "noName";
    }
    footerBtnData(string name, string thumbnail, string url, int menu)
    {
        this.name = name;
        this.thumbnail = thumbnail;
        this.url = url;
        this.menu = menu;
    }
}

public class FooterNavController : MonoBehaviour {

    public footerBtnData[] footerData;
    public GameObject footerBtn;

    /*
      DB에서 footerBar에 아이템에 대한 정보를 가져온다.
     */

	void Start () {
        //버튼 생성
        for (int i = 0; i < footerData.Length; ++i)
        {
            //버튼 생성
            GameObject btn = (GameObject)Instantiate(footerBtn);
            RectTransform tran = btn.GetComponent<RectTransform>();
            RectTransform footerTran = GetComponent<RectTransform>();

            //사이즈 설정
            /* 기본적으로 패널width/3 * scale.x, 패널height/4 * scale.y */
            Vector2 panelSize = new Vector2(footerTran.rect.width * globalSetup.widthRatio,
                footerTran.rect.height * globalSetup.heightRatio);

            float width = panelSize.x / footerData.Length;
            float height = panelSize.y;
            tran.sizeDelta = new Vector2(width, height);

            tran.SetParent(gameObject.transform);

            //앵커 위치 지정
            float xAnchor = (1.0f / (float)footerData.Length) * (float)i;
            tran.pivot = new Vector2(0, 0);
            tran.anchorMin = new Vector2(xAnchor, 0);
            tran.anchorMax = new Vector2(xAnchor, 1);
            tran.anchoredPosition = new Vector2(1, 1);

            //이미지 로딩 및 스프라이트 설정 또는 텍스트 설정
            if (footerData[i].thumbnail != "")
            {
                btn.transform.GetChild(0).GetComponent<Text>().text = "";
                StartCoroutine(downLoadThumbnail(footerData[i].thumbnail, btn.GetComponent<Image>()));
            }
            else
            {
                btn.transform.GetChild(0).GetComponent<Text>().text = footerData[i].name;
            }

            //메뉴 연결
            if (footerData[i].url != "")
            {

            }
            else if (footerData[i].menu != 0)
            {
                connectMenu(btn.GetComponent<Button>(), i, footerData[i].menu);
            }
        }
        highlightMenu(0);
	}

    IEnumerator downLoadThumbnail(string directory, Image targetImage)
    {
        WWW www = new WWW(directory);
        yield return www;

        Texture2D tex = www.texture;
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        targetImage.sprite = newSprite;
        targetImage.color = new Color(1, 1, 1, 1);

        Destroy(targetImage.transform.FindChild("ajaxLoader").gameObject);
    }

    void connectMenu(Button btn, int index, int menu)
    {
        footerBtn script = btn.gameObject.AddComponent<footerBtn>();
        script.nextPage = Framework.Instance.getMenuPage(footerData[index].menu - 1);
        script.index = index;
        script.footerNav = gameObject;
    }

    public void highlightMenu(int index)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject menuBtn = transform.GetChild(i).gameObject;
            if (i == index)
            {
                menuBtn.GetComponent<Image>().color = new Color(0.27f, 0.86f, 0.86f, 1.0f);
            }
            else
            {
                menuBtn.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }
}
