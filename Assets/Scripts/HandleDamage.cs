using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandleDamage : MonoBehaviour {
	public bool _uppercut;
	GameObject _parent;
	float _jumpHeight;
	Rigidbody2D _parentRb;
	public bool _kickDown;
	PlatformControls _controller;
	int _damage = 0;
    public Dictionary<GameObject, float> _enemies;
	internal bool _isSmackingDown;
	private Collider2D _trig;
	private AudioSource _parentAudioController;

	// Use this for initialization
	void Start () {
		_parent = gameObject.transform.parent.gameObject;
		_controller = _parent.GetComponent<PlatformControls>();
		_jumpHeight = _controller._jumpHeight;
		_parentRb = _parent.GetComponent<Rigidbody2D>();
        _enemies = new Dictionary<GameObject, float>();
		_trig = gameObject.GetComponent<Collider2D>();
		_parentAudioController = _parent.GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		if (_enemies != null)
		{
			if (_enemies.Keys.Count != 0)
			{
				List<GameObject> keys = new List<GameObject>(_enemies.Keys);
				foreach (GameObject key in keys)
				{
					if (_enemies[key] > 5f)
					{
						if (key != null)
						{
							key.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
							key.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
						}
						_enemies.Remove(key);
					}
					else
					{
                        if (key != null)
                        {
                            _enemies[key] += Time.deltaTime;
                            EnemyBehaviour behaviour = key.GetComponent<EnemyBehaviour>();
                            if (behaviour != null)
                            {
                                if (behaviour._isInUppercut)
                                {
                                    if (_controller._numberOfPresses > 5)
                                    {
                                        behaviour._isInUppercut = false;
                                    }
                                    else
                                    {
                                        key.transform.position = new Vector2(key.transform.position.x, _parent.transform.position.y);
                                    }

                                }
                            }

                        }

					}
				}
			}
		}
	}

    void OnTriggerStay2D(Collider2D coll) //here
    {
		if (coll.tag == "Enemy")
		{		
			_damage = _controller._damageToPass;
			EnemyBehaviour enemyController = coll.GetComponent<EnemyBehaviour>();
			Rigidbody2D collRb = coll.GetComponent<Rigidbody2D>();
			collRb.constraints = RigidbodyConstraints2D.None;
			collRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            if (enemyController != null)
            {
                enemyController._health -= _damage;
            }
            else
            {
                coll.GetComponent<BossBehaviour>()._health -= _damage;
            }

			if (!_enemies.ContainsKey(coll.gameObject))
			{
				_enemies.Add(coll.gameObject, 0f);
			}
			else
			{
				_enemies[coll.gameObject] = 0f;
			}
			if (_uppercut)
			{
				if (!_parentAudioController.isPlaying)
				{
					_parentAudioController.clip = _controller._bonusUpperCutClip;
					_parentAudioController.loop = false;
					_parentAudioController.Play();
				}
				_parentRb.AddForce(Vector2.up * _jumpHeight);
				foreach (KeyValuePair<GameObject, float> keyVal in _enemies)
				{
					if (keyVal.Key != null)
					{
						Collider2D otherColl = keyVal.Key.GetComponent<Collider2D>();
						if (_trig.IsTouching(otherColl))
						{
                            //Debug.Break();
                            //otherColl.GetComponent<Rigidbody2D>().AddForce(Vector2.up * _jumpHeight);
                            if (otherColl.GetComponent<EnemyBehaviour>() != null)
                            {
                                otherColl.GetComponent<EnemyBehaviour>()._isInUppercut = true;
                                otherColl.gameObject.GetComponent<EnemyBehaviour>()._health -= _damage;
                            }
                            else
                            {
                                otherColl.gameObject.GetComponent<BossBehaviour>()._health -= _damage;
                            }

						}
					}

				}
				_uppercut = false;
			}
			else if (_kickDown)
			{
				foreach (KeyValuePair<GameObject, float> keyVal in _enemies)
				{
					if (keyVal.Key != null)
					{
						Collider2D otherColl = keyVal.Key.GetComponent<Collider2D>();
						if (_trig.IsTouching(otherColl))
						{
                            if (otherColl.GetComponent<EnemyBehaviour>() != null)
                            {
                                otherColl.GetComponent<EnemyBehaviour>()._isInUppercut = false;
                                otherColl.GetComponent<Rigidbody2D>().AddForce(Vector2.down * _jumpHeight * 5);
                                otherColl.gameObject.GetComponent<EnemyBehaviour>()._health -= _damage;
                            }
                            else
                            {
                                otherColl.GetComponent<BossBehaviour>()._health -= 5;
                            }

						}
					}

				}
				_parentRb.isKinematic = true;
				_kickDown = false;
			}
		}
		else if (coll.tag == "Ground")
		{
			_isSmackingDown = false;

		}
		//Destroy(coll.gameObject);
	}
}
