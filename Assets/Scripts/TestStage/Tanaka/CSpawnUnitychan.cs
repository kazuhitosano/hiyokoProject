﻿using UnityEngine;
using System.Collections;

public class CSpawnUnitychan : MonoBehaviour
{
	public GameObject _modelPrefab = null;

	// 出現開始時間
	private float _startTime;
	// 出現間隔
	private const float SPAWN_INTERVAL = 1;
	// 最大出現数
	private const float SPAWN_MAX = 3;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( _modelPrefab != null )
		{
			if( isSpawn() )
			{
				// 出現初期化
				initSpawn();
				// モデル生成
				GameObject unitychan = Instantiate( _modelPrefab,
				            // XZ平面上の画面内にランダム生成
				            Camera.main.ViewportToWorldPoint( new Vector3( Random.value, Random.value, Camera.main.transform.position.y )),
				            new Quaternion() ) as GameObject;
				// オブジェクトルートへ
				unitychan.transform.SetParent( GameObject.Find( "ObjectRoot" ).transform, false );
				/*
				// カメラの中心へむかせる
				Vector3 targetPos = Camera.main.transform.position;
				targetPos.y = 0;
				unitychan.transform.LookAt( targetPos );
				*/
				// スクリプト取得
				UnityChanControlScriptWithRgidBody script = (UnityChanControlScriptWithRgidBody)unitychan.GetComponent( "UnityChanControlScriptWithRgidBody" );
				// 移動量を渡す
				script._moveValue = (float)(0.3f + ( 0.7f * Random.value ));
			}
		}
	}

	/**
	 * 出現初期化
	 */
	private void initSpawn()
	{
		_startTime = Time.time;
	}

	/**
	 * 出現確認
	 */
	private bool isSpawn()
	{
		// 出現間隔確認
		if( _startTime + SPAWN_INTERVAL <= Time.time )
		{
			// 生成済みモデル数確認
			int cnt = 0;
			// typeで指定した型の全てのオブジェクトを配列で取得し,その要素数分繰り返す.
			foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
			{
				// ルートオブジェクトのみ取得
				if( obj.transform.parent == GameObject.Find( "ObjectRoot" ).transform )
				{
					// シーン上に存在するオブジェクトならば処理.
					if (obj.activeInHierarchy)	
					{
						if( obj.name.IndexOf( "unitychan" ) != -1 )
						{
							cnt++;
						}
					}
				}
			}
			// 最大出現数を超えていないなら
			if( cnt < SPAWN_MAX )
			{
				// 出現可
				return true;
			}
		}
		// 出現不可
		return false;
	}
}
