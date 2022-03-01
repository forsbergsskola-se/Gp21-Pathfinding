using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowTest : MonoBehaviour
{
    
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationDamp = .5f;
    [SerializeField] Transform target;
    [SerializeField] float rayCastOffset = 1.5f;
    [SerializeField] float detectionDistance = 10f;
    [SerializeField] float minimumDistance;
    
    void Update()
    {
        if (!FindTarget())
            return;
        Move();
        PathFinding();
    }

    void PathFinding()
    {
        RaycastHit hit;
        Vector3 raycastOffset = Vector3.zero;
    
        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;
        
        Debug.DrawRay(left, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(right, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(up, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(down, transform.forward * detectionDistance, Color.red);
        
        if (Physics.Raycast(left, transform.forward, out hit, detectionDistance))
            raycastOffset += Vector3.right;
        else if (Physics.Raycast(right, transform.forward, out hit, detectionDistance))
            raycastOffset -= Vector3.right;
        if (Physics.Raycast(up, transform.forward, out hit, detectionDistance))
            raycastOffset -= Vector3.up;
        else if (Physics.Raycast(down, transform.forward, out hit, detectionDistance))
            raycastOffset += Vector3.up;
    
        if (raycastOffset != Vector3.zero)
            transform.Rotate(raycastOffset * 5f * Time.deltaTime);
        else
            Turn();
        
    }

    bool FindTarget()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        if (target == null)
            return false;

        return true;
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
