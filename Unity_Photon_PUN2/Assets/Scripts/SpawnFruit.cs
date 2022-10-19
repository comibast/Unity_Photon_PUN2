using UnityEngine;
using Photon.Pun;

namespace Comibast
{
    /// <summary>
    /// 生成水果
    /// </summary>
    public class SpawnFruit : MonoBehaviour
    {
        [SerializeField, Header("草莓")]
        private GameObject prefabFruit;
        [SerializeField, Header("生成頻率"), Range(0, 5)]
        private float intervalSpawn = 2.5f;
        [SerializeField, Header("生成點")]
        private Transform[] spawnPoints;

        private void Awake()
        {
            //如果 是 作為主機的客戶 才執行生成
            if (PhotonNetwork.IsMasterClient)
            {
                InvokeRepeating("Spawn", 0, intervalSpawn);
            }
        }

        private void Spawn()
        {
            int random = Random.Range(0, spawnPoints.Length);
            PhotonNetwork.Instantiate(prefabFruit.name, spawnPoints[random].position, Quaternion.identity);
        }

    }

}
