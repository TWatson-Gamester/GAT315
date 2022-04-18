using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision
{
    public static void CreateContacts(List<Body> bodies, out List<Contact> contacts)
    {
        contacts = new List<Contact>();
        for (int i = 0; i < bodies.Count; i++)
        {
            for (int j = i + 1; j < bodies.Count; j++)
            {
                Body bodyA = bodies[i];
                Body bodyB = bodies[j];
                if(bodyA.bodyType == Body.eBodyType.DYNAMIC || bodyB.bodyType == Body.eBodyType.DYNAMIC)
                {
                    if(TestOverlap(bodyA, bodyB))
                    {
                        contacts.Add(GenerateContact(bodyA, bodyB));
                    }
                }
            }
        }
    }

    public static Contact GenerateContact(Body bodyA, Body bodyB)
    {
        Contact contact = new Contact();

        contact.bodyA = bodyA;
        contact.bodyB = bodyB;

        Vector2 direction = bodyA.position - bodyB.position;
        float distance = direction.magnitude;
        float radius = ((CircleShape)bodyA.shape).radius + ((CircleShape)bodyB.shape).radius;
        contact.depth = radius - distance;

        contact.normal = direction.normalized;

        Vector2 position = bodyB.position + (((CircleShape)bodyB.shape).radius * contact.normal);
        Debug.DrawRay(position, contact.normal);

        return contact;
    }

    public static void SeparateContacts(List<Contact> contacts)
    {
        foreach (var contact in contacts)
        {
            float totalInverseMass = contact.bodyA.inverseMass + contact.bodyB.inverseMass;

            Vector2 seperation = contact.normal * (contact.depth / totalInverseMass);
            contact.bodyA.position += seperation * contact.bodyA.inverseMass;
            contact.bodyB.position -= seperation * contact.bodyB.inverseMass;
        }
    }

    public static bool TestOverlap(Body bodyA, Body bodyB)
    {
        return Circle.Intersects(new Circle(bodyA), new Circle(bodyB));
    }
}
