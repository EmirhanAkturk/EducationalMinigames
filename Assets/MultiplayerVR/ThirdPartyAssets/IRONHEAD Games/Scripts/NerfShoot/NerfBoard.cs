using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class NerfBoard : MonoBehaviour
{
    [Header("Random X range")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [Header("Random Y range")]
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private Vector3? startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag($"Bullet")) return;
        UpdatePos();
    }

    private void UpdatePos()
    {
        startPos ??= transform.position;
        transform.position = startPos.Value + new Vector3(0, Random.Range(minY, maxY), Random.Range(minX, maxX));
    }

    private void OnDrawGizmos()
    {
        var cubePos = startPos ?? transform.position;
        Gizmos.DrawCube(cubePos, new Vector3(1, (maxX - minX), (maxY - minY)));
    }
}

