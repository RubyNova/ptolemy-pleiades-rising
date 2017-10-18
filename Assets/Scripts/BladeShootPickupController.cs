using UnityEngine;
using System.Collections;

public class BladeShootPickupController : MonoBehaviour {

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
			coll.gameObject.GetComponent<PlatformControls>()._bladesEnabled = true;
			GameObject.Find("HUDCanvas").GetComponent<PauseMenuController>().GenericPause("Pressing the Right Mouse Button fires a Blade. To aim, use the cursor. once a blade hits the ground or an enemy, it will be stuck. To retrieve it, walk up to it. If an enemy has low enough health, you can regain health by walking up to an impaled enemy. Your blades will not penetrate passable platforms (green ones). The red striped platforms harm you, so do your best to avoid them!");
			Destroy(gameObject);
		}
	}
}
