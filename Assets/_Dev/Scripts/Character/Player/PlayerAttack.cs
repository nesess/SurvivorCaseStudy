using System;
using _Dev.Scripts.Events;
using UnityEngine;
using UnityEngine.UI;
using Lean.Pool;

namespace _Dev.Scripts
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private float attackRange;
        [SerializeField] private float attackInterval;
        [SerializeField] private PlayerBoomerangProjectile boomerangProjectilePrefab;
        
        [Header("References")]
        [SerializeField] private Image playerRangeImage;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Transform spawnPos;

        private float _lastAttackTime;
        private RaycastHit _hitInfo;
        private Transform _myTransform;
        private bool _canAttack = true;
        private void Awake()
        {
            _myTransform = transform;
        }

        private void OnEnable()
        {
            EventBus<PlayerKilledEvent>.AddListener(OnPlayerKilledEvent);
        }

        private void OnDisable()
        {
            EventBus<PlayerKilledEvent>.RemoveListener(OnPlayerKilledEvent);
        }

        private void OnPlayerKilledEvent(object sender, PlayerKilledEvent e)
        {
            _canAttack = false;
        }

        private void OnValidate()
        {
            playerRangeImage.transform.localScale = Vector3.one * attackRange;
        }

        private void Update()
        {
            if (_canAttack && _lastAttackTime + attackInterval < Time.time)
            {
                if(Physics.SphereCast(_myTransform.position - Vector3.up*attackRange,attackRange,Vector3.up,out _hitInfo,attackRange,enemyLayer))
                {
                    _lastAttackTime = Time.time;
                    var projectile = LeanPool.Spawn(boomerangProjectilePrefab.gameObject,spawnPos.position,Quaternion.identity).GetComponent<PlayerBoomerangProjectile>();
                    projectile.target = _hitInfo.transform;
                }
            }
        }
    }
}