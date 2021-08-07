using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Vector3 _positionOffset;
    private Vector3 _originalPos;
    private float _timePassed_position;

    [SerializeField] private float _yRotationAmount = 45;
    private Vector3 _furthermostClockwiseRotation;
    private Vector3 _furthermostAntiClockwiseRotation;
    private float _timePassed_rotation;

    private void Start()
    {
        SetupPosition();
        SetupRotation();       
    }

    private void Update()
    {
        UpdatePosition();
        UpdateRotation();
    }

    #region Position
    private void SetupPosition()
    {
        _originalPos = transform.position;
        _positionOffset += _originalPos;
    }

    private void UpdatePosition()
    {
        _timePassed_position += Time.deltaTime;
        transform.position = Vector3.Lerp(_originalPos, _positionOffset,
            Mathf.PingPong(_timePassed_position, 1));
    }
    #endregion


    private void SetupRotation()
    {
        _furthermostClockwiseRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + _yRotationAmount, transform.localEulerAngles.z);
        _furthermostAntiClockwiseRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - _yRotationAmount, transform.localEulerAngles.z);
        transform.localEulerAngles = _furthermostClockwiseRotation;
    }

    private void UpdateRotation()
    {
        _timePassed_rotation += (Time.deltaTime * 0.5f);
        transform.localEulerAngles = Vector3.Lerp(_furthermostClockwiseRotation, _furthermostAntiClockwiseRotation,
            Mathf.PingPong(_timePassed_rotation, 1));
    }

}
