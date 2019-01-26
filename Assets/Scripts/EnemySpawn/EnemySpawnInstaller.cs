using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

public class EnemySpawnInstaller : MonoInstaller
{
    public EnemyView enemyPrefab;
    public List<EnemySpawnDescriptor> spawnDescriptors;

    public override void InstallBindings()
    {
        Container.DeclareSignal<RebindEnemySpawnSettings>().OptionalSubscriber();

        Container.BindSignal<RebindEnemySpawnSettings>().ToMethod(RebindSettings);
        RebindSettings();

        Container.BindFactory<EnemyView, EnemyView.Factory>().FromComponentInNewPrefab(enemyPrefab);
    }

    private void RebindSettings() {
        Container.Unbind<EnemySpawnSettings>();

        int i, sum = 0;
        for (i = 0; i < spawnDescriptors.Count; i++) {
            sum += spawnDescriptors[i].percent;
        }
        int target = (int)(sum * UnityEngine.Random.value);
        
        sum = 0;
        for (i = 0; i < spawnDescriptors.Count; i++) {
            sum += spawnDescriptors[i].percent;
            if (target < sum) {
                break;
            }
        }

        Container.BindInstance(spawnDescriptors[i].settings).AsTransient();
    }

    [Serializable]
    public struct EnemySpawnDescriptor {
        public EnemySpawnSettings settings;
        public int percent;
    }
}

public struct RebindEnemySpawnSettings {
}

public struct AttackSignal {
    public float damage;
}