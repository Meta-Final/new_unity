using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWith3DObject : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas_3DObject;

    bool enter3DObject = false;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;
        canvas_3DObject = transform.GetChild(1).gameObject;
    }

    // Trigger ������ Player �� ���� ��
    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (other.CompareTag("Player") && !enter3DObject)
            {
                PhotonView pv = other.GetComponent<PhotonView>();
                if (pv.IsMine)
                {
                    canvas_3DObject.SetActive(true);
                }

                enter3DObject = true;
            }

        }
        
    }

    // Trigger ������ Player �� ������ ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enter3DObject)
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                canvas_3DObject.SetActive(false);
            }

            enter3DObject = false;
        }
    }
}
