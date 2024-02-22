using System;
using _Dev.Scripts.Events;
using UnityEngine;

namespace _Dev.Scripts
{
    public class PlayerMovementRotation : Movement
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private Animator animator;
        
        private Vector3 moveDirection = Vector3.zero;
        private static readonly int Run = Animator.StringToHash("Run");
        

        private void Update()
        {
            moveDirection.x = joystick.Horizontal;
            moveDirection.z = joystick.Vertical;
            if (moveDirection != Vector3.zero)
            {
                animator.SetBool(Run,true);
                Move();
                Rotate(moveDirection);
            }
            else
            {
                animator.SetBool(Run,false);
            }
        }
    }
}