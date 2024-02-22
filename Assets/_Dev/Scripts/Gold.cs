using System;
using _Dev.Scripts.Events;
using _Dev.Scripts.Interface;
using DG.Tweening;
using UnityEngine;

namespace _Dev.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class Gold : MonoBehaviour,IInteractable
    {
        [SerializeField] private GameObject collectParticle;
        private Collider _collider;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            transform.DOMoveY(1, 1f).SetLoops(-1,LoopType.Yoyo);
            transform.DORotate(new Vector3(0, 45, 0), 0.1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }

        public void OnInteract()
        {
            _collider.enabled = false;
            EventBus<GoldCollectedEvent>.Emit(this,new GoldCollectedEvent(transform));
            collectParticle.SetActive(true);
            collectParticle.transform.SetParent(null);
            transform.DOKill();
            Destroy(gameObject);
        }
    }
}