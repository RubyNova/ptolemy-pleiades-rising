using UnityEngine;
using System.Collections;

public class DamageIncreasePickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.L)) 
		{
			GameObject player = GameObject.Find("Pleiades");
			player.transform.position = transform.position;
			player.GetComponent<PlatformControls>()._health = 1000;
		}
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            PlatformControls playerControls = coll.gameObject.GetComponent<PlatformControls>();
            playerControls._bladesEnabled = true;
            playerControls._damageToPass += 10;
            GameObject.Find("HUDCanvas").GetComponent<PauseMenuController>().GenericPause("Damage upgrade acquired! Damage increased by 10. Next up is a boss fight. After you've kicked his ass, you'll enter his memory banks - careful, you only have a few minutes inside before it explodes!");
            Destroy(gameObject);
        }
    }
}
