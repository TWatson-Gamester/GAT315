using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static void ApplyImpules(List<Contact> contacts)
    {
        foreach (var contact in contacts)
        {
            Vector2 relativeVelocity = contact.bodyA.velocity - contact.bodyB.velocity;
            float normalVelocity = Vector2.Dot(relativeVelocity, contact.normal);

            if (normalVelocity > 0) continue;

            float totalInverseMass = contact.bodyA.inverseMass + contact.bodyB.inverseMass;
            float restitution = (contact.bodyA.restitution + contact.bodyB.restitution) * .5f;
            float impulseMagnitude = -((1 + restitution) * normalVelocity) / totalInverseMass;

            Vector2 impulse = contact.normal * impulseMagnitude;

            contact.bodyA.ApplyForce(contact.bodyA.velocity + (impulse * contact.bodyA.inverseMass), Body.eForceMode.FORCE);
            contact.bodyB.ApplyForce(contact.bodyB.velocity - (impulse * contact.bodyB.inverseMass), Body.eForceMode.FORCE);
        }
    }

    public static bool TestOverlap(Body bodyA, Body bodyB)
    {
        return Circle.Intersects(new Circle(bodyA), new Circle(bodyB));
    }

    public static void CreateBroadPhaseContacts(BroadPhase broadPhase, List<Body> bodies, List<Contact> contacts)
    {
        List<Body> results = new List<Body>();
        for (int i = 0; i < bodies.Count; i++)
        {
            results.Clear();
            // query the broad-phase for potential contacting bodies
            broadPhase.Query(bodies[i], results);

            // add broad-phase contacts 
            for (int j = 0; j < results.Count; j++)
            {
                // check if the result is self and that one of the bodies is a dynamic body
                if (results[j] != bodies[i] &&
                   (results[j].bodyType == Body.eBodyType.DYNAMIC || bodies[i].bodyType == Body.eBodyType.DYNAMIC))
                {
                    // check if contact already exists between these two bodies
                    //if (!Contact.ExistsIn(contacts, bodies[i], results[j]))
                    {
                        // create new contact and add to contacts
                        Contact contact = new Contact() { bodyA = bodies[i], bodyB = results[j] };
                        contacts.Add(contact);
                    }
                }
            }
        }

        contacts.Distinct(new Contact.ItemEqualityComparer());
    }

    public static void CreateNarrowPhaseContacts(List<Contact> contacts)
    {
        // remove contacts from narrow-phase test
        contacts.RemoveAll(contact => (TestOverlap(contact.bodyA, contact.bodyB) == false));
        // generate contact info from remaining contacts
        for (int i = 0; i < contacts.Count; i++)
        {
            GenerateContactInfo(contacts[i]);
        }
    }

    public static Contact GenerateContactInfo(Contact contact)
    {
        // compute depth
        Vector2 direction = contact.bodyA.position - contact.bodyB.position;
        float distance = direction.magnitude;
        float radius = ((CircleShape)contact.bodyA.shape).radius + ((CircleShape)contact.bodyB.shape).radius;
        contact.depth = radius - distance;

        // compute normal
        contact.normal = direction.normalized;

        return contact;
    }

}
