using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadAsset : MonoBehaviour {
	public GameObject goCube;
	public Transform newPosition;
	private string url1="http://127.0.0.1:8080/image";
	private string url2="http://127.0.0.1:8080/cubeasset";
	// Use this for initialization
	void Start () {
		downTexture ();
	}
	void downTexture(){
		StartCoroutine (download_Texture (url1));
		StartCoroutine (down_Pre (url2));
	}

	IEnumerator download_Texture(string url){
		WWW downAsset = new WWW (url);
		yield return downAsset;
		goCube.GetComponent<MeshRenderer> ().material.mainTexture = downAsset.assetBundle.LoadAsset<Texture> ("1 上午8.46.42 上午8.46.42"); 
		downAsset.assetBundle.Unload (false);
		downAsset.Dispose ();

	}
	IEnumerator down_Pre(string url){
		WWW downAsset = new WWW (url);
		yield return downAsset;

		GameObject pre = Instantiate (downAsset.assetBundle.LoadAsset("Cube") , newPosition)as GameObject;
		pre.GetComponent<Renderer> ().material.color = Color.yellow;
		downAsset.assetBundle.Unload (false);
		downAsset.Dispose ();
	}
}
