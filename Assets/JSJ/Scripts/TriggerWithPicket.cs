using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWithPicket : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas_PicketNotice;

    bool enterPicketZone = false;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;
    }

    // Trigger ������ Player �� ���� ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player") && !enterPicketZone)
        {
            canvas_PicketNotice.SetActive(true);
            enterPicketZone = true;
        }
    }

    // Trigger ������ Player �� ������ ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enterPicketZone)
        {
            canvas_PicketNotice.SetActive(false);
            enterPicketZone = false;
        }
    }
}
