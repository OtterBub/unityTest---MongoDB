﻿using UnityEngine;
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

	static public string GetFileUrl( string fileName )
	{
		QueryDocument query = new QueryDocument( "_id", "csyiQibg5TxMdhA9u" );
		BsonDocument doc = Instance.mainPage.FindOne( query );

		PrintBsonDocument( doc );

		return "";
	}

	private MongoDBControl( )
	{
		//string connectStr = "mongodb://175.126.82.238:27017";
		string connectStr = "mongodb://localhost:3001";
		client = new MongoClient( connectStr );

		server = client.GetServer( );
		server.Connect( new TimeSpan( 2000 ) );
		
		database = server.GetDatabase( "meteor" );
		
		mainPage = database.GetCollection<BsonDocument>("mainPage");
	}

	MongoServer server;
	MongoClient client;
	MongoDatabase database;
	MongoCollection<BsonDocument> mainPage;

	private static volatile MongoDBControl _instance;
	private static object syncRoot = new object( );
}
