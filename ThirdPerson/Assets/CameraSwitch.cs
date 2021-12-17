using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject FirstPersonCam;
    public GameObject ThirdPersonCam;
    public GameObject Crosshair;
   
    public int CamMode;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Cam"))
        {

            if (CamMode == 1)
            {
                CamMode = 0;
                Crosshair.SetActive(true);
            }

            else
            {
                CamMode += 1;
                Crosshair.SetActive(false);
            }

            
            StartCoroutine (CamChange());
        }
    }
        IEnumerator CamChange()
        {
            yield return new WaitForSeconds(0.1f);
            if (CamMode == 0)
            {

                FirstPersonCam.SetActive (true);
                ThirdPersonCam.SetActive (false);

            }
            if (CamMode == 1)
            {

                FirstPersonCam.SetActive(false);
                ThirdPersonCam.SetActive(true);

            }
                
        }

    
}
