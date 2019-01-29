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
        Container.DeclareSignal<RebindEnemySpawnSettings>().OptionalSubscriber().RunAsync();

        Container.BindInstance(this);
        Container.BindSignal<RebindEnemySpawnSettings>().ToMethod(RebindSettings);
        Container.BindSignal<EndGameSignal>().ToMethod(DestroyAll);
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

    private void DestroyAll () {
        float delay = 0f;
        EnemyView[] enemies = GetComponentsInChildren<EnemyView>(true);
        foreach (EnemyView enemy in enemies) {
            Destroy(enemy, delay);
            delay += Time.deltaTime;
        }
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