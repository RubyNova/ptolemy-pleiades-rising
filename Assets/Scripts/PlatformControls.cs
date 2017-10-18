using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class PlatformControls : MonoBehaviour 
{
	public GameObject _lastPlatform;
    public float _clampRange;
    public float maxSpeed = 10f;
    public float _jumpHeight = 10f;
    bool jumping = false;
    bool grounded = false;
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask whatIsGround;
    bool _facingLeft = true;
    float move;
    float fPlayerVelocity;
    //public Text uidebugtext;
    GameObject _combatHandler;
	HandleDamage _handlerControl;
    Rigidbody2D _rb;
	Animator _anim;
    BoxCollider2D _combatColl;
    public int _numberOfPresses;
	public int _health;
	public int _damageToPass;
    float _timeSinceLastPress;
	GameObject _rangedCombatHandler;
	HandleRangedDamage _rangedHandlerControl;
	int _numberOfMovePresses = 0;
	float _timeSinceLastMovePress = 0f;
	float _cooldown;
	public bool _teleportEnabled = false;
	public bool _bladesEnabled = false;
	private AudioSource _combatAudio;
	public AudioClip _punchAndKickClip; //done
	public AudioClip _jumpClip; //done
	public AudioClip _runClip; //done
	public AudioClip _bonusUpperCutClip;
	private AudioSource _mainAudio;
	public AudioClip _smashClip;
	public AudioClip _gameOverClip;
	public AudioClip _teleportSound;

	void Start () 
    {
		//if (Debug.isDebugBuild)
		//{
		//	Profiler.maxNumberOfSamplesPerFrame = 8
		//}
		_mainAudio = GetComponent<AudioSource>();
		_rb = GetComponent<Rigidbody2D>();
		_anim = GetComponent<Animator>();
        _combatHandler = transform.Find("CombatHandler").gameObject;
		_handlerControl = _combatHandler.GetComponent<HandleDamage>();
        _combatColl = _combatHandler.GetComponent<BoxCollider2D>();
		_combatAudio = _combatHandler.GetComponent<AudioSource>();
		_rangedCombatHandler = transform.Find("RangedCombatHandler").gameObject;
		_rangedHandlerControl = _rangedCombatHandler.GetComponent<HandleRangedDamage>();
        Flip();
		_facingLeft = false;
	}
	

    void FixedUpdate () 
    {
	    grounded = Physics2D.OverlapCircle(groundCheck.position,groundRadius,whatIsGround);
	    if(jumping == true && grounded == true)
        {
            _rb.AddForce(Vector2.up * _jumpHeight);
            jumping = false;
	    }
        fPlayerVelocity = _rb.velocity.x;
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if ((fPlayerVelocity < maxSpeed && move > 0) || (fPlayerVelocity > -maxSpeed && move < 0))
        {
            _rb.AddForce(Vector2.right * move * maxSpeed * 5);
			if (!_mainAudio.isPlaying && grounded)
			{
				_mainAudio.clip = _runClip;
				_mainAudio.loop = true;
				_mainAudio.Play();
			}
        }
        else if (move == 0 && grounded)
        {
            _rb.velocity = new Vector2(0f, 0f);
        }
		if ((move == 0 && grounded && !Input.GetKey(KeyCode.W) && _mainAudio.clip != _bonusUpperCutClip) || (_mainAudio.clip == _runClip && !grounded))
		{
			_mainAudio.Stop();
		}
		AnimatorStateInfo info = _anim.GetCurrentAnimatorStateInfo(0);
		if (move>0 && _facingLeft)
        {
			Flip();

        }
       else if(move<0 && !_facingLeft)
        {
			Flip();
        }
        _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_clampRange, _clampRange));

    }

    void Update()
    {
		if (_combatColl.enabled)
		{
			List<KeyValuePair<GameObject, float>> result = _handlerControl._enemies.Where(x => x.Key != null).ToList();
			if (result.Any())
			{
				if (result.Where(x => _combatColl.IsTouching(x.Key.GetComponent<Collider2D>())).Any())
				{
					if (!_combatAudio.isPlaying)
					{
						_combatAudio.clip = _punchAndKickClip; //plays clip here and not on damage frame event because it is a single thread, as a result this will never play if run from the damage frame event
						_combatAudio.loop = false;
						_combatAudio.Play();
					}
				}
			}
		}
        move = Input.GetAxisRaw("Horizontal");
		_anim.SetFloat("Speed", move);
		Vector3 mouse = Input.mousePosition;
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
		Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
		float angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg) - 90;
		_rangedCombatHandler.transform.rotation = Quaternion.Euler(0, 0, angle);
		Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(mouse);
		if ((mouseInWorld.x < transform.position.x && !_facingLeft) && move == 0)
		{
			Flip();
		}
		else if ((mouseInWorld.x > transform.position.x && _facingLeft) && move == 0)
		{
			Flip();
		}
		if (Input.GetKeyDown(KeyCode.W) && jumping==false && grounded == true)  
        {
			_mainAudio.loop = false;
			_mainAudio.clip = _jumpClip;
			_mainAudio.Play();
	 	    jumping = true;
        }
		else if (Input.GetKey(KeyCode.S))
		{
			GetComponent<PolygonCollider2D>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = true;
			_rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		else if (GetComponent<BoxCollider2D>().enabled && !_handlerControl._isSmackingDown)
		{
			GetComponent<PolygonCollider2D>().enabled = true;
			GetComponent<BoxCollider2D>().enabled = false;
			_rb.constraints = RigidbodyConstraints2D.None;
			_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		if (_numberOfPresses >= 6 || _timeSinceLastPress > 1f)
		{	
			_numberOfPresses = 0;
		}
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        if (Input.GetButtonDown("Fire1") && !stateInfo.IsName("TestPunch") && !stateInfo.IsName("TestKick"))
        {
			if (!grounded && _numberOfPresses == 0)
			{
				_numberOfPresses = 2;
			}
			if (_numberOfPresses < 6)
			{
                _timeSinceLastPress = 0f;
                switch (_numberOfPresses)
                {
                    case 0:
                    case 1:
                        _anim.SetBool("Punch", true);
                        break;
                    case 2:
					case 3:
						if (Input.GetKey(KeyCode.LeftShift))
						{
							_handlerControl._uppercut = true;
							_anim.SetBool("Kick", true);
						}
						else if (grounded)
						{
							_anim.SetBool("Kick", true);
						}
						else
						{
							_numberOfPresses = 4;
						}
                        break;
					case 4:
						if (!grounded)
						{
							_handlerControl._kickDown = true;
							_anim.SetBool("Kick", true);
						}
						else
						{
							_numberOfPresses = 0;
						}
						break;
					case 5:
						_rb.AddForce(Vector2.down * _jumpHeight);
						_handlerControl._isSmackingDown = true;
                        GetComponent<PolygonCollider2D>().enabled = false;
                        BoxCollider2D coll = GetComponent<BoxCollider2D>();
                        coll.enabled = true;
                        coll.bounds.Expand(new Vector2(5f, 0f));
                        _anim.SetBool("Kick", true);
						break;
				}

            }

		}
		else if (Input.GetButtonDown("Fire2") && _bladesEnabled)
		{
			_rangedHandlerControl.FireBlade();
		}
		else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && _cooldown >= 1 && _teleportEnabled)
		{
			_numberOfMovePresses += 1;
			if (_timeSinceLastMovePress < 1f && _numberOfMovePresses >= 2f)
			{
				Collider2D[] result = Physics2D.OverlapCircleAll(mouseInWorld, 3f);
				if (result.Where(x => x.gameObject.tag == "Blade").Any())
				{
					Transform closestBlade = result.Where(x =>x.gameObject.tag == "Blade").OrderBy(x => Vector2.Distance(mouseInWorld, x.transform.position)).First().transform;
					transform.position = closestBlade.position;
					_numberOfMovePresses = 0;
					_timeSinceLastMovePress = 0f;
					_cooldown = 0f;
					_mainAudio.Stop();
					_mainAudio.loop = false;
					_mainAudio.clip = _teleportSound;
					_mainAudio.Play();
				}

			}
			else if (_timeSinceLastMovePress > 1f)
			{
				_timeSinceLastMovePress = 0f;
				_numberOfMovePresses = 0;
			}
			else
			{
				_numberOfMovePresses += 1;
			}
		}
		_health = (int)Mathf.Clamp(_health, 0f, 1000f);
		if (_health == 0)
		{
			//uidebugtext.text = "Game Over";
			AudioSource cameraSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
			cameraSource.Stop();
			cameraSource.clip = _gameOverClip;
			cameraSource.Play();
			Destroy(gameObject);
		}
		else
		{
			//uidebugtext.text = "Health: " + _health;
		}

		_timeSinceLastMovePress += Time.deltaTime;

        _timeSinceLastPress += Time.deltaTime;
		_cooldown += Time.deltaTime;
    }
 
    void  Flip()
    {
 	    _facingLeft = !_facingLeft;
 	    transform.localScale = new Vector3(transform.localScale.x*-1f,transform.localScale.y,transform.localScale.z);
    }

    void OnDrawGizmosSelected()
    {
		//Gizmos.color = Color.yellow;
		//Gizmos.DrawSphere(groundCheck.position, groundRadius);
	}

    void OnDamageFrame()
    {

        _combatColl.enabled = true;
		_numberOfPresses += 1;
        _anim.SetBool("Kick", false);
		_anim.SetBool("Punch", false);
	}

    void OnPostDamageFrame() 
    {
        _combatColl.enabled = false;
    }

	void OnKickFinish()
	{
		if (_rb.isKinematic)
		{
			_rb.isKinematic = false;
		}

	}

	void OnCollisionEnter2D(Collision2D coll)
	{
        if (coll.gameObject.tag == "Enemy" && _handlerControl._isSmackingDown)
        {
            Collider2D enemyColl = coll.gameObject.GetComponent<Collider2D>();
            enemyColl.gameObject.layer = 10;
            Rigidbody2D enemyRb = coll.gameObject.GetComponent<Rigidbody2D>();
            enemyRb.constraints = RigidbodyConstraints2D.None;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (gameObject.transform.position.x <= enemyColl.gameObject.transform.position.x) //TODO: FIX THIS???
            {
                enemyRb.AddForce(new Vector2(Vector2.right.x * (_jumpHeight / 4), Vector2.up.y * (_rb.velocity.y + 200f)));
            }
            else
            {
                enemyRb.AddForce(new Vector2(Vector2.left.x * (_jumpHeight / 4), Vector2.up.y * (_rb.velocity.y + 200f)));
            }
            coll.gameObject.GetComponent<EnemyBehaviour>()._smashed = true;
        }
        else if (coll.gameObject.tag == "Ground")
        {
			if (_handlerControl._isSmackingDown)
			{
                GetComponent<PolygonCollider2D>().enabled = true;
                BoxCollider2D boxColl = GetComponent<BoxCollider2D>();
                boxColl.bounds.Expand(new Vector2(-5f, 0f));
                boxColl.enabled = false;
                _handlerControl._isSmackingDown = false;
				_combatAudio.clip = _smashClip;
				_combatAudio.loop = false;
				_combatAudio.Play();
			}

			if (coll.gameObject.GetComponent<PlatformDrop>() == null) 
			{
				_lastPlatform = coll.gameObject;
			}


        }
    }

}
