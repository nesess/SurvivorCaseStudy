using System;
using UnityEngine;

namespace _Dev.Scripts
{
    public class EnemyAnimationEvent : MonoBehaviour
    {
        private Enemy _enemy;

        private void Awake()
        {
            _enemy = GetComponentInParent<Enemy>();
        }

        public void InvokeThrowProjectile()
        {
            _enemy.ThrowProjectile();
        }

        public void AttackAnimCompleted()
        {
            _enemy.OnAttackAnimCompleted();
        }

        public void DeathAnimCompleted()
        {
            _enemy.OnDeathAnimCompleted();
        }
    }
}