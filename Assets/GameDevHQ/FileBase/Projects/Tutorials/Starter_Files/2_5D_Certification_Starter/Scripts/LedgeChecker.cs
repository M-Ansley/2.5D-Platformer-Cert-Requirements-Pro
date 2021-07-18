using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
    [SerializeField] private static Player _player;
    //[SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _ledgeGrabTransform;

    private void Start()
    {
        if (FindObjectOfType<Player>() != null)
        {
            _player = FindObjectOfType<Player>();
            //_playerAnimator = _player.gameObject.GetComponentInChildren<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ledge_Grab_Checker"))
        {
            Debug.Log("Ledge grab checker detected");
            if (_player != null)
            {
                _player.LedgeGrab(_ledgeGrabTransform.position);

            }
        }
    }
}
