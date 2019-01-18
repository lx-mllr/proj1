using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(LineRenderer))]
public class Shooter : MonoBehaviour
{
    LineRenderer _lineRenderer;
    Vector3 _dir;

    [Inject] readonly IInputManager _inputMan;

    [Range(0f, 0.25f)]
    public float variance;

    [Range(10, 30)]
    public float shootStr = 20f;

    [Range(1, 10)]
    public int shotsPerFrame = 1;

    public ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        _dir = Vector3.forward;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_inputMan.Firing) {
            _lineRenderer.enabled = true;
            List<Vector3> positions = new List<Vector3>();

            for (var i = 0; i < shotsPerFrame; i++) {
                Vector3 xyOffset = new Vector3((Random.value-.5f)*2*variance, (Random.value-.5f)*2*variance, 0);
                Vector3 target = (_dir + xyOffset) * shootStr;
                //target.y = 0;
                Vector3 direction = target - transform.position;
                direction = transform.rotation * direction;
                
                //int layer = LayerUtil.GetLayerFromPos(transform.position);
                Vector3 particlePos = transform.position + direction;

                Debug.DrawRay(transform.position, direction, Color.white, Time.deltaTime);
                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, shootStr)) {
                    if (hit.collider) {
                        particlePos = hit.point;
                        Instantiate(particleSystem, particlePos, Quaternion.identity, hit.collider.transform);

                        EnemyView enemy = hit.collider.GetComponent<EnemyView>();
                        if (enemy) {
                            enemy.TakeDamage();
                        }
                    }
                }
                positions.Add(transform.position);
                positions.Add(particlePos);
            }

            _lineRenderer.SetPositions(positions.ToArray());
        }
        else {
            _lineRenderer.enabled = false;
        }

    }
}
