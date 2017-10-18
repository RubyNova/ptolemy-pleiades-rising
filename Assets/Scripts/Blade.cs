using UnityEngine;
using System.Collections;

public class Blade : MonoBehaviour
{
    public float speed;
	bool _applyForce = true;
	Rigidbody2D _rb;
	GameObject _impaledEnemy;
	private GameObject _impaledGround;
	private AudioSource _soundControl;
	public GameObject _parent;

	void Start()
	{
		_rb = gameObject.GetComponent<Rigidbody2D>();
		_soundControl = GetComponent<AudioSource>();
        _parent = GameObject.Find("Pleiades");
	}

    void Update()
    {
		if (_applyForce)
		{
			transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
		}

	}


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
			_soundControl.Play();
			_impaledEnemy = coll.gameObject;
            EnemyBehaviour enemyBehaviour = _impaledEnemy.GetComponent<EnemyBehaviour>();
            if (enemyBehaviour == null)
            {
                _impaledEnemy.GetComponent<BossBehaviour>()._health -= 25;
            }
            else
            {
                enemyBehaviour._health -= 200;
            }
 
			_applyForce = false;
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.transform.SetParent(_impaledEnemy.transform);
            _rb.isKinematic = true;
        }
		else if (coll.gameObject.tag == "Ground")
		{
			_impaledGround = coll.gameObject;
			_applyForce = false;
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			gameObject.transform.SetParent(_impaledGround.transform);
			_rb.isKinematic = true;
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" && !_applyForce && _impaledEnemy != null)
		{
            EnemyBehaviour enemyBehaviour = _impaledEnemy.gameObject.GetComponent<EnemyBehaviour>();
            if (enemyBehaviour != null)
            {
                if (enemyBehaviour._health <= 400)
                {
					if (coll.gameObject.GetComponent<PlatformControls>() != null)
					{
						coll.gameObject.GetComponent<PlatformControls>()._health += _impaledEnemy.gameObject.GetComponent<EnemyBehaviour>()._health;
					}
					else if (coll.gameObject.GetComponent<TopDownControls>() != null)
					{
						coll.gameObject.GetComponent<TopDownControls>()._health += _impaledEnemy.gameObject.GetComponent<EnemyBehaviour>()._health;
					}
                    _impaledEnemy.gameObject.GetComponent<EnemyBehaviour>()._health += 10000;
					if (coll.transform.Find("RangedCombatHandler") != null)
					{
						coll.transform.Find("RangedCombatHandler").GetComponent<HandleRangedDamage>()._currentAmount -= 1;
					}
                    _impaledEnemy.gameObject.GetComponent<EnemyBehaviour>()._shouldDestroy = true;
                    Destroy(gameObject);
                }
            }
            else
            {
                BossBehaviour bossBehaviour = _impaledEnemy.gameObject.GetComponent<BossBehaviour>();
                if (bossBehaviour._health <= 400)
                {
//                    coll.gameObject.GetComponent<PlatformControls>()._health += bossBehaviour._health;
//                    bossBehaviour._health += 10000;
					if (coll.GetComponent<PlatformControls> () != null) 
					{
						coll.transform.Find ("RangedCombatHandler").GetComponent<HandleRangedDamage> ()._currentAmount -= 1;
					}
                    Destroy(gameObject);
                }
            }


		}
		else if (coll.gameObject.tag == "Player" && !_applyForce && _impaledGround != null)
		{
			if (coll.GetComponent<PlatformControls> () != null) {
				
			
				coll.transform.Find ("RangedCombatHandler").GetComponent<HandleRangedDamage> ()._currentAmount -= 1;
			}
			Destroy(gameObject);
		}
		else if (coll.gameObject.tag == "Player" && _impaledGround == null && _impaledEnemy == null && !_applyForce)
		{
			Destroy(gameObject);
		}
	}

	void OnBecameInvisible()
	{
		Destroy(gameObject);
	}

	void OnDestroy()
	{
		if (_parent != null) 
		{
			if (_parent.GetComponent<PlatformControls> () != null) {
				_parent.transform.Find ("RangedCombatHandler").GetComponent<HandleRangedDamage> ()._currentAmount--;
			}
		}

	}
}
