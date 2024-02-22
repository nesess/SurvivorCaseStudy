using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.Scripts;
using UnityEngine;

public class GroundMoveManager : MonoBehaviour
{
    [SerializeField] private Transform groundTransform;
    [SerializeField] private float checkInterval;
    [SerializeField] private float maxDistance;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = Player.Instance.transform;
        StartCoroutine(CheckDistanceMoveGroundCoroutine());
    }

    private IEnumerator CheckDistanceMoveGroundCoroutine()
    {
        var moveRight = Vector3.right * maxDistance;
        var moveForward = Vector3.forward * maxDistance;
        
        while (true)
        {
            var playerPos = playerTransform.position;
            var groundPos = groundTransform.position;

            if (Mathf.Abs(playerPos.x - groundPos.x) >= maxDistance)
            {
                groundTransform.position += (playerPos.x > groundPos.x) ? moveRight : -moveRight;
            }

            if (Mathf.Abs(playerPos.z - groundPos.z) >= maxDistance)
            {
                groundTransform.position += (playerPos.z > groundPos.z) ? moveForward : -moveForward;
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
    
}
