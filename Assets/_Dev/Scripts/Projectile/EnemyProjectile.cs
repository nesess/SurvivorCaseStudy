using System;
using Lean.Pool;
using UnityEngine;

namespace _Dev.Scripts
{
    public class EnemyProjectile : Projectile
    {
        [SerializeField] private float maxDistance;
        [SerializeField] private float moveSpeed;

        private Transform _myTransform;

        private void Awake()
        {
            _myTransform = transform;
        }
        
        public override void OnSpawn()
        {
            base.OnSpawn();
            LeanPool.Despawn(gameObject,maxDistance / moveSpeed);
        }
        
        private void Update()
        {
            _myTransform.Translate(Vector3.forward * (Time.deltaTime * moveSpeed));
        }
    }
}