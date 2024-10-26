using UnityEngine;

public class CameraController : MonoBehaviour
{
      public Transform player; // �÷��̾� Transform
      public float distance = 10f; // ī�޶�� �÷��̾� ���� �Ÿ�
      public float height = 5f; // ī�޶��� ����
    public float scrollSpeed = 2f; // ���콺 �� �ӵ�
    public float mouseSensitivity = 5f; // ���콺 ����
      private float rotationX = 0f; // ī�޶��� ���� ȸ��
      private float rotationY = 0f; // ī�޶��� ���� ȸ��
   
      void Update()
      {
          Cursorlock();

          // ���콺 �Է¿� ���� ȸ�� ���� ����
          rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
          rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
          rotationY = Mathf.Clamp(rotationY, -20f, 80f); // ���� ȸ�� ���� ����

        // ���콺 �� �Է¿� ���� ī�޶� �Ÿ� ����
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * scrollSpeed; // �Ÿ� ����
        distance = Mathf.Clamp(distance, 5f, 20f); // �ּ�, �ִ� �Ÿ� ����


        // ī�޶��� ȸ�� ����
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
          Vector3 position = player.position - rotation * Vector3.forward * distance + Vector3.up * height;
    
          // ī�޶� ��ġ�� ȸ�� ����
          transform.position = position;
          transform.rotation = rotation;
       }

    public void Cursorlock()
    {
        // UI Ŭ�� ���¸� Ȯ��
        if (Input.GetMouseButtonDown(0))
        {
            // UI ��ҿ� Ŭ���� ���
            if (IsPointerOverUIObject())
            {
                Cursor.lockState = CursorLockMode.None; // Ŀ�� ���� ����
                return; // UI Ŭ�� �� ī�޶� ȸ�� �ڵ� ���� �ߴ�
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // UI�� �ƴ� ��� ����
            }
        }
    }

    // ���콺 �����Ͱ� UI ��� ���� �ִ��� Ȯ���ϴ� �޼��� (���� �˻�)
    private bool IsPointerOverUIObject()
    {
        // EventSystem�� ���� ���콺 ��ġ���� UI�� �ִ��� Ȯ��
        UnityEngine.EventSystems.PointerEventData eventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

}
