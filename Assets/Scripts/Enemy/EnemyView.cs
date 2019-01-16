using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyView : MonoBehaviour
{
    [Inject] readonly EnemySpawnSettings _settings;
    

    private static float spawnOffset = 0f;
    
    private Marching _marching;
    private float hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = _settings.HP;
        _marching = GetComponentInChildren<Marching>();
        
        if (spawnOffset == 0f) {
            MeshCollider _ground = GameObject.FindWithTag("Ground").GetComponent<MeshCollider>();
            spawnOffset = (_ground.bounds.extents.magnitude * .7f);
        }

        float x = (Random.value * 2) - 1;
        float z = (Random.value * 2) - 1;
        Vector3 startPos = new Vector3(x, 0, z);
        startPos = startPos.normalized * spawnOffset;
        transform.position = startPos;
        transform.LookAt(Vector3.zero);

        gameObject.layer = LayerUtil.GetLayerFromPos(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_marching) {
            transform.position += transform.forward * (_marching.MoveSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage () {
        hp--;
        if (hp <= 0) {
            Destroy(gameObject);
        }
    }

    public class Factory : PlaceholderFactory<EnemyView> {
    }
}
