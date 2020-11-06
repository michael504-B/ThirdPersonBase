﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField]
    float moveSpeed = 1.0f;
    [SerializeField]
    float meleeRange = 1.0f;

    public LayerMask playerLayer;
    public LayerMask environmentLayer;

    [Header("References")]
    [SerializeField]
    PlayerController player;
    [SerializeField]
    Sight obstacleSight;

    Vector3 goalLocation;
    Vector3 currentTargetLocation;
    bool shouldMove = false;
    bool dead = false;

    float timeSinceChecked = 0;
    bool blocked = false;

    // Start is called before the first frame update
    void Awake()
    {
        shouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
            return;

        goalLocation = FindObjectOfType<PlayerController>().transform.position;

        //var fullVision = sight.Sight(90, 10, Color.red);
        var directlyInFront = obstacleSight.GetSightInformation();// (90, 3, 3, Color.blue);

        blocked = false;

        for(int i = 0; i < directlyInFront.Length; i++)
        {
            var current = directlyInFront[i];

            if (current.seen == true && current.tag == "Environment")
            {
                // we've got something blocking us
                blocked = true;
            }
            
        }

        Debug.Log(blocked);

        // if we're blocked, find the first sight that isn't obstructed and head that way
        if(blocked)
        {
            if (timeSinceChecked > 1)
            {
                HandleBlocked(directlyInFront);
                timeSinceChecked = 0;
            }
        }
        
        else
        {
            currentTargetLocation = goalLocation;
        }

        timeSinceChecked += Time.deltaTime;

        Debug.DrawLine(currentTargetLocation, currentTargetLocation + Vector3.up * 5, Color.green);


        if (shouldMove)
        {
            currentTargetLocation.y = 0;
            //transform.LookAt(currentTargetLocation);

            Quaternion toRotation = Quaternion.LookRotation(currentTargetLocation - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.5f * Time.deltaTime);
        }

        if (!shouldMove && Vector3.Distance(transform.position, goalLocation) > meleeRange * 2)
        {
            shouldMove = true;
        }


        if (Vector3.Distance(transform.position, goalLocation) < meleeRange)
        {
            shouldMove = false;
            //goalLocation = Vector3.zero;
        }

        

        GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed * (shouldMove ? 1 : 0);
        GetComponent<Animator>().SetFloat("moveSpeed", GetComponent<Rigidbody>().velocity.magnitude);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Sword" && !dead)
        {
            Destroy(gameObject, 3.0f);
        }
    }

    void HandleBlocked(SightInformation[] directlyInFront)
    {
        Vector3 acceptableLocation = Vector3.zero;
        bool found = false;
        for (int i = 0; i < directlyInFront.Length; i++)
        {
            var current = directlyInFront[i];


            if (current.seen == false)
            {
                if (!found)
                {
                    acceptableLocation = current.location;
                    found = true;

                }
            }
        }

        //currentTargetLocation = (goalLocation + acceptableLocation) / 2;
        currentTargetLocation = acceptableLocation;
    }

    
}