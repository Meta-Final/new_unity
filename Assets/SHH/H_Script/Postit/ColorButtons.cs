using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtons : MonoBehaviour
{
    public InventoryUI inventoryUI;
    Color myColor;
    public GameObject colorbtn;
    void Start()
    {
        myColor = GetComponent<Image>().color;
        GetComponent<Button>().onClick.AddListener(TransferMyColor);
    }



    void TransferMyColor()
    {
        inventoryUI.SetPostItColor(myColor);
    }
    void CloseColorbtn()
    {
        colorbtn.SetActive(false);
    }
}
