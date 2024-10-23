using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Input.GetMouseButtonDown(0))
        {
            PicketAndGoToScene();

        }
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

    public void PicketAndGoToScene()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(cameraRay, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Picket"))
            {
                LoadNextScene();
            }
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("PicketArticle_Scene");
    }
}
