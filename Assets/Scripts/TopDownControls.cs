using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopDownControls : MonoBehaviour
{
	public float maxSpeed = 10f;
	public float rotSpeed = 5f;
	Rigidbody2D rb;
	float rot, move;
	public GameObject bullet, muzzle;
	public int _health;
	private DoorControl _doorInstance;
	public List<char> _numeralData;
	private PauseMenuController _pauseController;
	public Canvas _HUDCanvas;
	public AudioClip _gameOverClip;
	private AudioSource _audioController;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		_pauseController = _HUDCanvas.GetComponent<PauseMenuController>();
		_audioController = GetComponent<AudioSource>();
	}

	void Update()
	{
		_health = Mathf.Clamp(_health, 0, 1000);
		if (Input.GetButtonDown("Fire2"))
		{
			_audioController.Play();
			Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
		}
		if (_health <= 0)
		{
			AudioSource cameraSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
			cameraSource.Stop();
			cameraSource.clip = _gameOverClip;
			cameraSource.Play();

			Destroy(gameObject);
		}

		if (Input.GetKeyDown("e"))
		{
			if (_doorInstance != null)
			{
				if (_doorInstance._isSolved)
				{
					if (_doorInstance.IsOpen)
					{
						_doorInstance.Close();
					}
					else
					{
						_doorInstance.Open();
					}
				}
				else
				{
					_pauseController.ShowPasscodeScreen(_doorInstance);
				}
			}
		}
	}


	void FixedUpdate()
	{
		rot = Input.GetAxis("Horizontal");
		move = Input.GetAxis("Vertical");
		//anim.SetFloat("Speed",Mathf.Abs(move));
		transform.Rotate(Vector3.forward * -rot * rotSpeed);
		rb.AddRelativeForce(new Vector2(0, move * maxSpeed));

	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.name.Contains("Door"))
		{
			_doorInstance = coll.gameObject.GetComponent<DoorControl>();
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		_doorInstance = null;
	}
}
