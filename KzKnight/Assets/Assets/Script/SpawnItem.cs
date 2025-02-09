using UnityEngine;
using Photon.Pun;        
using System.Collections;
using System.Collections.Generic;

public class SpawnItem : MonoBehaviourPun
{
    [SerializeField]
    private List<GameObject> itemPrefabs; // Danh sách prefab cần spawn

    [SerializeField]
    private float spawnInterval = 4f; // Thời gian giữa các lần spawn (n giây)

    private float minX = -7f, maxX = 7f, minY = -5f, maxY = 7f;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            this.enabled = false;  // Tắt GameManager trên máy không phải Master Client
            return;
        }
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Chờ spawnInterval giây trước mỗi lần spawn
            yield return new WaitForSeconds(spawnInterval);

            // Tạo vị trí ngẫu nhiên trong khoảng đã định
            Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            // Chọn một prefab ngẫu nhiên từ danh sách
            int randomIndex = Random.Range(0, itemPrefabs.Count);

            // Instantiate item thông qua PhotonNetwork để đồng bộ trên mạng
            PhotonNetwork.Instantiate(itemPrefabs[randomIndex].name, randomPos, Quaternion.identity);
        }
    }
}
