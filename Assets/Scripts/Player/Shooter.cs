using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Shooter : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputMan.Firing) {
            Vector3 direction = _dir + new Vector3((Random.value-.5f)*variance, (Random.value-.5f)*variance, 0);
            direction = transform.rotation * direction;

            direction.Normalize();
            direction *= shootStr;
            
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
        }
        else {
            if(particleSystem.isPlaying) {
                particleSystem.Stop();
            }
        }

    }
}
