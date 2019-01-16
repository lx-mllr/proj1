using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

public class EnemySpawnInstaller : MonoInstaller
{
    public List<EnemySpawnDescriptor> spawnDescriptors;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<RebindEnemySpawnSettings>().OptionalSubscriber();

        Container.BindSignal<RebindEnemySpawnSettings>().ToMethod(RebindSettings);
        RebindSettings();
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