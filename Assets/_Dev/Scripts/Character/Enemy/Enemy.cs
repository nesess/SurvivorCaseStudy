using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.Scripts.Events;
using _Dev.Scripts.Interface;
using DG.Tweening;
using UnityEngine;
using Lean.Pool;

namespace _Dev.Scripts
{
    public class Enemy : Movement,IPoolable,IHittable, IInteractable
    {
        [Header("Stats")]
        [SerializeField] private float enemyAttackRange;
        [SerializeField] private float damage;
        [SerializeField] private float despawnRange = 100f;
        [Header("References")]
        [SerializeField] private EnemyProjectile projectilePrefab;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform characterHand;
        [SerializeField] private ParticleSystem deathParticle;
        [SerializeField] private Collider triggerCollider;
        
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Die = Animator.StringToHash("Die");
        
        private Transform _playerTransform;
        private Transform _myTransform;
        private bool _isDead = false;

        public bool IsAttacking
        {
            get => _isAttacking;
            set
            {
                _isAttacking = value;
                animator.SetBool(Attack,_isAttacking);
            }
        }
        private bool _isAttacking;
        private static readonly int Dance = Animator.StringToHash("Dance");

        private void Start()
        {
            _playerTransform = Player.Instance.transform;
            _myTransform = transform;
        }

        private IEnumerator MoveTowardsPlayerState()
        {
            Vector3 direction;
            float directionMagnitude;
            do
            {
                yield return null;
                direction = _playerTransform.position - _myTransform.position;
                directionMagnitude = direction.magnitude;

                if (directionMagnitude > despawnRange)
                {
                    LeanPool.Despawn(gameObject);
                    break;
                }
                
                Rotate(direction.normalized);
                Move();
                
            } while (directionMagnitude > enemyAttackRange);
            
            AttackToPlayer();
        }

        private IEnumerator AttackPlayerState()
        {
            while (IsAttacking)
            {
                Rotate((_playerTransform.position-_myTransform.position).normalized);
                yield return null;
            }
            
        }

        private void AttackToPlayer()
        {
            IsAttacking = true;
            StartCoroutine(AttackPlayerState());
        }

        public void ThrowProjectile()
        {
            var projectile = LeanPool.Spawn(projectilePrefab.gameObject,characterHand.position,Quaternion.identity).GetComponent<Projectile>();
            projectile.Damage = damage;
            var projectileTransform = projectile.transform;
            //projectileTransform.SetParent(transform);
            projectileTransform.LookAt(_playerTransform);
            var eulerAngles = projectileTransform.eulerAngles;
            eulerAngles.x = 0;
            projectileTransform.eulerAngles = eulerAngles;
        }

        public void OnAttackAnimCompleted()
        {
            if (Vector3.Distance(_myTransform.position, _playerTransform.position) > enemyAttackRange)
            {
                IsAttacking = false;
                StartCoroutine(MoveTowardsPlayerState());
            }
        }

        public void OnSpawn()
        {
            animator.Rebind();
            animator.Update(0f);
            IsAttacking = false;
            _isDead = false;
            characterController.enabled = true;
            triggerCollider.enabled = true;
            StartCoroutine(MoveTowardsPlayerState());
        }

        public void OnDespawn()
        {
            
        }

        public void OnDeathAnimCompleted()
        {
            EventBus<SkeletonKilledEvent>.Emit(this,new SkeletonKilledEvent());
            LeanPool.Spawn(deathParticle.gameObject, transform.position + Vector3.up*2f,Quaternion.identity);
            LeanPool.Despawn(gameObject);
        }
        
        public void OnHit(float damage)
        {
            StopAllCoroutines();
            transform.DOPunchScale(Vector3.one*0.4f, 0.4f);
            characterController.enabled = false;
            animator.SetTrigger(Die);
            _isDead = true;
            triggerCollider.enabled = false;
        }

        public void OnInteract()
        {
            if(_isDead) return;
            EventBus<PlayerTouchedSkeletonEvent>.Emit(this,new PlayerTouchedSkeletonEvent());
        }

        protected override void OnPlayerKilledEvent(object sender, PlayerKilledEvent e)
        {
            base.OnPlayerKilledEvent(sender, e);
            StopAllCoroutines();
            animator.SetTrigger(Dance);
        }
    }
}