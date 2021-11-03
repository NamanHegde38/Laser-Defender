using System;
using UnityEngine;

public class DamageDealer : MonoBehaviour {

    [SerializeField] private int damage = 100;
    public event EventHandler OnProjectileHit;

    public int GetDamage() {
        return damage;
    }

    public void Hit() {
        OnProjectileHit?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
}
