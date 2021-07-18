using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animatior;
    [SerializeField] private GameObject _model;

    [SerializeField] private AudioSource _footstepsAudioSource;

    private bool _rechargeToggle = false;
    public bool _canMove = true;
    public bool ledgeGrabbing = false;

    [SerializeField] private float _speed = 5f;

    [SerializeField] private float _jumpHeight = 25;
    [SerializeField] private float _gravity = 1f;

    private float _yVelocity;

    private Vector3 _direction;
    private Vector3 _velocity;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animatior = GetComponentInChildren<Animator>();
    }

    void Update()
    {        
        CalculateMovement();

    }


    private void CalculateMovement()
    {
        float _horizontal = Input.GetAxisRaw("Horizontal");
        RunningSound(_horizontal);

        if (_controller.isGrounded)
        {
            if (_canMove)
            {
                _direction = new Vector3(0, 0, _horizontal);
                _velocity = _direction * _speed;
                SetModelDirection(_horizontal);

            }
            //else
            //{
            //    if (!_rechargeToggle)
            //    {
            //        _rechargeToggle = true;
            //        StartCoroutine(LandingRecharge(.5f));
            //    }
            //}


            _animatior.SetFloat("Speed", Mathf.Abs(_horizontal));
            _animatior.SetBool("Grounded", true);

            if (Input.GetButtonDown("Jump"))
            {
                _yVelocity = _jumpHeight; // NOT += 
                _animatior.SetBool("Jumping", true);
                //_animatior.SetTrigger("Jump");
                //  _rechargeToggle = false;
                // _canMove = false;
            }

        }
        else
        {
            _animatior.SetBool("Jumping", false);
            _animatior.SetBool("Grounded", false);
            _yVelocity -= _gravity;
        }

        _velocity.y = _yVelocity;
        _controller.Move(_velocity * Time.deltaTime);
    }


    private void SetModelDirection(float axisInput)
    {
        Vector3 facingRotation = _model.transform.localEulerAngles;

        if (axisInput > 0) // moving right.
        {
            facingRotation.y = 0;
            //_model.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (axisInput < 0) // moving left.
        {
            facingRotation.y = 180;
            //_model.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        _model.transform.localEulerAngles = facingRotation;
    }

    private void RunningSound(float axisInput)
    {
        if (Mathf.Abs(axisInput) > 0)
        {
            if (_controller.isGrounded)
            {
                _footstepsAudioSource.volume = 0.8f;
            }
            else
            {
                _footstepsAudioSource.volume = 0f;
            }
        }
        else
        {
            _footstepsAudioSource.volume = 0f;
        }
    }

    private IEnumerator LandingRecharge(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        _canMove = true;
    }


    public void LedgeGrab(Vector3 grabPos)
    {
        _controller.enabled = false;
        _animatior.SetBool("LedgeGrab", true);
        transform.position = grabPos;
    }
}
