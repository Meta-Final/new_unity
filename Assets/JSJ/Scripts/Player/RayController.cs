using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour
{
    public Transform rayPos;
    public float distance = 10f;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        Ray ray = new Ray(rayPos.position, rayPos.forward);
        RaycastHit rayHitInfo;

        if (Physics.Raycast(ray, out rayHitInfo, distance))
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


        
    }
}
