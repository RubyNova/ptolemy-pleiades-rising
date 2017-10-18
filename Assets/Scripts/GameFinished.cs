using UnityEngine;
using System.Collections;

public class GameFinished : MonoBehaviour {

	public Canvas _HUDCanvas;
	private PauseMenuController _pauseController;
	public AudioClip _victoryClip;

	// Use this for initialization
	void Start () {
		_pauseController = _HUDCanvas.GetComponent<PauseMenuController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			AudioSource cameraSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
			cameraSource.Stop();
			cameraSource.clip = _victoryClip;
			cameraSource.Play();
			_pauseController.GameOver();
		}
	}
}
