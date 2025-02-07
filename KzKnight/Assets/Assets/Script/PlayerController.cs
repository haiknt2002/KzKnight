using Assets.Assets.Script.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;
    enum PlayerState
    {
        Stand,
        Run,
        Dead
    }

    private Animator anim;
    private PlayerState currentState = PlayerState.Stand;


    public float speed;
    private Rigidbody2D rb;
    private Vector2 movement;

    private bool facingRight = true;

    void Start()
    {
        // Khởi tạo thuộc tính Player ban đầu
        this.player = new Player();

        this.speed = player.Speed;
        rb = GetComponent<Rigidbody2D>();
        anim = rb.GetComponent<Animator>();
    }

    void Update()
    {
        // Lấy input từ bàn phím
        movement.x = Input.GetAxisRaw("Horizontal"); // A, D hoặc ←, →
        movement.y = Input.GetAxisRaw("Vertical");   // W, S hoặc ↑, ↓

        // Xoay nhân vật trái - phải
        RotatePlayerToMouse();

        // Cập nhật trạng thái dựa trên input
        if (movement.magnitude > 0)
        {
            currentState = PlayerState.Run;
        }
        else
        {
            currentState = PlayerState.Stand;
        }

        // Gửi trạng thái mới cho Animator
        anim.SetInteger("PLayerState", (int)currentState);

        if(player.HP < 0) { this.currentState = PlayerState.Dead; Die(); }
    }
    void RotatePlayerToMouse()
    {
        // Lấy vị trí chuột trong không gian thế giới
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;  // Đảm bảo chuột nằm trên mặt phẳng 2D

        // Tính toán hướng từ vị trí của nhân vật đến vị trí của chuột
        Vector3 direction = (mousePosition - this.transform.position).normalized;

        // Kiểm tra xem chuột có ở bên phải hay bên trái của nhân vật
        if (direction.x > 0 && !facingRight)
        {
            Flip(); // Nếu chuột bên phải và nhân vật đang quay trái, xoay lại
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip(); // Nếu chuột bên trái và nhân vật đang quay phải, xoay lại
        }
    }
    private void Flip()
    {
        // Đảo ngược chiều của sprite nhân vật theo trục X
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x = -localScale.x;  // Lật sprite
        transform.localScale = localScale;
    }

    void FixedUpdate()
    {
        // Di chuyển nhân vật
        rb.velocity = movement.normalized * speed;
    }
    public void Die()
    {
        currentState = PlayerState.Dead;
        anim.SetInteger("PLayerState", (int)currentState);
        rb.velocity = Vector2.zero; // Dừng di chuyển khi chết
    }
}
