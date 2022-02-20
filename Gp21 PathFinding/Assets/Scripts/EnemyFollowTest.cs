using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowTest : MonoBehaviour
{
    
    public float speed = 3f;
    public float rotationDamp = .5f;
    public Transform target;
    public float minimumDistance;
    
    void Update()
    {
        Turn();
        Move();
    }

    void Turn()
    {
        Vector3 targetDist = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetDist);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationDamp * Time.deltaTime); 
    }

    void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
