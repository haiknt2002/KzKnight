using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Transform của nhân vật mà camera sẽ theo dõi
    public Transform target;
    [SerializeField]
    // Offset (khoảng cách giữa camera và nhân vật), có thể điều chỉnh trong Inspector
    private Vector3 offset = new Vector3(0,0,-10);

    // Tốc độ mượt của camera khi di chuyển (0 - 1, giá trị càng nhỏ di chuyển càng mượt)
    public float smoothSpeed = 0.025f;

    // Giới hạn Map (điều chỉnh các giá trị này sao cho phù hợp với kích thước Map của bạn)
    private float minX = -7f, maxX = 7f, minY = -5f, maxY = 7f;

    // Sử dụng LateUpdate để đảm bảo camera di chuyển sau khi nhân vật đã di chuyển
    void LateUpdate()
    {
        // Tính vị trí mong muốn của camera (vị trí của nhân vật cộng thêm offset)
        Vector3 desiredPosition = target.position + offset;

        // Giới hạn vị trí theo trục X và Y để không vượt quá giới hạn Map
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // Di chuyển mượt mà từ vị trí hiện tại đến vị trí mong muốn
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Cập nhật vị trí của camera
        transform.position = smoothedPosition;
    }
}
