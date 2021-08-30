using UnityEngine;
using MoreMountains.Feedbacks;

public class Enemy : MonoBehaviour {

    [Header("Enemy Stats")]
    [SerializeField] private float health = 100;
    [SerializeField] private int scoreValue = 150;
    
    [Header("Shooting")]
    [SerializeField] private float minTimeBetweenShots = 0.2f;
    [SerializeField] private float maxTimeBetweenShots = 3f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed = 10f;

    [Header("Effects")]
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private GameObject deathVFX2;
    [SerializeField] private float durationOfExplosion = 1f;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] [Range(0,1)] private float shootVolume = 0.75f;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] [Range(0,1)] private float deathVolume = 0.75f;

    [SerializeField] private MMFeedbacks hitFeedback;
    [SerializeField] private MMFeedbacks deathFeedback;

    private GameSession _gameSession;
    private float _shotCounter;
    
    private void Start() {
        _gameSession = FindObjectOfType<GameSession>();
        _shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void Update() {
        CountDownAndShoot();
    }

    private void CountDownAndShoot() {
        _shotCounter -= Time.deltaTime;
        if (!(_shotCounter <= 0f)) return;
        Fire();
        _shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Fire() {
        var laser = Instantiate(projectile, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootVolume);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) return;
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        hitFeedback?.PlayFeedbacks();
        var particles = Instantiate(hitVFX, transform.position, transform.rotation, transform);
        Destroy(particles, durationOfExplosion);
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        _gameSession.AddToScore(scoreValue);
        var explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        if (deathVFX2) {
            var secondaryExplosion = Instantiate(deathVFX2, transform.position, transform.rotation);
            Destroy(secondaryExplosion, durationOfExplosion);
        }

        deathFeedback?.PlayFeedbacks();
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathVolume);
        Destroy(gameObject);
    }
}
