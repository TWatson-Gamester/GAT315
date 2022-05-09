using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeNode
{
    AABB nodeAABB;
    int nodeCapacity;
    int nodeLevel;
    List<Body> nodeBodies = new List<Body>();

    QuadtreeNode northeast;
    QuadtreeNode northwest;
    QuadtreeNode southeast;
    QuadtreeNode southwest;
    bool subdivided = false;

    public QuadtreeNode(AABB aabb, int capacity, int level)
    {
        nodeAABB = aabb;
        nodeCapacity = capacity;
        nodeLevel = level;
    }

    public void Insert(Body body)
    {
        //check if with node AABB
        if (!nodeAABB.Contains(body.position)) return;

        //check if whihin capacity
        if(nodeBodies.Count < nodeCapacity)
        {
            nodeBodies.Add(body);
        }
        else
        {
            //subdivide
            // exceeded capacity, subdivide node
            if (!subdivided) Subdivide();
            // insert body into the newly subdivided nodes
            northeast.Insert(body);
            // insert northwest...
            northwest.Insert(body);
            southeast.Insert(body);
            southwest.Insert(body);
        }
    }

    public void Draw()
    {
        Color color = BroadPhase.colors[nodeLevel % BroadPhase.colors.Length];

        nodeAABB.Draw(color);
        nodeBodies.ForEach(body => Debug.DrawLine(nodeAABB.center, body.position, color));

        // draw northeast node
        if (northeast != null) northeast.Draw();
        // draw northwest node
        if (northwest != null) northwest.Draw();
        // draw southeast node
        if (southeast != null) southeast.Draw();
        // draw southwest node
        if (southwest != null) southwest.Draw();
    }

    private void Subdivide()
    {
        float xo = nodeAABB.extents.x * 0.5f;
        float yo = nodeAABB.extents.y * 0.5f;

        northeast = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
        northwest = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x + xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
        southeast = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x + xo, nodeAABB.center.y - yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
        southwest = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y - yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);

        subdivided = true;
    }

    public void Query(AABB aabb, List<Body> results)
    {
        if (!nodeAABB.Contains(aabb)) return;

        results.AddRange(nodeBodies);

        if (subdivided)
        {
            northeast.Query(aabb, results);
            northwest.Query(aabb, results);
            southeast.Query(aabb, results);
            southwest.Query(aabb, results);
        }
    }
}
