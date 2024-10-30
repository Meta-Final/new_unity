using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PicketSceneManager : MonoBehaviour
{
    public GameObject stickerPrefab;
    public Canvas boardCanvas;
    public Button btn_Sticker;
    public Button btn_GoToTown;

    bool isActivate = false;

    void Start()
    {
        btn_Sticker.onClick.AddListener(ActivateMakeSticker);
        btn_GoToTown.onClick.AddListener(GoToTown);
    }

    void Update()
    {
        if (isActivate && Input.GetMouseButtonDown(0))
        {
            MakeSticker();
        }
        
    }


    public void GoToTown()
    {
        SceneManager.LoadScene("Meta_Town_Scene");
    }

    public void ActivateMakeSticker()
    {
        isActivate = true;
    }

    public void MakeSticker()
    {
        Vector3 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            boardCanvas.GetComponent<RectTransform>(),
            mousePosition,
            Camera.main,
            out Vector3 worldPosition);

        Instantiate(stickerPrefab, worldPosition, Quaternion.identity, boardCanvas.transform);


    }
}
