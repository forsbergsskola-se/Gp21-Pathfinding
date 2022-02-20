using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowTest : MonoBehaviour
{
    
    public float speed;
    public Transform target;
    public float minimumDistance;
    
    void Update()
    {
        // using guide: https://www.youtube.com/watch?v=dmQyfWxUNPw&list=PLVYYdtLY07hTstdXRVAQGR0YaNH4pnvpv&index=1
        if (Vector2.Distance(transform.position, target.position) > minimumDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                target.position, speed * Time.deltaTime);
        }

        
    }
}
