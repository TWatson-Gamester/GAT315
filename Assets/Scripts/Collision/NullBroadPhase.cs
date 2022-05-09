using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullBroadPhase : BroadPhase
{
	public List<Body> bodies { get; set; } = new List<Body>();

	public override void Build(AABB aabb, List<Body> bodies)
	{
		bodies.Clear();
		bodies.AddRange(bodies);
	}

	public override void Query(AABB aabb, List<Body> results)
	{
		//
	}

	public override void Query(Body body, List<Body> results)
	{
		results.AddRange(bodies);
	}

	public override void Draw()
	{
		//
	}
}
