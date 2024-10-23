using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using static UnityEngine.Rendering.DebugUI.Table;

public class ScreenCaptureManager : MonoBehaviour
{
    // 드래그 영역을 보여주는 사각형
    private Rect dragArea;

    //드래그 확인 변수
    private bool isDragging = false;

    //드래그 시작 지점 위치
    private Vector2 startPosition;

    //캡쳐된 이미지를 저장 변수
    public Texture2D capturedTexture;

    // 캡처된 이미지 유니티 화면에 미리보기
    public RawImage displayImage;

    bool isCapturing = false;

    void Update()
    {
        // 마우스의 왼쪽 버튼을 눌렀을 때 드래그 시작
        if (Input.GetMouseButtonDown(0) && isCapturing)
        {
            StartDrag();
        }

        //드래그 중
        if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateDrag();
        }
        
        //마우스를 떼면 드래그 끝
            if (Input.GetMouseButtonUp(0) && isDragging)
        {
            EndDrag();
        }

        // 스페이스바 입력 처리
        if (Input.GetKeyDown(KeyCode.Space) && !isDragging)
        {
            StartCoroutine(CaptureScreen());
        }
    }

    private void StartDrag()
    {
        isDragging = true; //드래그 활성화
        startPosition = Input.mousePosition; //현재 마우스 위치 두기
        startPosition.y = Screen.height - startPosition.y; // Y 좌표 반전
    }

    private void UpdateDrag()
    {
        Vector2 currentMousePosition = Input.mousePosition; //현재 마우스 위치 두기
        currentMousePosition.y = Screen.height - currentMousePosition.y; // Y 좌표 반전

        // 드래그 영역 업데이트
        dragArea = Rect.MinMaxRect(
            Mathf.Min(startPosition.x, currentMousePosition.x),
            Mathf.Min(startPosition.y, currentMousePosition.y),
            Mathf.Max(startPosition.x, currentMousePosition.x),
            Mathf.Max(startPosition.y, currentMousePosition.y)
        );
    }

    private void EndDrag()
    {
        isDragging = false; //드래그 비활성화
        isCapturing = false;
    }

    private IEnumerator CaptureScreen()
    {
        // 잠시 대기하여 드래그가 완료된 후 캡처 한다
        yield return new WaitForEndOfFrame();
        print("캡쳐성공!");

        // 드래그 영역의 크기와 위치를 기반으로 텍스처 생성
        capturedTexture = new Texture2D((int)dragArea.width, (int)dragArea.height, TextureFormat.RGB24, false);

        // ReadPixels 메서드의 y 좌표 반전을 위해 현재 드래그 영역의 Y 좌표를 조정
        capturedTexture.ReadPixels(new Rect(dragArea.x, Screen.height - dragArea.yMax, dragArea.width, dragArea.height), 0, 0);
        capturedTexture.Apply();

        // RawImage에 캡처된 텍스쳐를 보여준다.
        if (displayImage != null)
        {
            displayImage.texture = capturedTexture;
        }

        // 결과를 파일로 저장
        byte[] bytes = capturedTexture.EncodeToPNG();
        string now = DateTime.Now.ToString();
        now = now.Replace(":", "-");
        System.IO.File.WriteAllBytes(Path.Combine(Application.dataPath, now + ".png"), bytes); //경로설정
        print("저장성공!");

      
        // 이미지를 인벤토리에 추가
        //string base64 = System.Convert.ToBase64String(bytes);
        //InventoryManager.instance.addScreen(base64);
    }

    public void OnClickedButton()
    {
        isCapturing = true;
    }

    
    private void OnGUI()
    {
        // 드래그 영역 색상 및 모양
        if (isDragging)
        {
            GUI.color = new Color(1, 1, 0, 0.5f); // 반투명 노란색
            GUI.DrawTexture(dragArea, Texture2D.whiteTexture);
        }
    }
}
