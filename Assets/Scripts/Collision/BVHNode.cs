using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
    AABB aabb;
    List<Body> bodies = new List<Body>();

    BVHNode right;
    BVHNode left;

    public BVHNode(List<Body> bodies)
    {
        this.bodies = bodies;
        ComputeBoundry();
        Split();
    }

    public void ComputeBoundry()
    {
        if (bodies.Count == 0) return;

        aabb.center = bodies[0].position;
        aabb.size = Vector3.zero;

        bodies.ForEach(body => this.aabb.Expand(body.shape.GetAABB(body.position)));
    }

    public void Split()
    {
        int length = bodies.Count;
        int half = length / 2;
        if (half >= 1)
        {
            left = new BVHNode(bodies.GetRange(0, half));
            right = new BVHNode(bodies.GetRange(half, half));
            //< clear bodies, bodies are now in left / right children >
            bodies.Clear();
        }
    }

    public void Query(AABB aabb, List<Body> results)
    {
        if (!aabb.Contains(aabb)) return;

        results.AddRange(bodies);

        left?.Query(aabb, results);
        right?.Query(aabb, results);
    }

    public void Draw()
    {
        aabb.Draw(Color.white);

        left?.Draw();
        right?.Draw();
    }
}
