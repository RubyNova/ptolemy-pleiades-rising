using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BossBehaviour : MonoBehaviour {
    public GameObject _triggerObject;
    public int _health;
    private int _origHealth;
    private GameObject _player;
	private Rigidbody2D _rb;
	public bool _topdown;

	// Use this for initialization
	void Start () 
    {
        _origHealth = _health;
        _player = GameObject.Find("Pleiades");
		_rb = GetComponent<Rigidbody2D>();
		_rb.constraints = RigidbodyConstraints2D.None;
		_rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
	}
	
	// Update is called once per frame
	void Update () {
        if (_triggerObject == null)
        {
			if (_player.transform.position.y < transform.position.y && _rb.velocity.y > -3)
			{
				_rb.AddForce(Vector2.down * 3);
			}
			else if (_player.transform.position.y > transform.position.y && _rb.velocity.y < 3)
			{
				_rb.AddForce(Vector2.up * 3);
			}
			else if (_player.transform.position.y == transform.position.y)
			{
				_rb.velocity = new Vector2(0f, 0f);
			}
            if (_health <= 0)
            {
				GameObject.Find("HUDCanvas").GetComponent<PauseMenuController>().GenericPause("Congratulations - you just beat up a dummy boss, have a cookie. Now, get inside it and find it's memory banks - these will be vital to your mission. Careful though, you only have a few minutes before it self-destructs and fries you!", true);
				Destroy(gameObject);
            }
        }
        else
        {
            _health = _origHealth;
        }

	
	}
}
