using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    float damage = 10f;
    [SerializeField] private List<GameObject> items;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PhotonView targetView = collision.GetComponent<PhotonView>();

            // Chỉ xử lý nếu đối tượng va chạm KHÔNG phải chủ sở hữu viên đạn
            if (targetView != null && !targetView.IsMine)
            {
                targetView.RPC("TakeDamage", RpcTarget.AllBuffered, damage);
                PhotonNetwork.Destroy(gameObject); // Hủy đạn sau khi va chạm
            }
        }
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            DropItem(collision.transform);
        }
    }
    void DropItem(Transform pos)
    {
        int i = Random.Range(0, items.Count);
        Instantiate(items[i], pos);
    }
}
