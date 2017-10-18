using UnityEngine;
using System.Collections;

public class PickupTutorialController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			GameObject.Find("HUDCanvas").GetComponent<PauseMenuController>().GenericPause("Welcome! You have just interacted with a pickup - the first few will serve as tutorials as well as upgrades. To move, used A and D. To jump, use W, and to crouch use S. to attack things, use the left mouse button.");
			Destroy(gameObject);
		}
	}
}
