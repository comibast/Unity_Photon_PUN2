using UnityEngine;
using Photon.Pun;

namespace Comibast
{
    /// <summary>
    /// �ͦ����G
    /// </summary>
    public class SpawnFruit : MonoBehaviour
    {
        [SerializeField, Header("���")]
        private GameObject prefabFruit;
        [SerializeField, Header("�ͦ��W�v"), Range(0, 5)]
        private float intervalSpawn = 2.5f;
        [SerializeField, Header("�ͦ��I")]
        private Transform[] spawnPoints;

        private void Awake()
        {
            //�p�G �O �@���D�����Ȥ� �~����ͦ�
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
