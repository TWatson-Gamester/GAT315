using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
    AABB nodeAABB;
    List<Body> nodeBodies = new List<Body>();

    BVHNode right;
    BVHNode left;

    public BVHNode(List<Body> bodies)
    {
        nodeBodies = bodies;
        ComputeBoundry();
        Split();
    }

    public void ComputeBoundry()
    {

    }

    public void Split()
    {

    }

    public void Query(AABB aabb, List<Body> results)
    {
        if (!nodeAABB.Contains(aabb)) return;

        results.AddRange(nodeBodies);

        left?.Query(aabb, results);
        right?.Query(aabb, results);
    }

    public void Draw()
    {
        nodeAABB.Draw(Color.white);

        left?.Draw();
        right?.Draw();
    }
}
