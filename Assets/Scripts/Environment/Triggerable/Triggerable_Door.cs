using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable_Door : ATriggerable
{
	[SerializeField]
	private Transform _doorObject;

	private Vector3 _closedPos = default;

	[SerializeField]
	private Vector3 _offset = default;

	[SerializeField, Range(0.1f, 15f)]
	private float _moveDuration = 1f;

	[SerializeField, Range(0.1f, 60.0f)]
	private float _stayOpenTime = 1;


	private float _current = 0f;

	private bool _isMoving;
	private bool _up = true;

	private void Start()
	{
		if (!_doorObject)
		{
			enabled = false;
			return;
		}
		_closedPos = _doorObject.transform.position;
	}

	void Update()
	{
		if (!_isMoving)
			return;

		Vector3 offset = _doorObject.transform.rotation * this._offset;

		Vector3 startPosition = _closedPos;
		Vector3 endPosition = _closedPos + offset;

		if (_up)
		{
			_current += Time.smoothDeltaTime;
		}
        else
        {
			_current -= Time.smoothDeltaTime;
        }

		_doorObject.transform.position = Vector3.Slerp(startPosition, endPosition, _current / _moveDuration);

		if (_current >= _moveDuration)
		{
			_isMoving = false;
			_current = _moveDuration;
		}
		else if(_current <= 0)
        {
			_isMoving = false;
			_current = 0.0f;
        }
	}

    public override void GotTriggered(TriggerPlate by)
    {
		//Debug.Log ("trigger");
		_up = true;
		//if (_current > 0.0f)
		//{
		//	_current = 1 - _current;
		//}
		_isMoving = true;
    }

    public override void GotUnTriggered(TriggerPlate by)
    {
		//Debug.Log("Untrigger");
		_up = false;
		//_isOpen = !_isOpen;

		//if (_current > 0.0f)
		//{
		//	_current = 1 - _current;
		//}

		if (Mathf.Approximately(_current, _moveDuration))
		{
			StartCoroutine(CloseAfterTime(_stayOpenTime));
		}
		else
		{
			_isMoving = true;
		}
	}

	private IEnumerator CloseAfterTime(float time)
    {
		yield return new WaitForSeconds(time);
		_isMoving = true;
    }


}
