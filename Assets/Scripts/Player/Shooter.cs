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
            Vector3 xyOffset = new Vector3((Random.value-.5f)*variance, (Random.value-.5f)*variance, 0);
            Vector3 target = (_dir + xyOffset) * shootStr;
            target.y = 0;
            Vector3 direction = target - transform.position;
            direction = transform.rotation * direction;
            
            int layer = LayerUtil.GetLayerFromPos(transform.position);
            Vector3 particlePos = transform.position + direction;
            Debug.Log("Shooting into quad: " + LayerMask.LayerToName(layer));

            Debug.DrawRay(transform.position, direction, Color.white, Time.deltaTime);
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, shootStr, layer)) {
                if (hit.collider) {
                    particlePos = hit.point;
                    EnemyView enemy = hit.collider.GetComponentInParent<EnemyView>();
                    if (enemy) {
                        enemy.TakeDamage();
                    }
                }
            }

            if (particleSystem.isStopped) {
                particleSystem.Play();
            }
            particleSystem.transform.position = particlePos;

            Vector3[] positions = { transform.position, particlePos };
            _lineRenderer.SetPositions(positions);
        }
        else {
            _lineRenderer.enabled = false;
            if(particleSystem.isPlaying) {
                particleSystem.Stop();
            }
        }

    }
}
