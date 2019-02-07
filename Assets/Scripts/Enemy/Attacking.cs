using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyView))]
public class Attacking : MonoBehaviour
{
    private EnemyView _enemyView;

    void Start()
    {
        _enemyView = GetComponentInParent<EnemyView>();
    }

    public void DispatchAttackWrapper() {
        _enemyView.DispatchAttack();
    }
}
