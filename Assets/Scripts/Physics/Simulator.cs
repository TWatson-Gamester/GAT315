using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] FloatData fixedFPS;
	[SerializeField] StringData fps;
	
	public List<Force> forces;
	public List<Body> bodies { get; set; } = new List<Body>();
	Camera activeCamera;
	private float timeAccumulator = 0;
	float fixedDeltaTime { get => 1.0f / fixedFPS.value; }

	private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {
		timeAccumulator += Time.deltaTime;
		forces.ForEach(force => force.ApplyForce(bodies));
/*        foreach(var body in bodies)
        {
			body.Step(Time.deltaTime);
        }*/

		while (timeAccumulator > fixedDeltaTime)
		{
			bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime));

			timeAccumulator = timeAccumulator - fixedDeltaTime;
		}

		foreach (var body in bodies)
		{
			body.acceleration = Vector2.zero;
		}

		fps.value = (1.0f / Time.deltaTime).ToString("F2");
	}

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return new Vector3(world.x, world.y, 0);
	}
}
