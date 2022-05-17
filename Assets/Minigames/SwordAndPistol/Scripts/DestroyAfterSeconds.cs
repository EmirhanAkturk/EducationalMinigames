using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private PoolType poolType;
    [SerializeField] private float seconds = 2.0f;

    private Coroutine destroyCoroutine;
    
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, seconds);
        return;
        destroyCoroutine = CoroutineDispatcher.ExecuteWithDelay(() =>
        {
            destroyCoroutine = null;
            DestroyObject();
        }, seconds);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
        return;
        if (!gameObject.activeSelf) return;
        if(destroyCoroutine != null) CoroutineDispatcher.StopCoroutine(destroyCoroutine);
        PoolingSystem.Instance.Destroy(poolType, gameObject);
    }
}
