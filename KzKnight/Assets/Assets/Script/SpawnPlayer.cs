using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject weaponPrefab;

    private float minX = -7f, maxX = 7f, minY = -5f, maxY = 7f;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 randomPos = new Vector2(Random.Range(minX,maxX),Random.Range(minY,maxY));
        if (playerPrefab != null && weaponPrefab != null)
        {
            GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);
            GameObject weaponObj = PhotonNetwork.Instantiate(weaponPrefab.name, randomPos, Quaternion.identity);

            // Gán weapon làm con của player để weapon tự động theo dõi player
            //weaponObj.transform.SetParent(playerObj.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
