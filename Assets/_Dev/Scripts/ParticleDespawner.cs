using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class ParticleDespawner : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        LeanPool.Despawn(gameObject);
    }
}
