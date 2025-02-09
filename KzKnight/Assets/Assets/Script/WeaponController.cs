using Photon.Pun;
using UnityEngine;

public class WeaponController : MonoBehaviourPunCallbacks
{
    private GameObject Player;
    [Header("Tham chiếu đối tượng")]
    private Transform player;         // Transform của nhân vật (để weapon đi theo)
    public Transform firePoint;      // FirePoint nằm ở đầu weapon (nơi bắn đạn)
    public GameObject bulletPrefab;   // Prefab của viên đạn
    public PhotonView view;
    [Header("Cài đặt")]
    public float rotationSpeed = 500f;   // Tốc độ xoay của weapon
    public Vector3 offset;               // Offset từ vị trí của nhân vật đến weapon (nếu cần)

    private bool facingRight = true;     // Trạng thái facing của weapon (theo nhân vật)

    private void Start()
    {
        view = this.GetComponent<PhotonView>();
        Player = GameObject.FindGameObjectWithTag("Player");
        player = Player.transform;
    }
    void Update()
    {
        if(view.IsMine)
        {
            // 1. Cập nhật vị trí weapon theo nhân vật (với offset nếu cần)
            transform.position = player.position + offset;

            // 2. Flip weapon dựa vào vị trí chuột so với nhân vật
            UpdateFacingDirection();

            // 3. Xoay weapon sao cho "đầu" (firePoint) luôn hướng về phía chuột
            RotateWeaponToMouse();
            if (Input.GetButtonDown("Fire1"))
            {
                FireBullet();
            }
        }
        
    }
    /// <summary>
    /// Tạo và bắn viên đạn theo hướng firePoint.
    /// </summary>
    void FireBullet()
    {
        // Tạo viên đạn tại vị trí firePoint với góc quay hiện tại
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, Quaternion.identity);

        // Thêm lực cho viên đạn để nó bay theo hướng firePoint.right
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Nếu weapon facing right, dùng firePoint.right; nếu facing left, dùng -firePoint.right
            Vector2 direction = facingRight ? firePoint.right : -firePoint.right;
            rb.velocity = direction * 20f;
        }

        // Hủy viên đạn sau 2 giây
        Destroy(bullet, 3f);
    }

    /// <summary>
    /// Cập nhật facing của weapon (flip) dựa vào vị trí chuột so với nhân vật.
    /// Nếu chuột bên phải, weapon hướng phải; nếu bên trái, weapon flip (trái).
    /// </summary>
    void UpdateFacingDirection()
    {
        // Lấy vị trí chuột trong không gian thế giới (đảm bảo z=0)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // So sánh vị trí chuột với nhân vật
        if (mousePos.x >= player.position.x && !facingRight)
        {
            Flip();
        }
        else if (mousePos.x < player.position.x && facingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// Xoay weapon theo chuột.
    /// Nếu weapon bị flip (facing left) thì cộng thêm 180° vào góc để đảm bảo firePoint luôn hướng ra ngoài.
    /// </summary>
    void RotateWeaponToMouse()
    {
        // Lấy vị trí chuột trong không gian thế giới
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Tính vector hướng từ weapon đến chuột
        Vector3 direction = (mousePos - transform.position).normalized;

        // Tính góc (độ) giữa vector đó và trục X
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Nếu weapon đang facing left (flip) thì cần điều chỉnh góc xoay
        if (!facingRight)
        {
            angle += 180f;
        }

        // Tạo Quaternion cho góc xoay mục tiêu
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // Xoay weapon mượt mà về targetRotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Flip weapon bằng cách đảo ngược scale.x.
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x = -localScale.x;
        transform.localScale = localScale;
    }
}
