using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : Shape
{
    public override float size { get => transform.localScale.x; set => transform.localScale = Vector2.one * value; }
    public override float area => radius * radius * Mathf.PI;

    public float radius => size * .5f;
}
