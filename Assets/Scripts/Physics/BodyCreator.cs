using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyCreator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] Body bodyPrefab;
    [SerializeField] FloatData speed;
    [SerializeField] FloatData size;

	bool action = false;
	bool pressed = false;

    private void Update()
    {
        if (action && (pressed || Input.GetKey(KeyCode.LeftControl)))
        {
            pressed = false;
            Vector3 position = Simulator.Instance.GetScreenToWorldPosition(Input.mousePosition);
            Body body = Instantiate(bodyPrefab, position, Quaternion.identity);
            body.shape.size = size.value;
            body.ApplyForce(Random.insideUnitCircle.normalized * speed.value, Body.eForceMode.VELOCITY);

            Simulator.Instance.bodies.Add(body);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        action = true;
        pressed = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        action = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        action = false;
    }
}
