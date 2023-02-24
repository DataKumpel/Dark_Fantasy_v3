using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {
    public Light reveal_light;
    public float reveal_light_radius = 10f;
    public ParticleSystem flames;
    public Player owner;

    public void Start() {
        if(owner != null)
            owner.RegisterTorch(this);
        
        reveal_light.range = reveal_light_radius;
    }

    public void SwitchLight(bool val) {
        reveal_light.range = val ? reveal_light_radius : 0f;
        flames.gameObject.SetActive(val);
    }

    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(reveal_light.transform.position, reveal_light_radius);
    }
}
