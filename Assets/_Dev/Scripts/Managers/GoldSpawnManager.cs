using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Dev.Scripts.Events;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Dev.Scripts
{
    public class GoldSpawnManager : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private int goldSpawnCount;
        [SerializeField] private Vector2 minMaxDistance;
        [SerializeField] private float checkDistanceInterval;
        [SerializeField] private float maxDistanceForReposition;
        [SerializeField] private float goldMoveUITime;

        [Header("References")] 
        [SerializeField] private Gold goldPrefab;
        [SerializeField] private Transform goldContainer;
        [SerializeField] private RectTransform goldUI;

        private List<Transform> goldList = new();
        private Transform playerTransform;
        private Camera _mainCamera;

        private void OnEnable()
        {
            EventBus<GoldCollectedEvent>.AddListener(OnGoldCollectedEvent);
        }

        private void OnDisable()
        {
            EventBus<GoldCollectedEvent>.RemoveListener(OnGoldCollectedEvent);
        }

        private void OnGoldCollectedEvent(object sender, GoldCollectedEvent e)
        {
            goldList.Remove(e.goldTransform);
        }

        private void Start()
        {
            playerTransform = Player.Instance.transform;
            _mainCamera = Camera.main;
            
            for (int i = 0; i < goldSpawnCount; i++)
            {
                var rand = Random.insideUnitCircle;
                goldList.Add(Instantiate(goldPrefab, new Vector3(rand.x, 0, rand.y) * minMaxDistance.y,
                    quaternion.identity, goldContainer).transform);
            }

            StartCoroutine(CheckGoldDistances());
        }

        private IEnumerator CheckGoldDistances()
        {
            while (true)
            {
                yield return new WaitForSeconds(checkDistanceInterval);
                var playerPos = playerTransform.position;
                foreach (var gold in goldList)
                {
                    if (!(Vector3.Distance(gold.position, playerPos) > maxDistanceForReposition)) continue;
                    
                    var angle = Random.Range(0f, 2f * Mathf.PI);
                    var distance = Random.Range(minMaxDistance.x, minMaxDistance.y);
                    var offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * distance;
                    
                    gold.position = playerPos + offset;
                }
            }
        }
    }
}