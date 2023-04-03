using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParticleEffectPlay : MonoBehaviour
{
    public ParticleSystem effect;
    private void Awake() {
        effect.Play();
    } 
    private void Update() {
        if(effect.isStopped) {
            Destroy(gameObject);
        }
    }
}
