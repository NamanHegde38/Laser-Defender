using System;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] private GameObject hitVFX;
    [SerializeField] private float durationOfExplosion = 1f;
    
    private DamageDealer damageDealer;
    
    private void Start() {
        damageDealer = GetComponent<DamageDealer>();
        damageDealer.OnProjectileHit += HitProjectile;
    }

    private void HitProjectile(object sender, EventArgs e) {
        if (!hitVFX) return;
        var particles = Instantiate(hitVFX, transform.position, Quaternion.identity);
        Destroy(particles, durationOfExplosion);
        damageDealer.OnProjectileHit -= HitProjectile;
    }
}
