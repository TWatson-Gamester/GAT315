using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public enum eForceMode
    {
        FORCE,
        VELOCITY,
        ACCELERATION
    }

    public Vector2 position { get => transform.position; set => transform.position = value; }
    public Vector2 velocity { get; set; } = Vector2.zero;
    public Vector2 acceleration { get; set; } = Vector2.zero;
    public Vector2 force{ get; set; } = Vector2.zero;
    public float mass => shape.mass;
    public float inverseMass { get => (mass == 0) ? 0 : 1 / mass; }
    public Shape shape;

    public void ApplyForce(Vector2 force, eForceMode forceMode)
    {
        switch (forceMode)
        {
            case eForceMode.FORCE:
                acceleration += force * inverseMass;
                break;
            case eForceMode.VELOCITY:
                velocity = force;
                break;
            case eForceMode.ACCELERATION:
                acceleration += force;
                break;
            default:
                break;
        }
    }

    public void Step(float dt)
    {
        //acceleration = Simulator.Instance.gravity + (force * inverseMass);
    }

}
