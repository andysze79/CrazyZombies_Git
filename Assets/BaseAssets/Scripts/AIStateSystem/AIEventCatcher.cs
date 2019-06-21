using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace BaseAssets.AI
{
    public class AIEventCatcher : MonoBehaviour
    {
        public List<EventCall> onDamageEvent = new List<EventCall>();

        public void AnimationEvent(AnimationEvent _animationEvent)
        {
            foreach (EventCall action in onDamageEvent)
            {
                if (action.callName == _animationEvent.stringParameter)
                {
                    action.eventCall.Invoke(gameObject);
                }
            }
        }
    }

    [System.Serializable]
    public class EventCall
    {
        public string callName;
        [System.Serializable]
        public class OnDamageEvent : UnityEvent<GameObject> { }
        public OnDamageEvent eventCall = new OnDamageEvent();
    }
}