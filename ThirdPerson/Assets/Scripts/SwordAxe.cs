using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAxe : MonoBehaviour
{

    public bool activated;

    public float sAxerotationSped;


    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += transform.forward * sAxerotationSped * Time.deltaTime;
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 9 )
        {
            print(collision.gameObject.name);
            activated = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            GetComponent<Rigidbody>().Sleep();
           
        }
        
    }
}
