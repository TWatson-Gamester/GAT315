using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
    public static void ExplicitEuler(Body body, float dt)
    {
        //acceleration = force / mass;
        body.position += body.velocity * dt;
        body.velocity += body.acceleration * dt;
    }

    public static void SemiImplicitEuler(Body body, float dt)
    {
        //acceleration = force / mass;
        body.velocity += body.acceleration * dt;
        body.position += body.velocity * dt;
    }
}
