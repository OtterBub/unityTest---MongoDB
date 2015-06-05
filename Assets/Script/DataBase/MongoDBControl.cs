using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using UnityEngine.UI;

public class MongoDBControl
{
	static public Text text;

	public static MongoDBControl Instance
	{
		get
		{
			if( _instance == null )
			{
				lock( syncRoot )
				{
					if( _instance == null )
					{
						_instance = new MongoDBControl( );
					}
				}
			}
			return _instance;
		}
	}

	public static void Init( Text txt = null )
	{
		if( init == false )
		{
			init = true;
			if( txt != null ) {
				Instance.debugText = txt;
				Instance.debugText.text = "";
			}
			else
				Debug.Log( "DebugText is null" );
			//connectStr = "mongodb://175.126.82.238:27017";
			Instance.client = new MongoClient( "mongodb://" + Instance.connectStr + Instance.dbPort );

			Instance.server = Instance.client.GetServer( );
			Instance.server.Connect( new TimeSpan( 2000 ) );
			DebugText("Connect");

			Instance.database = Instance.server.GetDatabase( "meteor" );
			DebugText("Get DB");

			Instance.mainPage = Instance.database.GetCollection<BsonDocument>( "mainPage" );
			Instance.fileRecord = Instance.database.GetCollection<BsonDocument>( "cfs.images.filerecord" );
			DebugText("Get Collection");
		}
	}

	static public void DebugText( string str )
	{
		if( Instance.debugText != null )
			Instance.debugText.text += str + '\n';
	}

	static public MongoCollection<T> GetCollection<T>(string collectionName)
	{
		return Instance.database.GetCollection<T>(collectionName);
	}

	static public void PrintBsonValue( BsonValue val )
	{
		switch( val.BsonType )
		{
			case BsonType.Int32:
				Debug.Log( "Int32: " + val.AsInt32 );
				break;
			case BsonType.String:
				Debug.Log( "String: " + val.AsString );
				break;
			case BsonType.Double:
				Debug.Log( "Double: " + val.AsDouble );
				break;
			case BsonType.Null:
				Debug.Log( "Null: " + val.AsBsonNull );
				break;
			case BsonType.Array:
				foreach(  BsonValue arrayVal in val.AsBsonArray )
				{
					PrintBsonValue( arrayVal );
				}
				break;
			case BsonType.Document:
				PrintBsonDocument( val.AsBsonDocument );
				break;
			case BsonType.Undefined:
				break;
		}
	}

	static public void PrintBsonElement( BsonElement val )
	{
		Debug.Log(val.Name + ": ");
		PrintBsonValue( val.Value );
	}

	static public void PrintBsonDocument( BsonDocument doc )
	{
		foreach( BsonElement val in doc )
		{
			PrintBsonElement( val );
		}
	}

	static public string GetFileUrl( string fileKey )
	{
		string dir = Instance.connectStr + Instance.serverPort + "/cfs/files/images/";

		QueryDocument query = new QueryDocument( "_id", fileKey );
		BsonDocument doc = Instance.fileRecord.FindOne( query );

		DebugText("Get File Url: " + doc["original"].AsBsonDocument["name"].AsString );

		return "http://" + dir + fileKey + '/' + doc["original"].AsBsonDocument["name"].AsString;
	}

	private MongoDBControl( )
	{
		
	}

	//string connectStr = "localhost";
	string connectStr = "175.126.82.238";
	string serverPort = ":3000";
	string dbPort = ":27017";
	//string dbPort = ":3001";

	MongoServer server;
	MongoClient client;
	MongoDatabase database;
	MongoCollection<BsonDocument> mainPage;
	MongoCollection<BsonDocument> fileRecord;
	Text debugText;

	static bool init = false;
	private static volatile MongoDBControl _instance;
	private static object syncRoot = new object( );
}
