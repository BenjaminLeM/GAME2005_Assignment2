
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Launch : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Body body;
    private float launchSpeed = 0.0f;
    private float elevationAngle = 0.0f;
    private float drag = 1.0f;
    private float mass = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        launchSpeed = 120.0f;
        for (int i = 0; i < 19; i++)
        {
            if (i == 0)
            {
                elevationAngle = 0.0f;
                Shoot();
            }
            else if (i == 1)
            {
                elevationAngle = 3.19f;
                Shoot();
            }
            else if (i == 2)
            {
                elevationAngle = 6.42f;
                Shoot();
            }
            else if (i == 3)
            {
                elevationAngle = 9.736f;
                Shoot();
            }
            else if (i == 4)
            {
                elevationAngle = 13.194f;
                Shoot();
            }
            else if (i == 5)
            {
                elevationAngle = 16.874f;
                Shoot();
            }
            else if (i == 6)
            {
                elevationAngle = 20.905f;
                Shoot();
            }
            else if (i == 7)
            {
                elevationAngle = 25.529f;
                Shoot();
            }
            else if (i == 8)
            {
                elevationAngle = 31.367f;
                Shoot();
            }
            else if (i == 9)
            {
                elevationAngle = 45f;
                Shoot();
            }
            else if (i == 10)
            {
                elevationAngle = 58.633f;
                Shoot();
            }
            else if (i == 11)
            {
                elevationAngle = 64.471f;
                Shoot();
            }
            else if (i == 12)
            {
                elevationAngle = 69.095f;
                Shoot();
            }
            else if (i == 13)
            {
                elevationAngle = 73.126f;
                Shoot();
            }
            else if (i == 14)
            {
                elevationAngle = 76.806f;
                Shoot();
            }
            else if (i == 15)
            {
                elevationAngle = 80.264f;
                Shoot();
            }
            else if (i == 16)
            {
                elevationAngle = 83.58f;
                Shoot();
            }
            else if (i == 17)
            {
                elevationAngle = 86.61f;
                Shoot();
            }
            else if (i == 18)
            {
                elevationAngle = 90.0f;
                Shoot();
            }
        }
    }
    public void Shoot()
    {
        GameObject newObject = Instantiate(projectilePrefab);
        newObject.transform.position = transform.position;
        body = newObject.GetComponent<Body>();
        body.vel = new Vector3(launchSpeed * Mathf.Cos(Mathf.Deg2Rad * elevationAngle),
                                launchSpeed * Mathf.Sin(Mathf.Deg2Rad * elevationAngle),
                                0);
        body.drag = drag;
        body.weight = mass;
        body.startElevationAngle = elevationAngle;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Shoot();
        //}
    }
}