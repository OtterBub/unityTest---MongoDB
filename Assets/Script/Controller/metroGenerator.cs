using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

// MongoDB
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Wrappers;

[System.Serializable]
public class metroData
{
    public Vector2 position;
    public Vector2 scale;
    public string thumbnail = null;
    public string url = null; 
    public int menu;

    public metroData()
    {
        position = new Vector2(1, 1);
        scale = new Vector2(1, 1);
    }
    public metroData(Vector2 pos, Vector2 scale, string thumbnail, string url, int menu)
    {
        position = pos;
        this.scale = scale;
        this.thumbnail = thumbnail;
        this.url = url;
        this.menu = menu;
    }
}

public class metroGenerator : MonoBehaviour {
    public metroData[] metroBtnData; //최대 12개
    public GameObject metroPrefab;
    public GameObject contentPanel;
    public float padding;
	
	void Start () {
       
		MongoCollection<BsonDocument> coll = MongoDBControl.GetCollection<BsonDocument>( "mainPage" );
		
		MongoCursor<BsonDocument> cur = coll.FindAll();
		foreach( BsonDocument doc in cur )
		{
			BsonValue val;
			if( doc.TryGetValue( "images", out val ) )
			{
				int count = 0;
				metroBtnData = new metroData[val.AsBsonArray.Count];

				for( int i = 0; i < metroBtnData.Length; ++i )
				{
					if( metroBtnData[i] == null )
						metroBtnData[i] = new metroData();
				}

				foreach( BsonValue imagesVal in val.AsBsonArray )
				{
					BsonDocument imageDoc = imagesVal.AsBsonDocument;
					//Debug.Log(imageDoc.ToString());
					Debug.Log( MongoDBControl.GetFileUrl(imageDoc["picture"].AsString) );
					if( metroBtnData[count] != null )
					{
						BsonValue docVal;
						metroBtnData[count].thumbnail = MongoDBControl.GetFileUrl( imageDoc["picture"].AsString );
						metroBtnData[count].position.x = imageDoc["pos"].AsBsonDocument["x"].AsInt32;
						metroBtnData[count].position.y = imageDoc["pos"].AsBsonDocument["y"].AsInt32;
						metroBtnData[count].scale.x = imageDoc["scale"].AsBsonDocument["x"].AsInt32;
						metroBtnData[count].scale.y = imageDoc["scale"].AsBsonDocument["y"].AsInt32;
						if( imageDoc.TryGetValue( "url", out docVal ) ) 
							metroBtnData[count].url = docVal.AsString;
						if( imageDoc.TryGetValue( "menu", out docVal ) )
							metroBtnData[count].menu = docVal.AsInt32;
					}
					
					count++;
				}
			}
		}

        for (int i = 0; i < metroBtnData.Length; ++i)
        {
            //버튼 생성
            GameObject metroBtn = (GameObject)Instantiate(metroPrefab);
            RectTransform metroRectTran = metroBtn.GetComponent<RectTransform>();

            //사이즈 설정
            /* 기본적으로 패널width/3 * scale.x, 패널height/4 * scale.y */
            Vector2 panelSize = new Vector2(contentPanel.GetComponent<RectTransform>().rect.width * globalSetup.widthRatio,
                contentPanel.GetComponent<RectTransform>().rect.height * globalSetup.heightRatio);

            float width = panelSize.x / 3 * metroBtnData[i].scale.x;
            float height = panelSize.y / 4 * metroBtnData[i].scale.y;
            metroRectTran.sizeDelta = new Vector2(width-padding*2, height-padding*2);

            metroRectTran.SetParent(contentPanel.transform);

            //앵커 위치 지정
            /* X앵커: 0.333333f * (열 - 1) x가 열 / y가 행
               Y앵커: 0.25f * (5-행) 
             */
            float xAnchor = 0.333333f * (metroBtnData[i].position.x - 1);
            float yAnchor = 0.25f * (5-metroBtnData[i].position.y);
            metroRectTran.anchorMin = new Vector2(xAnchor, yAnchor);
            metroRectTran.anchorMax = new Vector2(xAnchor, yAnchor);
            metroRectTran.anchoredPosition = new Vector2(padding,-padding);

            //이미지 로딩 및 스프라이트 설정
            StartCoroutine(downLoadThumbnail(metroBtnData[i].thumbnail, metroBtn.GetComponent<Image>()));

            //메뉴 연결
            if (metroBtnData[i].url != "")
            {

            }
            else if(metroBtnData[i].menu != 0)
            {
                connectMenu(metroBtn.GetComponent<Button>(), i, metroBtnData[i].menu);
            }

        }
	}


    IEnumerator downLoadThumbnail(string directory,Image targetImage)
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
        moveNextPage script = btn.gameObject.AddComponent<moveNextPage>();
        script.isSaveHistory = true;
        script.isReverse = false;
        script.nextPage = Framework.Instance.getMenuPage(metroBtnData[index].menu - 1);
    }
}
