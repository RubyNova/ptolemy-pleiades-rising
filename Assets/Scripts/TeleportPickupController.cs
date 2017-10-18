using UnityEngine;
using System.Collections;

public class TeleportPickupController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") 
		{
			coll.gameObject.GetComponent<PlatformControls> ()._teleportEnabled = true;
			GameObject.Find("HUDCanvas").GetComponent<PauseMenuController>().GenericPause("You can now teleport to your blades! double tap A or D with your cursor near a blade to teleport to it. Try to get through this small test course up ahead to familiarise yourself with everything!");
			Destroy(gameObject);
		}
	}
}
