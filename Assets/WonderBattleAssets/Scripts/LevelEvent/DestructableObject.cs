using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{

    public enum State {
        Idle,
        Destroy,
        Sink
    }

    public State m_currentState = State.Idle;

    public State CurrentState { get; set; }
    public float ExplodeForce { get; set; }
    public float ExplodeForceRandomness { get; set; }
    public Vector3 ExplodeDirection { get; set; }
    public Rigidbody[] Rigidbodies { get; set; }
    public Collider[] Colliders { get; set; }

    public void Awake()
    {
        Rigidbodies = GetComponentsInChildren<Rigidbody>();
        Colliders = GetComponentsInChildren<Collider>();

        Initiate();
    }

    public void Initiate() {
        foreach (var obj in Rigidbodies)
        {
            obj.useGravity = false;
        }

        foreach (var obj in Colliders)
        {
            obj.enabled = false;
        }
    }

    public void Update()
    {
        if (CurrentState != m_currentState)
        {
            CurrentState = m_currentState;

            switch (CurrentState)
            {
                case State.Idle:
                    break;
                case State.Destroy:
                    foreach (var obj in Rigidbodies)
                    {
                        obj.useGravity = true;
                        obj.AddForce(ExplodeDirection * ExplodeForce * Random.Range(ExplodeForceRandomness,1f));
                    }
                    foreach (var obj in Colliders)
                    {
                        obj.enabled = true;
                    }
                    break;
                case State.Sink:
                    foreach (var obj in Colliders)
                    {
                        obj.enabled = false;
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
