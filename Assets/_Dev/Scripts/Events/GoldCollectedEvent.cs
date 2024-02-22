using UnityEngine;

namespace _Dev.Scripts.Events
{
    public readonly struct GoldCollectedEvent
    {
        public readonly Transform goldTransform;

        public GoldCollectedEvent(Transform goldTransform)
        {
            this.goldTransform = goldTransform;
        }
    }
}