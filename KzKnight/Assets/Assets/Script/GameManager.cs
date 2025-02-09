using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject weaponPrefab;

    private float minX = -7f, maxX = 7f, minY = -5f, maxY = 7f;

    public override void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("đã load Scene");
        // Chỉ spawn player nếu đối tượng player chưa tồn tại
        if (PhotonNetwork.IsConnected && playerPrefab != null && weaponPrefab != null)
        {
            Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);
            PhotonNetwork.Instantiate(weaponPrefab.name, randomPos, Quaternion.identity);
        }
    }
}
