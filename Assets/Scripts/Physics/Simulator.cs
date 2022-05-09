using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] BoolData simulate;
	[SerializeField] FloatData fixedFPS;
	[SerializeField] StringData fps;
	
	public List<Force> forces;
	public List<Body> bodies { get; set; } = new List<Body>();
	Camera activeCamera;
	private float timeAccumulator = 0;
	float fixedDeltaTime { get => 1.0f / fixedFPS.value; }
	//BroadPhase broadPhase = new Quadtree();
	BroadPhase broadPhase = new BVH();

	private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {

		fps.value = (1.0f / Time.deltaTime).ToString("F2");
		if (!simulate.value) return;
		timeAccumulator += Time.deltaTime;
		forces.ForEach(force => force.ApplyForce(bodies));
		Vector2 screenSize = GetScreenSize();
		while (timeAccumulator > fixedDeltaTime)
		{
			// construct broad-phase tree
			broadPhase.Build(new AABB(Vector2.zero, GetScreenSize()), bodies);
			var contacts = new List<Contact>();
			Collision.CreateBroadPhaseContacts(broadPhase, bodies, contacts);
			Collision.CreateNarrowPhaseContacts(contacts);
			Collision.SeparateContacts(contacts);
			Collision.ApplyImpules(contacts);

			bodies.ForEach(body =>
			{
				Integrator.SemiImplicitEuler(body, fixedDeltaTime);
				body.position = body.position.Wrap(-screenSize * .5f, screenSize * .5f);
				body.shape.GetAABB(body.position).Draw(Color.white);
			});

			timeAccumulator = timeAccumulator - fixedDeltaTime;
		}

		foreach (var body in bodies)
		{
			body.acceleration = Vector2.zero;
		}
		broadPhase.Draw();
	}

    public Body GetScreenToBody(Vector3 screen)
    {
		Body body = null;

		Ray ray = activeCamera.ScreenPointToRay(screen);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider)
        {
			hit.collider.gameObject.TryGetComponent<Body>(out body);
        }

		return body;
    }

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return new Vector3(world.x, world.y, 0);
	}

	public Vector2 GetScreenSize()
    {
		return activeCamera.ViewportToWorldPoint(Vector2.one) * 2;
    }
	public void ClearBodies()
    {
		foreach(Body body in bodies)
        {
			Destroy(body.gameObject);
        }
		bodies.Clear();
    }
}
