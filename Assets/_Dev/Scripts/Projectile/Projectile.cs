using System;
using System.Collections.Generic;
using _Dev.Scripts.Interface;
using UnityEngine;
using Lean.Pool;
using UnityEngine.Serialization;

namespace _Dev.Scripts
{
    public abstract class Projectile : MonoBehaviour,IPoolable
    {
        [SerializeField] private ParticleSystem hitParticle;
        
        [HideInInspector] public float Damage;

        private List<TrailRenderer> _trailRenderers = new List<TrailRenderer>();

        private void Start()
        {
            foreach (var trailRenderer in GetComponentsInChildren<TrailRenderer>())
            {
                _trailRenderers.Add(trailRenderer);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHittable hittable))
            {
                hittable.OnHit(Damage);
                LeanPool.Despawn(gameObject);
            }
        }

        public virtual void OnSpawn()
        {
            foreach (var trailRenderer in _trailRenderers)
            {
                trailRenderer.Clear();
            }
        }

        public virtual void OnDespawn()
        {
            LeanPool.Spawn(hitParticle.gameObject, transform.position,Quaternion.identity);
        }
    }
}