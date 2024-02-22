using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.Scripts;
using _Dev.Scripts.Events;
using UnityEngine;

public class PlayState : MonoBehaviour
{
    [SerializeField] private PlayMenu playMenu;
    [SerializeField] private EndMenu endMenuMenu;
     
    private const string goldSaveName = "totalGold";
    private int totalEarnedGold
    {
        get => PlayerPrefs.GetInt(goldSaveName, 0);
        set
        {
            PlayerPrefs.SetInt(goldSaveName,value);
            playMenu.SetGoldText(value);
        }
    }

    private int totalKilledSkeleton;

    private void Start()
    {
        playMenu.SetGoldText(totalEarnedGold);
        playMenu.SetKillCountText(totalKilledSkeleton);
    }

    private void OnEnable()
    {
        EventBus<GoldCollectedEvent>.AddListener(OnGoldCollectedEvent);
        EventBus<SkeletonKilledEvent>.AddListener(OnSkeletonKilledEvent);
        EventBus<PlayerKilledEvent>.AddListener(OnPlayerKilledEvent);
    }

    private void OnDisable()
    {
        EventBus<GoldCollectedEvent>.RemoveListener(OnGoldCollectedEvent);
        EventBus<SkeletonKilledEvent>.RemoveListener(OnSkeletonKilledEvent);
        EventBus<PlayerKilledEvent>.RemoveListener(OnPlayerKilledEvent);
    }

    private void OnPlayerKilledEvent(object sender, PlayerKilledEvent e)
    {
        playMenu.gameObject.SetActive(false);
        endMenuMenu.gameObject.SetActive(true);
        endMenuMenu.score = totalKilledSkeleton;
    }

    private void OnSkeletonKilledEvent(object sender, SkeletonKilledEvent e)
    {
        totalKilledSkeleton++;
        playMenu.SetKillCountText(totalKilledSkeleton);
    }

    private void OnGoldCollectedEvent(object sender, GoldCollectedEvent e)
    {
        totalEarnedGold++;
    }
}
