using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    
    [Header("Player")]
    [SerializeField] private int health = 200;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 1f;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private float durationOfExplosion = 1f;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] [Range(0,1)] private float deathVolume = 0.75f;
    
    [Header("Projectile")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileFiringPeriod = 0.1f;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] [Range(0,1)] private float shootVolume = 0.75f;

    private float _xMin, _xMax;
    private float _yMin, _yMax;

    private Coroutine _firingCoroutine;

    private void Start() {
        SetUpMoveBoundaries();
    }
    
    private void Update() {
        Move();
        Fire();
    }

    private void Move() {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, _xMin, _xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, _yMin, _yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }
    
    private void Fire() {
        if (Input.GetButtonDown("Fire1")) {
            _firingCoroutine = StartCoroutine(FireContinuously());
        }

        if (Input.GetButtonUp("Fire1")) {
            StopCoroutine(_firingCoroutine);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator FireContinuously() {
        while (true) {
            var laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) return;
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        var explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathVolume);
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
    }

    public int GetHealth() {
        return health;
    }

    private void SetUpMoveBoundaries() {
        var gameCamera = Camera.main;
        
        _xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        _xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        _yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        _yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
