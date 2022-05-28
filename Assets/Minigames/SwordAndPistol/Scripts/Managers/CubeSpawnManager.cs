using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.SwordAndPistol.Scripts.Managers
{
    public class CubeSpawnManager : Singleton<CubeSpawnManager>
    {
        [SerializeField] private GameObject[] Spawnpoints;
        [SerializeField] private GameObject[] Cubeprefabs;
        // [SerializeField] private float timeRate;

        private float CubeSpawnPeriod => GameManager.Instance.GetCurrentSpawnPeriod();
        private int index;
        private int indexcube;
        public bool CanSpawnCube => GameManager.Instance.IsPlaying;

        // private void OnEnable()
        // {
        //     EventService.OnGameStart.AddListener(StartSpawnCube);
        // }
        //
        // private void OnDisable()
        // {
        //     EventService.OnGameStart.RemoveListener(StartSpawnCube);
        // }

        private void Start()
        {
            StartCoroutine(CreateCubes());
        }

        private IEnumerator CreateCubes()
        {
            while (true)
            {
                if (CanSpawnCube)
                {
                    index = Random.Range(0, 4);
                    indexcube = Random.Range(0, 2);
                    GameObject cube = Instantiate(Cubeprefabs[indexcube], Spawnpoints[index].transform.position, Quaternion.Euler(0, 0, 0));
                    cube.transform.SetParent(transform);
                }

                yield return new WaitForSeconds(CubeSpawnPeriod);
            }
        }

    }
}
