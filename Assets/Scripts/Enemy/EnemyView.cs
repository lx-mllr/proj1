using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Marching))]
public class EnemyView : MonoBehaviour
{
    public List<ParticleSystem> onHitSystems;

    [Inject] readonly EnemySpawnSettings _settings;
    [Inject] readonly SignalBus _signalBus;
    
    public static float spawnDistance = 0.6f;
    private static float spawnOffset = 0f;
    
    public Animator animator { get; private set; }
    private Marching _marching;
    private float hp;
    private bool _idle;
    private List<Collider> _trackedColliders;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnOffset == 0f) {
            MeshCollider _ground = GameObject.FindWithTag("Ground").GetComponent<MeshCollider>();
            spawnOffset = (_ground.bounds.extents.magnitude * spawnDistance);
        }
        
        hp = _settings.HP;
        _marching = GetComponentInChildren<Marching>();
        animator = GetComponentInChildren<Animator>();
        _idle = false;
        _trackedColliders = new List<Collider>();

        float x = (Random.value * 2f) - 1f;
        float z = (Random.value * 2f) - 1f;
        Vector3 startPos = new Vector3(x, 0, z);
        startPos = startPos.normalized * spawnOffset;
        transform.position = startPos;
        transform.LookAt(Vector3.zero);

        gameObject.layer = LayerUtil.GetLayerFromPos(transform.position);
    }

    void LateUpdate () {
        float speed = _marching.MoveSpeed * _settings.MoveSpeed;
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    void OnTriggerEnter (Collider other) {
        BaseView baseView = other.GetComponentInParent<BaseView>();

        if (baseView) {
            animator.SetBool("InAttackRange", true);            // there has to be a better way to do this..
        }
    }

    void OnCollisionEnter (Collision collision) {
        Collider oCollider = collision.collider;
        EnemyView oEnemyView = oCollider.GetComponent<EnemyView>(); 

        if (oEnemyView && _trackedColliders.IndexOf(oCollider) < 0) {
            if (oEnemyView.animator.GetCurrentAnimatorStateInfo(0).fullPathHash  == animator.GetCurrentAnimatorStateInfo(0).fullPathHash) {
                _trackedColliders.Add(oCollider);
                
                _idle = !(oEnemyView.animator.GetBool("Idle"));
                animator.SetBool("Idle", _idle);
            }
        }
    }

    void OnCollisionExit (Collision collision) {
        if (!_idle) { return; }

        Collider other = collision.collider;
        int index = _trackedColliders.IndexOf(other);
        if (index >= 0) {
            _trackedColliders.RemoveAt(index);
        }

        _idle = _trackedColliders.Count == 0 ? false : true;
        animator.SetBool("Idle", _idle);
    }

    public void ProcessHit (RaycastHit hit) {
        for (var i = 0; i < onHitSystems.Count; i++) {
            Instantiate(onHitSystems[i], hit.point, onHitSystems[i].transform.rotation);
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
