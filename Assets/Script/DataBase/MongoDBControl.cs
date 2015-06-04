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

	public MongoCollection<T> GetCollection<T>(string collectionName)
	{
		return database.GetCollection<T>(collectionName);
	}

	private MongoDBControl( )
	{
		string connectStr = "mongodb://175.126.82.238:27017";
		client = new MongoClient( connectStr );
		server = client.GetServer( );
		server.Connect( new TimeSpan( 2000 ) );
		database = server.GetDatabase( "meteor" );
	}

	MongoServer server;
	MongoClient client;
	MongoDatabase database;

	private static volatile MongoDBControl _instance;
	private static object syncRoot = new object( );
}
