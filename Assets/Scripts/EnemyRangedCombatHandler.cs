using UnityEngine;
using System.Collections;

public class EnemyRangedCombatHandler : MonoBehaviour {
	private EnemyBehaviour _behaviour;
    private BossBehaviour _bossBehaviour;
	public GameObject _bulletPrefab;
	private float _cooldown;
	private float _cooldownComparator;
    private GameObject _player;
	private AudioSource _gunAudio;

	// Use this for initialization
	void Start () {
        if (!transform.parent.name.Contains("Boss"))
        {
            _behaviour = GetComponentInParent<EnemyBehaviour>();
			if (_behaviour._topdown)
			{
				_cooldownComparator = 1f;
			}
			else
			{
				_cooldownComparator = 4f;
			}
			_gunAudio = GetComponent<AudioSource>();
		}
        else
        {
            _bossBehaviour = GetComponentInParent<BossBehaviour>();
            _player = GameObject.Find("Pleiades");
			if (_bossBehaviour._topdown)
			{
				_cooldownComparator = 1f;
			}
			else
			{
				_cooldownComparator = 4f;
			}
		}

	}

	// Update is called once per frame
	void Update ()
	{
        if (!transform.parent.name.Contains("Boss"))
        {
            if (_behaviour._rayResult.collider != null)
            {
                if (_behaviour._rayResult.collider.gameObject.tag == "Player") //separate and repeated condition so it's not auto disabled by the cooldown
                {
                    _behaviour._shouldNotMove = true;
                }
                else
                {
                    _behaviour._shouldNotMove = false;
                }
                if (_behaviour._rayResult.collider.gameObject.tag == "Player" && _cooldown >= _cooldownComparator)
                {

                    Vector3 playerLoc = _behaviour._rayResult.collider.gameObject.transform.position;
                    Vector3 diff = playerLoc - transform.position;
                    diff.Normalize();
                    float angle = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) - 90;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
					if (transform.parent.GetComponent<SpriteRenderer>().isVisible)
					{
						_gunAudio.Play();
					}

                    Instantiate(_bulletPrefab, transform.position, transform.rotation);
                    _cooldown = 0f;
                }
            }
        }
        else if (_bossBehaviour._triggerObject == null && _cooldown >= 2f)
        {

            Vector3 playerLoc = _player.transform.position;
            Vector3 diff = playerLoc - transform.position;
            diff.Normalize();
            float angle = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(_bulletPrefab, transform.position, transform.rotation);
            _cooldown = 0f;


        }
		_cooldown += Time.deltaTime;
	}
}
