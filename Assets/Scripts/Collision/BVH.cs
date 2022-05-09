using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH : BroadPhase
{
    BVHNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        // create quadtree root node
        rootNode = new BVHNode(bodies);
    }

    public override void Query(AABB aabb, List<Body> results)
    {
        //
    }

    public override void Query(Body body, List<Body> results)
    {
        //
    }

    public override void Draw()
    {
        rootNode?.Draw();
    }
}
