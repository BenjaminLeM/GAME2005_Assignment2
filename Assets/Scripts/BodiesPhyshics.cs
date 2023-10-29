using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using System.IO;
using UnityEditor.Experimental.GraphView;

public class BodiesPhyshics : MonoBehaviour
{
    // Homework: Incorporate launch position, launch speed, and launch angle into our new custom-physics system!
    // Plot 3D projectile motion by specifying a pitch (launch-angle about X) and yaw (launch-angle about Y)
    public Vector3 launchPosition;
    public float dt;
    public Vector3 grav = new Vector3(0, -1.6f, 0);
    public List<Body> bodies = new List<Body>();
    void checkForNewBodies()
    {
        Body[] allObjects = FindObjectsOfType<Body>(false);
        foreach (Body BodyFound in allObjects)
        {
            if (!bodies.Contains(BodyFound))
            {
                bodies.Add(BodyFound);
            }
        }
    }
    bool checkSphereSphereCollision(Body bodyA, Body bodyB)
    {
        Vector3 displacement = bodyA.transform.position - bodyB.transform.position;
        float distance = displacement.magnitude;
        return distance < bodyA.radius + bodyB.radius;
    }
    bool checkSpherePlaneCollision(Body sphere, Body halfSpace)
    {
        //currently non functional
        //float distance = Mathf.Abs(bodyB.transform.position.x * bodyA.transform.position.x 
        //                            + bodyB.transform.position.y * bodyA.transform.position.y 
        //                            + bodyB.transform.position.z * bodyA.transform.position.z + 
        //                            (bodyB.transform.position.x + bodyB.transform.position.y + bodyB.transform.position.z)) /
        //                            Mathf.Sqrt(bodyB.transform.position.x * bodyB.transform.position.x
        //                            + bodyB.transform.position.y * bodyB.transform.position.y
        //                            + bodyB.transform.position.z * bodyB.transform.position.z);
        //return distance <= bodyA.radius;
        Vector3 normal = halfSpace.transform.rotation * new Vector3(0, 1, 0);
        Vector3 displacement = sphere.transform.position - halfSpace.transform.position;
        float projection = Vector3.Dot(displacement, normal);
        return projection == sphere.radius;
    }
    bool checkSphereHalfPlaneCollision(Body sphere, Body halfSpace)
    { 
        Vector3 normal = halfSpace.transform.rotation * new Vector3(0,1,0);
        Vector3 displacement = sphere.transform.position - halfSpace.transform.position;
        float projection = Vector3.Dot(displacement, normal);   
        return projection < sphere.radius;
    }
    private void checkCollision()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            Body bodyA = bodies[i];
            for (int j = 0; j < bodies.Count; j++)
            {
                Body bodyB = bodies[j];
                //checks for collision detection type
                if (bodyA.GetShape() == 0 && bodyB.GetShape() == 0)
                {
                    if (checkSphereSphereCollision(bodyA, bodyB))
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    }
                    else
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    }
                }
                else if (bodyA.GetShape() == 0 && bodyB.GetShape() == 2)
                {
                    if (checkSpherePlaneCollision(bodyA, bodyB))
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    }
                    else
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    }
                }
                else if (bodyB.GetShape() == 0 && bodyA.GetShape() == 2)
                {
                    if (checkSpherePlaneCollision(bodyB, bodyA))
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    }
                    else
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    }
                }
                else if (bodyA.GetShape() == 0 && bodyB.GetShape() == 3)
                {
                    if (checkSphereHalfPlaneCollision(bodyA, bodyB))
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    }
                    else
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    }
                }
                else if (bodyB.GetShape() == 0 && bodyA.GetShape() == 3)
                {
                    if (checkSphereHalfPlaneCollision(bodyB, bodyA))
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    }
                    else
                    {
                        bodyA.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    }
                }
            }
        }
    }
    private void printToConsole() 
    {
        Debug.ClearDeveloperConsole();
        int printedProjectiles = 0;
        Debug.Log("Launched Projectiles: \n\n");
        for (int i = bodies.Count; i > 0; i--) 
        {
            if (bodies[i-1].ProjectileLaunchState == 2) 
            {
                Debug.Log("Projectile " + printedProjectiles.ToString() + ": \n ElevationAngle: " + bodies[i-1].startElevationAngle.ToString() + "\n"
                                                        + "Distance Travelled: " + bodies[i-1].distanceTravelled.ToString() + "\n"
                                                        + "Time in air: " + bodies[i-1].flightTime.ToString() + "\n");
                printedProjectiles++;
            }
        }
    }
    private void Start()
    {
        dt = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        checkForNewBodies();
        checkCollision();
        foreach (Body body in bodies)
        {
            if (body.isProjectile)
            {
                //checks if projectile is launching 
                if (body.ProjectileLaunchState == 0)
                {
                    body.ProjectileLaunchState = 1;
                    body.startPos = body.transform.position;
                    body.Simulate(grav, dt);
                    body.transform.localPosition += new Vector3(
                        (body.vel.x * dt) * body.drag,
                        (body.vel.y * dt) * body.drag,
                        (body.vel.z * dt) * body.drag);
                    body.flightTime += dt;
                }
                //checks if projectile is landed
                else if (body.transform.position.y <= 0 && body.ProjectileLaunchState == 1)
                {
                    body.ProjectileLaunchState = 2;
                    body.endPos = body.transform.position;
                    body.distanceTravelled = body.endPos.x - body.startPos.x;
                }
                //checks if projectile is in the air
                else if(body.ProjectileLaunchState == 1)
                {
                    if ((body.transform.position.y + body.vel.y * dt) <= 0) 
                    {
                        body.Simulate(grav, dt);
                        body.transform.localPosition += new Vector3(
                            ((body.vel.x*(body.transform.position.y /body.vel.y)) * dt) * body.drag,
                            ((body.vel.y * (body.transform.position.y / body.vel.y)) * dt) * body.drag,
                            ((body.vel.z) * dt) * body.drag);
                        body.transform.position = new Vector3(body.transform.position.x, 0.0f, body.transform.position.z);
                        body.flightTime += dt;
                    }
                    else
                    {
                        body.Simulate(grav, dt);
                        body.transform.localPosition += new Vector3(
                            (body.vel.x * dt) * body.drag,
                            (body.vel.y * dt) * body.drag,
                            (body.vel.z * dt) * body.drag);
                        body.flightTime += dt;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            printToConsole();
        }
        //Simulate(Physics.gravity, Time.fixedDeltaTime);
        //transform.position = new Vector3(
        //    transform.position.x + (vel.x * dt) * drag,
        //    transform.position.y + (vel.y * dt) * drag,
        //    transform.position.z + (vel.z * dt) * drag
        //);
    }
}