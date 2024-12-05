using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePlatformSpawner : MonoBehaviour
{
    public float speed;             // platform moving speed
    public int startingPoint;       // starting position of the platform
    public Transform[] points;       // an array of transform points (positions where the platform needs to move)

    private int i = 0;  //index of array

    private void Start()
    {
        transform.position = points[startingPoint].position;    // setting position of platform to one of the points positions using startingPoint

    }

    private void Update()
    {
        // checking the distance of the platform and the point
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            if (i == points.Length - 1)  // check if platform was on the last point after index increase
            {
                RespawnPlatform();
            }
            else
            {
                i++;
            }
           
        }

        // moving the platform to the point position with the index "i"
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void RespawnPlatform()
    {
        i = 0;
        transform.position = points[startingPoint].position;
    }

    // moving player with platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }

}
