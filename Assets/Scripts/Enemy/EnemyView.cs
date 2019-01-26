using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyView : MonoBehaviour
{
    public List<ParticleSystem> onHitSystems;

    [Inject] readonly EnemySpawnSettings _settings;
    [Inject] readonly SignalBus _signalBus;
    
    public static float spawnDistance = 0.6f;
    private static float spawnOffset = 0f;
    
    private Animator _animator;
    private Marching _marching;
    private float hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = _settings.HP;
        _marching = GetComponentInChildren<Marching>();
        _animator = GetComponentInChildren<Animator>();
        
        if (spawnOffset == 0f) {
            MeshCollider _ground = GameObject.FindWithTag("Ground").GetComponent<MeshCollider>();
            spawnOffset = (_ground.bounds.extents.magnitude * spawnDistance);
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
            float speed = _marching.MoveSpeed * _settings.MoveSpeed;
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter (Collider other) {
        BaseView baseView = other.GetComponentInParent<BaseView>();
        if (baseView) {
            // there has to be a better way to do this..
            _animator.SetBool("InAttackRange", true);
        }

    }

    public void ProcessHit (RaycastHit hit) {
        
        for (var i = 0; i < onHitSystems.length; i++) {
            Instantiate(onHitSystems[i], rayEnd, particleSystem.transform.rotation);
        }

        hp--;
        if (hp <= 0) {
            Destroy(gameObject);
            _signalBus.Fire(new AddScoreSignal() {
                val = _settings.score
            });
        }
    }

    public void DispatchAttack () {
        _signalBus.Fire(new AttackSignal () {
                damage = _settings.AttackStr
        });
    }

    public class Factory : PlaceholderFactory<EnemyView> {
    }
}
