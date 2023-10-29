using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : Shape
{
    // Using transform.position since we're deriving from MonoBehaviour!
    public Vector3 startPos = new Vector3();
    public Vector3 endPos = new Vector3();
    public float startElevationAngle = 0.0f;
    public float distanceTravelled = 0.0f;
    public Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);
    public float weight = 1.0f;
    public float drag = 0.0f;
    public float radius = 0.5f;
    public int ProjectileLaunchState = 0;
    // 0 is starting launch 
    // 1 is launched
    // 2 is launch finished
    public bool isProjectile = true;

    public void Simulate(Vector3 acc, float dt)
    {
        vel = vel + acc * weight * dt;
        transform.position = transform.position + vel * dt;
    }
}