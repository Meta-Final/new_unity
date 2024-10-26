using UnityEngine;

public class CameraController : MonoBehaviour
{
      public Transform player; // 플레이어 Transform
      public float distance = 10f; // 카메라와 플레이어 간의 거리
      public float height = 5f; // 카메라의 높이
    public float scrollSpeed = 2f; // 마우스 휠 속도
    public float mouseSensitivity = 5f; // 마우스 감도
      private float rotationX = 0f; // 카메라의 수평 회전
      private float rotationY = 0f; // 카메라의 수직 회전
   
      void Update()
      {
          Cursorlock();

          // 마우스 입력에 따라 회전 각도 변경
          rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
          rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
          rotationY = Mathf.Clamp(rotationY, -20f, 80f); // 수직 회전 각도 제한

        // 마우스 휠 입력에 따라 카메라 거리 조정
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * scrollSpeed; // 거리 조정
        distance = Mathf.Clamp(distance, 5f, 20f); // 최소, 최대 거리 설정


        // 카메라의 회전 설정
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
          Vector3 position = player.position - rotation * Vector3.forward * distance + Vector3.up * height;
    
          // 카메라 위치와 회전 적용
          transform.position = position;
          transform.rotation = rotation;
       }

    public void Cursorlock()
    {
        // UI 클릭 상태를 확인
        if (Input.GetMouseButtonDown(0))
        {
            // UI 요소에 클릭한 경우
            if (IsPointerOverUIObject())
            {
                Cursor.lockState = CursorLockMode.None; // 커서 고정 해제
                return; // UI 클릭 시 카메라 회전 코드 실행 중단
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // UI가 아닌 경우 고정
            }
        }
    }

    // 마우스 포인터가 UI 요소 위에 있는지 확인하는 메서드 (몰라서 검색)
    private bool IsPointerOverUIObject()
    {
        // EventSystem을 통해 마우스 위치에서 UI가 있는지 확인
        UnityEngine.EventSystems.PointerEventData eventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

}
