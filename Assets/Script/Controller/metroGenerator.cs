using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

// MongoDB
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
	public string thumbnail;
	public string connectType; //url, 또는 menu
	public string typeInfo;

	metroData( )
	{
		position = new Vector2( 1, 1 );
		scale = new Vector2( 1, 1 );
	}
	metroData( Vector2 pos, Vector2 scale, string thumbnail, string connectType, string typeInfo )
	{
		position = pos;
		this.scale = scale;
		this.thumbnail = thumbnail;
		this.connectType = connectType;
		this.typeInfo = typeInfo;
	}
}

public class metroGenerator : MonoBehaviour
{
	public metroData[] metroBtnData; //최대 12개
	public GameObject metroPrefab;
	public GameObject contentPanel;
	public float padding;
	public UnityEngine.UI.Text text;

	void Start( )
	{
		MongoCollection<BsonDocument> coll = MongoDBControl.Instance.GetCollection<BsonDocument>( "imagePosts" );
		MongoCursor<BsonDocument> cur = coll.FindAll();
		if ( ( MongoDBControl.text == null ) && ( text != null ) )
			MongoDBControl.text = text;
		MongoDBControl.text.text = "";

		foreach( BsonDocument doc in cur )
		{
			BsonValue val;
			if( doc.TryGetValue("images", out val) )
			{
				BsonArray array = val.AsBsonArray;
				foreach( BsonDocument image in array )
				{
					BsonValue imageVal;
					if( image.TryGetValue( "title", out imageVal ) )
					{
						Debug.Log( "title: " + imageVal.AsString );
						MongoDBControl.text.text += "title: " + imageVal.AsString + " " + '\n';
					}
					if( image.TryGetValue( "pos", out imageVal ) )
					{
						BsonValue posX, posY;
						if( imageVal.AsBsonDocument.TryGetValue("x", out posX) &&
							imageVal.AsBsonDocument.TryGetValue( "y", out posY ) )
						{
							Debug.Log( "pos: " + posX.AsInt32 + ", " + posY.AsInt32 );
						}
					}
					if( image.TryGetValue( "scale", out imageVal ) )
					{
						BsonValue scaleX, scaleY;
						if( imageVal.AsBsonDocument.TryGetValue("x", out scaleX) &&
							imageVal.AsBsonDocument.TryGetValue( "y", out scaleY ) )
						{
							Debug.Log( "scale: " + scaleX.AsInt32 + ", " + scaleY.AsInt32 );
						}
					}
					if( image.TryGetValue( "picture", out imageVal ) )
					{
						Debug.Log( "picture: " + imageVal.AsString );
						MongoDBControl.text.text += "picture: " + imageVal.AsString + " " + '\n';
					}
				}
			}
		}


		for( int i = 0; i < metroBtnData.Length; ++i )
		{
			//버튼 생성
			GameObject metroBtn = ( GameObject )Instantiate( metroPrefab );
			RectTransform metroRectTran = metroBtn.GetComponent<RectTransform>( );
			metroRectTran.SetParent( contentPanel.transform );

			//사이즈 설정
			/* 기본적으로 패널width/3 * scale.x, 패널height/4 * scale.y */
			Vector2 panelSize = new Vector2( contentPanel.GetComponent<RectTransform>( ).rect.width * globalSetup.widthRatio,
				contentPanel.GetComponent<RectTransform>( ).rect.height * globalSetup.heightRatio );

			float width = panelSize.x / 3 * metroBtnData[i].scale.x;
			float height = panelSize.y / 4 * metroBtnData[i].scale.y;
			metroRectTran.sizeDelta = new Vector2( width - padding * 2, height - padding * 2 );

			//앵커 위치 지정
			/* X앵커: 0.333333f * (열 - 1) x가 열 / y가 행
			   Y앵커: 0.25f * (5-행) 
			 */
			float xAnchor = 0.333333f * ( metroBtnData[i].position.x - 1 );
			float yAnchor = 0.25f * ( 5 - metroBtnData[i].position.y );
			metroRectTran.anchorMin = new Vector2( xAnchor, yAnchor );
			metroRectTran.anchorMax = new Vector2( xAnchor, yAnchor );
			metroRectTran.anchoredPosition = new Vector2( padding, -padding );

			//이미지 로딩 및 스프라이트 설정
			StartCoroutine( downLoadThumbnail( metroBtnData[i].thumbnail, metroBtn.GetComponent<Image>( ) ) );

			//메뉴 연결
			if( metroBtnData[i].connectType == "menu" )
			{
				connectMenu( metroBtn.GetComponent<Button>( ), metroBtnData[i].typeInfo );
			}
			else if( metroBtnData[i].connectType == "url" )
			{

			}

		}
	}


	IEnumerator downLoadThumbnail( string directory, Image targetImage )
	{
		WWW www = new WWW( directory );
		yield return www;

		Texture2D tex = www.texture;
		Sprite newSprite = Sprite.Create( tex, new Rect( 0, 0, tex.width, tex.height ), new Vector2( 0.5f, 0.5f ) );
		targetImage.sprite = newSprite;
		targetImage.color = new Color( 1, 1, 1, 1 );

		Destroy( targetImage.transform.FindChild( "ajaxLoader" ).gameObject );
	}

	void connectMenu( Button btn, string menu )
	{
		btn.onClick.RemoveAllListeners( );
		btn.onClick.AddListener( ( ) => contentPanel.GetComponent<metroBtnController>( ).popupMenu( menu, btn.gameObject ) );
	}
}
