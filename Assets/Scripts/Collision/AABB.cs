using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    public struct AABB
    {
    public Vector2 center;
    public Vector2 size;
    public Vector2 extents { get { return size * .5f; } }

    public Vector2 min { get { return center - extents; } set { SetMinMax(value, max); } }
    public Vector2 max { get { return center + extents; } set { SetMinMax(min, value); } }

    public AABB(Vector2 center, Vector2 size)
    {
        this.center = center;
        this.size = size;
    }

    public bool Contains(AABB aabb)
    {
    return (min.x <= aabb.max.x && max.x >= aabb.min.x) &&
           (min.y <= aabb.max.y && max.y >= aabb.min.y);
    }

    public bool Contains(Vector2 point)
    {
    return (point.x >= min.x && point.x <= max.x) &&
           (point.y >= min.y && point.y <= max.y);
    }

    public void SetMinMax(Vector2 min, Vector2 max)
    {
        size = (max - min);
        center = min + extents;
    }

    public void Expand(Vector2 point)
    {
        SetMinMax(Vector2.Min(point, min), Vector2.Max(point, max));
    }

    public void Expand(AABB aabb)
    {
        SetMinMax(Vector2.Min(aabb.min, min), Vector2.Max(aabb.max, max));
    }

    public void Draw(Color color, float width = 0.05f)
    {
        Debug.DrawLine(new Vector2(min.x, min.y), new Vector2(max.x, min.y), color);
        Debug.DrawLine(new Vector2(min.x, max.y), new Vector2(max.x, max.y), color);
        Debug.DrawLine(new Vector2(min.x, min.y), new Vector2(min.x, max.y), color);
        Debug.DrawLine(new Vector2(max.x, min.y), new Vector2(max.x, max.y), color);
        /*  < use Debug.DrawLine to draw four lines of the AABB>*/
    }
}
