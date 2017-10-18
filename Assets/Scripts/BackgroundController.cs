using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {
	private GameObject _camera;


	// Use this for initialization
	void Start () {
		_camera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector2(transform.position.x, _camera.transform.position.y);
	}
}
