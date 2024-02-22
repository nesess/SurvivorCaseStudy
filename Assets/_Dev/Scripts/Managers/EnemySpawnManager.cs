using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.Scripts;
using _Dev.Scripts.Events;
using UnityEngine;
using Lean.Pool;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private Vector2 minMaxSpawnDistance;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnIntervalDecreaseMultiplier;
    [SerializeField] private float intervalDecreaseTime;

    [Header("References")]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform enemyParent;

    private Transform _playerTransform;
    private bool _canSpawn = true;

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
        StopAllCoroutines();
    }
    
    private void Start()
    {
        _playerTransform = Player.Instance.transform;
        StartCoroutine(SpawnEnemiesCoroutine());
        StartCoroutine(SpawnIntervalDecreaseCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (_canSpawn)
        {
            var spawnPoint = Random.insideUnitCircle.normalized *
                             Random.Range(minMaxSpawnDistance.x, minMaxSpawnDistance.y);
            LeanPool.Spawn(enemyPrefab, _playerTransform.position +new Vector3(spawnPoint.x,0,spawnPoint.y), quaternion.identity, enemyParent);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnIntervalDecreaseCoroutine()
    {
        while (_canSpawn)
        {
            spawnInterval *= spawnIntervalDecreaseMultiplier;
            yield return new WaitForSeconds(intervalDecreaseTime);
        }
    }

}
