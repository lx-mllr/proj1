using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawner", menuName = "SpawnerSettings/EnemySpawner")]
public class EnemySpawnSettings : ScriptableObject
{
    public float SpawnRate;
    public float MoveSpeed;
    public float AttackStr;
}
