using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private CollisionDetector _collisionDetector;
    private Rigidbody _rigidbody;
    private Renderer _renderer;

    private float _minLifeTime = 2f;
    private float _maxLifeTime = 5f;
    private bool _detonationStarted = false;
    private bool _isColorSwitched = false;

    private float _lifeTime;

    public Rigidbody Rigidbody => _rigidbody;

    public event Action<Cube> LifeTimeEnded;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _collisionDetector = GetComponent<CollisionDetector>();
    }

    private void OnEnable()
    {
        GenerateLifeTime();
        _collisionDetector.CollisionDetected += HandleCollision;
    }

    private void OnDisable()
    {
        _collisionDetector.CollisionDetected -= HandleCollision;
    }

    private void Update()
    {
        if (_detonationStarted)
        {
            _lifeTime -= Time.deltaTime;
        }
        if (_lifeTime <= 0)
        {
            LifeTimeEnded?.Invoke(this);
        }
    }

    private void HandleCollision()
    {
        _detonationStarted = true;

        if (_isColorSwitched == false)
        {
            _renderer.material.color = Random.ColorHSV();
            _isColorSwitched = true;
        }
    }

    private void GenerateLifeTime()
    {
        _lifeTime = Random.Range(_minLifeTime, _maxLifeTime + 1);
    }
}
