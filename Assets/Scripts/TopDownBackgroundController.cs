using UnityEngine;
using System.Collections;

public class TopDownBackgroundController : MonoBehaviour {
	private Transform _camera;

	// Use this for initialization
	void Start () {
		_camera = GameObject.Find("Main Camera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = _camera.transform.position;
	}
}
