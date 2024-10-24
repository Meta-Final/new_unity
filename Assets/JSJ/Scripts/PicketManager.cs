using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicketManager : MonoBehaviour
{
    public GameObject picketPrefab;
    public GameObject player;

    Vector3 picketPos;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        
    }

    public void MakePicket()
    {
        Ray ray = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 'PicketZone' 에만 Picket 이 생성
            if (hit.collider.gameObject.layer == 17)
            {
                picketPos = player.transform.position + player.transform.forward * 3f;

                GameObject obj = Instantiate(picketPrefab, picketPos, Quaternion.identity);
                obj.transform.LookAt(player.transform);

            }
        }
        
    }
}
