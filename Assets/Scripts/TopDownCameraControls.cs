using UnityEngine;
using System.Collections;

public class TopDownCameraControls : MonoBehaviour {
    public Transform thePlayer;
    Vector3 playerPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (thePlayer != null)
		{
			playerPos = thePlayer.transform.position;
			transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
		}
	}
}
