using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Camera))]
public class CamFollow : MonoBehaviour {

    public GameObject _target;
    public float _lerpFactor = 0.1f;
    public Vector3 _offset = new Vector3(0, 0.7f, 0);
    Vector3 _newPosition;
	
	void FixedUpdate() 
    {
		if (_target != null)
		{
			_newPosition = transform.position - _offset;
			_newPosition = new Vector3(Mathf.Lerp(_newPosition.x, _target.transform.position.x, _lerpFactor), Mathf.Lerp(_newPosition.y, _target.transform.position.y, _lerpFactor), transform.position.z);

			transform.position = _newPosition + _offset;
		}

    }
}
