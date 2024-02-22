using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.Scripts.Events;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public  class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    public CharacterController characterController;
    private float deltaTime;
    private bool _canMove = true;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    private void OnEnable()
    {
        EventBus<PlayerKilledEvent>.AddListener(OnPlayerKilledEvent);
    }

    private void OnDisable()
    {
        EventBus<PlayerKilledEvent>.RemoveListener(OnPlayerKilledEvent);
    }

    protected virtual void OnPlayerKilledEvent(object sender, PlayerKilledEvent e)
    {
        _canMove = false;
    }

    protected void Move()
    {
        if (_canMove)
        {
            characterController.Move(transform.forward * (moveSpeed * Time.deltaTime));
        }
    }

    protected void Rotate(Vector3 direction)
    {
        if (_canMove)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
        }
    }
}
