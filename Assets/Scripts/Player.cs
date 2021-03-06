using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animatior;
    private LedgeChecker _currentLedge;
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

    private bool _rolling = false;
    private bool _canMoveVertically = true;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animatior = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _yVelocity = _jumpHeight; // NOT += 
                _animatior.SetBool("Jumping", true);
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!_rolling)
                {
                    StartCoroutine(BarrelRoll());
                }
            }

        }
        LedgeGrabbingBehaviour();
        if (_canMoveVertically)
        {
            CalculateGroundedMovement();
        }
    }

    void FixedUpdate()
    {
        CalculateUngroundedMovement();
    }


    /// <summary>
    /// Run in Update to let platforms control our position
    /// </summary>
    private void CalculateGroundedMovement()
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

            _animatior.SetFloat("Speed", Mathf.Abs(_horizontal));
            _animatior.SetBool("Grounded", true);

        }
        _velocity.y = _yVelocity;
        _controller.Move(_velocity * Time.deltaTime);
    }

    /// <summary>
    /// Run in FixedUpdate (for jump consistency on lower performance)
    /// </summary>
    private void CalculateUngroundedMovement()
    {
        if (!_controller.isGrounded)
        {
            if (!ledgeGrabbing)
            {
                _animatior.SetBool("Jumping", false);
                _animatior.SetBool("Grounded", false);
                _yVelocity -= _gravity;
            }
        }
    }

    /// <summary>
    /// Returns true if currently facing right; false if currently facing left
    /// </summary>
    private bool FacingRight()
    {
        if (_model.transform.localEulerAngles.y == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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


    private void LedgeGrabbingBehaviour()
    {
        if (ledgeGrabbing)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _animatior.SetTrigger("ClimbUp");
            }
        }
    }

    public void LedgeGrab(Vector3 grabPos, LedgeChecker ledge)
    {
        ledgeGrabbing = true;
        _currentLedge = ledge;
        _controller.enabled = false;
        _animatior.SetBool("LedgeGrab", true);
        _animatior.SetFloat("Speed", 0.0f);
        _animatior.SetBool("Jumping", false);
        _animatior.SetBool("Grounded", true);
        transform.position = grabPos;
    }

    public void ClimbUp()
    {
        if (FacingRight())
        {
            Vector3 increasePos = new Vector3(0, 7.24f, 1f);
            transform.position += increasePos;
        }
        else
        {
            Vector3 increasePos = new Vector3(0, 7.24f, -1f);
            transform.position += increasePos;
        }
        _controller.enabled = true;
        _animatior.SetBool("LedgeGrab", false);
        _currentLedge = null;
        ledgeGrabbing = false;
        //transform.position = new Vector3(0, 7.24, )
    }

    IEnumerator BarrelRoll()
    {
        _canMoveVertically = false;
        _rolling = true;
        _animatior.SetTrigger("Roll");
        yield return new WaitForSecondsRealtime(0.5f);
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos;
        
        if (FacingRight())
        {
            endPos += new Vector3(0, 0, 17f);
        }
        else
        {
            endPos -= new Vector3(0, 0, 17f);
        }

       
        float duration = .85f;
        float time = 0;
        while (transform.position != endPos && _controller.isGrounded)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (time / duration));
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        _canMoveVertically = true;
        _rolling = false;
    }



    #region Collectibles

    public void IncreaseSpeed(float increment)
    {
        _speed += increment;
    }

    #endregion
}
