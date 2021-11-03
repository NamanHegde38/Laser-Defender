using System.Collections;
using MoreMountains.Feedbacks;
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
    [SerializeField] private Transform gunPos;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileFiringPeriod = 0.1f;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] [Range(0,1)] private float shootVolume = 0.75f;

    [Header("Overheat")]
    [SerializeField] private float initialshootHeat;
    [SerializeField] private float shootHeatPercent;
    [SerializeField] private float heatDecreaseRate;
    [SerializeField] private float startTimeTillCooling;
    [SerializeField] private float maxHeat;
    
    [Header("Effects")]
    [SerializeField] private GameObject shootVFX;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private AudioClip[] hitSound;
    [SerializeField] [Range(0, 1)] private float hitVolume;
    [SerializeField] private AudioClip overheatSound;
    [SerializeField] [Range(0, 1)]private float overheatVolume;
    [SerializeField] private MMFeedbacks shootFeedback;
    [SerializeField] private MMFeedbacks hitFeedback;
    [SerializeField] private MMFeedbacks deathFeedback;

    private float _xMin, _xMax;
    private float _yMin, _yMax;

    private Coroutine _firingCoroutine;
    private float _heat = 0f;
    private bool _canShoot = true;
    private bool _canCool = true;
    private AudioListener _audioListener;
    private float timeTillCooling;

    private void Start() {
        SetUpMoveBoundaries();
        _audioListener = FindObjectOfType<AudioListener>();
    }
    
    private void Update() {
        DecreaseHeat();
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
            if (_canShoot) {
                _firingCoroutine = StartCoroutine(FireContinuously());
            }
        }

        if (Input.GetButtonUp("Fire1") || !_canShoot) {
            StopCoroutine(_firingCoroutine);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator FireContinuously() {
        while (true) {
            var laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, _audioListener.transform.position, shootVolume);

            var particles = Instantiate(shootVFX, gunPos.position, Quaternion.identity, transform);
            Destroy(particles, durationOfExplosion);
            shootFeedback?.PlayFeedbacks();
            AddHeat();
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void DecreaseHeat() {
        if (_canCool) {
            _heat = Mathf.Clamp(_heat, 0f, maxHeat + 1f);
            _heat -= heatDecreaseRate * Time.deltaTime;
        }

        if (_heat >= maxHeat) {
            AudioSource.PlayClipAtPoint(overheatSound, _audioListener.transform.position, overheatVolume);
            _canShoot = false;
        }
        else if (_heat <= 0) {
            _canShoot = true;
        }

        if (!_canCool) {
            if (timeTillCooling <= 0) {
                _canCool = true;
                timeTillCooling = startTimeTillCooling;
            }
            else {
                timeTillCooling -= Time.deltaTime;
            }
        }
    }
    
    private void AddHeat() {
        _canCool = false;

        if (shootHeatPercent > 0) {
            if (_heat <= 0) {
                _heat += initialshootHeat;
            }
            else {
                var percentToAdd = shootHeatPercent / 100 * _heat;
                _heat += percentToAdd;
            }
        }
        else {
            _heat += initialshootHeat;
        }
        
    }

    public float GetHeat() {
        return _heat;
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) return;
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        if (hitVFX) {
            var particles = Instantiate(hitVFX, transform.position, Quaternion.identity, transform);
            Destroy(particles, durationOfExplosion);
        }
        
        var sound = hitSound[Random.Range(0, hitSound.Length)];
        AudioSource.PlayClipAtPoint(sound, _audioListener.transform.position, hitVolume);
        
        hitFeedback?.PlayFeedbacks();
        damageDealer.Hit();
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        var explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        deathFeedback?.PlayFeedbacks();
        AudioSource.PlayClipAtPoint(deathSound, _audioListener.transform.position, deathVolume);
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
