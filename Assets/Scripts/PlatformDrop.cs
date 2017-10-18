using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlatformDrop : MonoBehaviour {
	Vector2 _origin;
	List<GameObject> _children;
	Rigidbody2D _dropBody;
	public float _platformDropDelay;

	// Use this for initialization
	void Start () {
		_dropBody = GetComponent<Rigidbody2D>();
		_children = new List<GameObject>();
         foreach (Transform child in transform)
         {
                 _children.Add(child.gameObject);
         }
		_origin = gameObject.transform.position;
	}


	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			print ("working");
			StartCoroutine(CommenceDrop());
		}
	}

	IEnumerator CommenceDrop()
	{
		foreach (GameObject child in _children)
		{
			SpriteRenderer renderer = child.gameObject.GetComponent<SpriteRenderer>();
			renderer.color = Color.red;
		}
		yield return new WaitForSeconds(_platformDropDelay);
		DropPlatform();
	}

	void DropPlatform()
	{
		_dropBody.isKinematic = false;
	}

	public void Reset(Vector2 playerVector)
	{
		if (transform.position.x > playerVector.x) 
		{
			_dropBody.isKinematic = true;
			transform.position = _origin;
			foreach (GameObject child in _children) {
				child.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
	}
}
