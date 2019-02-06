using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Marching))]
public class EnemyView : MonoBehaviour
{
    public static float spawnDistance = 0.6f;
    private static float spawnOffset = 0f;
    
    public List<ParticleSystem> onDestroySystems;

    public Animator animator { get; private set; }
    private Marching _marching;
    private Renderer _renderer;
    private Collider _collider;
    private int hp;
    private bool _idle;
    private List<Collider> _trackedColliders;

    EnemySpawnSettings _settings;
    SignalBus _signalBus;
    float _hpStartRatio;

    [Inject] 
    public void Construct (float ratio, EnemySpawnSettings settings, SignalBus signalBus) {
        _settings = settings;
        _signalBus = signalBus;
        _hpStartRatio = ratio;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (spawnOffset == 0f) {
            MeshCollider _ground = GameObject.FindWithTag("Ground").GetComponent<MeshCollider>();
            spawnOffset = (_ground.bounds.extents.magnitude * spawnDistance);
        }
        
        _hpStartRatio = Mathf.Max(0.1f, _hpStartRatio);
        hp = (int)Mathf.Ceil(_settings.HP * _hpStartRatio);
        _idle = false;

        _renderer = GetComponentInChildren<Renderer>();
        _marching = GetComponentInChildren<Marching>();
        _collider = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();
        
        _trackedColliders = new List<Collider>();

        float x = (Random.value * 2f) - 1f;
        float z = (Random.value * 2f) - 1f;
        Vector3 startPos = new Vector3(x, 0, z);
        startPos = startPos.normalized * spawnOffset;
        if (Mathf.Floor(startPos.x) == 0) {
            startPos.x = 1;
        }
        if (Mathf.Floor(startPos.z) == 0) {
            startPos.z = 1;
        }

        transform.position = startPos;
        transform.LookAt(Vector3.zero);

        gameObject.layer = LayerUtil.GetLayerFromPos(transform.position);
    }

    void LateUpdate () {
        if (hp > 0) {
            float speed = _marching.MoveSpeed * _settings.MoveSpeed;
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        _renderer.material.SetFloat("_HPRatio", (hp / (float)_settings.HP));
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
        float maxDelay = 0;

        hp--;
        if (hp == 0) {
            for (int i = 0; i < onDestroySystems.Count; i++) {
                Instantiate(onDestroySystems[i], hit.point, transform.rotation * onDestroySystems[i].transform.rotation, transform);
                float dur = onDestroySystems[i].main.startLifetime.constantMax + onDestroySystems[i].main.duration;
                maxDelay = Mathf.Max(dur, maxDelay);
            }

            _renderer.enabled = false;
            _collider.enabled = false;
            Destroy(gameObject, maxDelay);

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

    public class Factory : PlaceholderFactory<float, EnemyView> {
        [Inject] public EnemySpawnInstaller spawnParent;
    }
}
