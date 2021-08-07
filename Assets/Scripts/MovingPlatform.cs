using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;
    [SerializeField] private float _speed = 1.0f;

    private bool _moveForwards = true;

    // Update is called once per frame
    void FixedUpdate() // physics update. Consistent. Useful for things require physics movement.
    {
        MovementBehaviour();
    }

    #region Movement

    private void MovementBehaviour()
    {
        if (_moveForwards)
        {
            MoveToPosition(targetB.position);

            if (CheckPositionReached(targetB.position))
            {
                _moveForwards = !_moveForwards;
            }
        }
        else
        {
            MoveToPosition(targetA.position);

            if (CheckPositionReached(targetA.position))
            {
                _moveForwards = !_moveForwards;
            }
        }
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _speed);
    }

    private bool CheckPositionReached(Vector3 targetPosition)
    {
        if (transform.position != targetPosition)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    #region Collision_Detection

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.parent = this.transform;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.parent = null;

        }
    }

    #endregion
}