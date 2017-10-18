using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnemyBehaviour : MonoBehaviour
{
	public int _health;
	public bool _shouldDestroy = false;
	Rigidbody2D _rb;
	public Transform _groundCheck;
	public LayerMask _whatIsGround;
	public float _groundRadius;
	public bool _grounded = false;
	public bool _smashed;
	public bool _shouldNotMove;
	Collider2D _collider;
	public bool _isImpaled = false;
	public int _enemySpeed;
	private int _rangedEnemySpeed;
	GameObject _player;
	public LayerMask _mask;
	private float _timeSinceLastDamageOutput;
	public RaycastHit2D _rayResult;
	private Transform _leftGroundCheck;
	private Transform _rightGroundCheck;
	public bool _isRanged;
    public bool _isInUppercut;
	public bool _topdown;

	// Use this for initialization
	void Start()
	{
		if (_isRanged)
		{
			_leftGroundCheck = transform.Find("LeftGroundCheck");
			_rightGroundCheck = transform.Find("RightGroundCheck");
			_rangedEnemySpeed = _enemySpeed;
		}
		_rb = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
		_player = GameObject.Find("Pleiades");
	}

	// Update is called once per frame
	void Update()
	{
		if (_player != null)
		{
			Vector2 direction = (_player.transform.position - transform.position).normalized;
			_grounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _whatIsGround);
			_rayResult = Physics2D.Raycast(transform.position, direction, Vector2.Distance(transform.position, _player.transform.position), _mask);

			if (_isImpaled)
			{
				_shouldNotMove = true;
				_rb.constraints = RigidbodyConstraints2D.None;
				_rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
			}
			if (!_topdown)
			{
				if (!_isRanged)
				{
					if (_rayResult.collider != null)
					{
						if (_rayResult.collider.gameObject.tag == "Player")
						{
							HandleMove(_player);
						}
					}
				}
				else if (_grounded && !_shouldNotMove)
				{
					bool leftGrounded = Physics2D.OverlapCircle(_leftGroundCheck.position, _groundRadius, _whatIsGround);
					bool rightGrounded = Physics2D.OverlapCircle(_rightGroundCheck.position, _groundRadius, _whatIsGround);
					if (!leftGrounded)
					{
						_rangedEnemySpeed *= -1;
						//Flip();
					}
					if (!rightGrounded)
					{
						_rangedEnemySpeed *= -1;
						//Flip();
					}
					transform.Translate(new Vector3(_rangedEnemySpeed, 0f, 0f) * Time.deltaTime);
				}


				if (_smashed && _rb.velocity.y < 0 && _grounded)
				{
					_smashed = false;
					gameObject.layer = 9;
				}
			}
			else
			{
				if (_rayResult.collider != null)
				{
					if (_rayResult.collider.gameObject.tag == "Player")
					{
						Vector2 moveResult = Vector2.MoveTowards(transform.position, _player.transform.position, _enemySpeed * Time.deltaTime);
						_rb.MovePosition(moveResult);
					}
				}

			}
			if (_health <= 0)
			{
				_shouldDestroy = true;
			}
			ValidateDestructionState();
			_timeSinceLastDamageOutput += Time.deltaTime;
		}
	}

	private void HandleMove(GameObject playerObj)
	{
			if (!_shouldNotMove && !_smashed)
			{
				if (playerObj.transform.position.x < transform.position.x && _rb.velocity.x > -_enemySpeed)
				{
					_rb.AddForce(Vector2.left * _enemySpeed);
				}
				else if (playerObj.transform.position.x > transform.position.x && _rb.velocity.x < _enemySpeed)
				{
					_rb.AddForce(Vector2.right * _enemySpeed);
				}
			}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Blade")
		{
			_isImpaled = true;
		}

	}
	void OnCollisionStay2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Player" && _timeSinceLastDamageOutput > 3f)
		{
			if (!_topdown)
			{
				coll.gameObject.GetComponent<PlatformControls>()._health -= 20;
			}
			else
			{
				coll.gameObject.GetComponent<TopDownControls>()._health -= 20;
			}
			_timeSinceLastDamageOutput = 0f;
		}
	}

	void ValidateDestructionState()
	{
		if (_shouldDestroy && _grounded && !_topdown)
		{
			Destroy(gameObject);
		}
		else if (_shouldDestroy && _topdown)
		{
			Destroy(gameObject);
		}
	}

    //void OnDrawGizmosSelected()
    //{
    //	Gizmos.color = Color.yellow;
    //	Gizmos.DrawSphere(_groundCheck.position, _groundRadius);
    //	Gizmos.DrawSphere(_leftGroundCheck.position, _groundRadius);
    //	Gizmos.DrawSphere(_rightGroundCheck.position, _groundRadius);
    //}
}