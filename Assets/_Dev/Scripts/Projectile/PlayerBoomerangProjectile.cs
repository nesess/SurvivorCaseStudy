using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Dev.Scripts
{
    public class PlayerBoomerangProjectile : Projectile
    {
        
        
        [ReadOnly] public Transform target;
        [SerializeField] private float moveSpeed;

        private Transform _myTransform;
        private float _rotationTime;

        private void Awake()
        {
            _myTransform = transform;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            StartCoroutine(MoveToTarget());
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            StopAllCoroutines();
        }
        

        private IEnumerator MoveToTarget()
        {
            yield return null;
            var differenceVector = target.position - _myTransform.position;
            _myTransform.right = differenceVector.normalized;
            _rotationTime = differenceVector.magnitude;
            float elapsedTime = 0;
            while (elapsedTime < _rotationTime)
            {
                _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, Quaternion.LookRotation((target.position - _myTransform.position).normalized), elapsedTime/_rotationTime);
                var rot = _myTransform.eulerAngles;
                rot.x = 0;
                rot.z = 0;
                _myTransform.eulerAngles = rot;
                _myTransform.Translate(Vector3.forward * (Time.deltaTime * moveSpeed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
    }
}