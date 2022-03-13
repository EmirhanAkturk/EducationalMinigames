using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnPoint
{
    SpawnPointType SpawnPointType { get; }
    void Spawn();
}
