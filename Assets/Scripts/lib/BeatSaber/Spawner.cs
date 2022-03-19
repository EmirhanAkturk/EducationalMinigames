using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PoolType[] cubeTypes;
    [SerializeField] private Transform[] points;
    [SerializeField] private float beat = (60 / 130) * 2;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > beat)
        {
            var cubeType = cubeTypes[Random.Range(0, cubeTypes.Length)];
            var cube = PoolingSystem.Instance.Create(cubeType, points[Random.Range(0, points.Length)]);
            cube.transform.localPosition = Vector3.zero;
            cube.transform.Rotate(transform.forward, 90 * Random.Range(0, 4));
            timer -= beat;
        }

        timer += Time.deltaTime;
    }
}
