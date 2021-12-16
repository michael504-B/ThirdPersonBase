using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SwordAxe swordAxe;
    [Header("Movement Variables")]

    [SerializeField]
    float moveSpeed = 3.0f;

    [Header("References")]

    [SerializeField]
    Transform mainCamera;

    [SerializeField]
    BoxCollider swordCollider;

    public Rigidbody swordAxeRb;
    Rigidbody rb;

    Animator anim;

    bool startedCombo = false;
    bool swordAxeReturn = false;
    bool aiming = false;
    bool activated;


    float timeSinceButtonPressed = 0.0f;

    [SerializeField]
    float saxRotation;

    [SerializeField]
    float throwStr = 25;
    
    [SerializeField]
    Camera cam;

    [SerializeField]
    float defaultFOV = 90;
    

   


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        swordAxe = swordAxe.GetComponent<SwordAxe>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        var camForward = mainCamera.forward;
        var camRight = mainCamera.right;

        camForward.y = 0;
        camForward.Normalize();
        camRight.y = 0;
        camRight.Normalize();

        var moveDirection = (camForward * v * moveSpeed) + (camRight * h * moveSpeed);

        transform.LookAt(transform.position + moveDirection);
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        anim.SetFloat("moveSpeed", Mathf.Abs(moveDirection.magnitude));

        if(Input.GetButtonDown("Jump") && !startedCombo)
        {
            anim.SetTrigger("swordCombo");
            startedCombo = true;
        }

        if(Input.GetButtonDown("Jump") && startedCombo)
        {
            timeSinceButtonPressed = 0;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("throw");
            //SwordAxeThorw();
        }
       if (activated)
       {
            transform.localEulerAngles += transform.forward * saxRotation * Time.deltaTime;
       }
      
        /*code dont work
        if (Input.GetButton("Fire2"))
        {
            Aiming();
        }
        else
            cam.fieldOfView = (defaultFOV);
        */

        timeSinceButtonPressed += Time.deltaTime;
    }

    public void PotentialComboEnd()
    {
        TurnOffSwordCollider();

        if (timeSinceButtonPressed < 0.5f)
            return;

        anim.SetTrigger("stopCombo");
        startedCombo = false;
        timeSinceButtonPressed = 0;
        
    }

    public void EndOfCombo()
    {
        startedCombo = false;
        timeSinceButtonPressed = 0;
        TurnOffSwordCollider();
    }

    public void TurnOnSwordCollider()
    {
        swordCollider.enabled = true;
    }

    public void TurnOffSwordCollider()
    {
        swordCollider.enabled = false;
    }

    public void SwordAxeThorw()
    {
        swordAxeReturn = false;
        swordAxe.activated = true;
        swordAxeRb.isKinematic = false;
        swordAxeRb.transform.parent = null;
        swordAxeRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        swordAxeRb.AddForce(transform.forward * throwStr,ForceMode.Impulse);
    }

    public void ReturnSwordAxe()
    {
        swordAxeReturn = true;
        swordAxeRb.isKinematic = true;
    }

    public void Aiming()
    {

        cam.fieldOfView = (defaultFOV / 2);

    }

    public void OnCollisionEnter(Collision collision)
    {
        activated = false;
        //GetComponent<Rigidbody>().isKinematic = true;
    }


}
