using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RayController : MonoBehaviour
{
    public Transform rayPos;

    public GameObject canvasNotice;
    
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Meta_ScrapBook_Scene")
        {
            canvasNotice = GameObject.Find("Canvas_Interactive");
        }
        else
        {
            canvasNotice = null;
        }
        
        if (canvasNotice != null)
        {
            canvasNotice.SetActive(false);
        }
    }

    
    void Update()
    {
        InteractObject();
        
    }

    public void InteractObject()
    {
        Ray ray = new Ray(rayPos.position, rayPos.forward);
        RaycastHit rayHitInfo;

        if (Physics.Raycast(ray, out rayHitInfo, 5f))
        {
            if (rayHitInfo.collider.gameObject.layer == 21)
            {
                print(rayHitInfo.collider.name);
                if (Input.GetKey(KeyCode.K))
                {
                    MetaConnectionMgr.instance.TownToEyesMagazine();
                }
            }
        }

        if (Physics.Raycast(ray, out rayHitInfo, 3f))
        {
            if (rayHitInfo.collider.gameObject.layer == 22)
            {
                print(rayHitInfo.collider.name);
                canvasNotice.SetActive(true);

            }
            else
            {
                canvasNotice.SetActive(false);
            }

        }
    }

    
}
