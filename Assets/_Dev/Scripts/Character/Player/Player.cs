using System;
using _Dev.Scripts.Events;
using _Dev.Scripts.Interface;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _Dev.Scripts
{
   
    public class Player : MonoBehaviour, IHittable
    { 
        public static Player Instance { get; private set; }

        private bool _isDead;
        
        public float Health
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                healthSlider.DOKill(true);
                healthSlider.DOValue(currentHealth / startingHealth, 1f).SetSpeedBased();
                if (currentHealth <= 0)
                {
                    _isDead = true;
                    EventBus<PlayerKilledEvent>.Emit(this,new PlayerKilledEvent());
                    GetComponentInChildren<Animator>().SetTrigger(Die);
                }
            }
        }
        
        [Header("Stats")]
        [SerializeField] private float startingHealth;
        [ReadOnly] [SerializeField] private float currentHealth;
        
        [Header("References")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Transform visualTransform;
        private static readonly int Die = Animator.StringToHash("Die");

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }

        private void OnEnable()
        {
            EventBus<PlayerTouchedSkeletonEvent>.AddListener(OnPlayerTouchedSkeletonEvent);
        }

        private void OnDisable()
        {
            EventBus<PlayerTouchedSkeletonEvent>.RemoveListener(OnPlayerTouchedSkeletonEvent);
        }

        private void OnPlayerTouchedSkeletonEvent(object sender, PlayerTouchedSkeletonEvent e)
        {
            Health = 0;
        }

        private void Start()
        {
            currentHealth = startingHealth;
        }
        
        public void OnHit(float damage)
        {
            if (_isDead) return;
            Health -= damage;
            visualTransform.DOPunchScale(Vector3.one*0.4f, 0.3f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_isDead) return;
            
            if (other.TryGetComponent(out IInteractable iInteractable))
            {
                Debug.Log("other name: "+other.gameObject);
                iInteractable.OnInteract();
            }
        }
    }
}