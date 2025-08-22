using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;

    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;

    [SerializeField] private float _repeatRate = 3;

    [SerializeField] private Transform _transform;

    private float _minRange = -7;
    private float _maxRange = 7;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Create(),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0, _repeatRate);
    }

    private void GetCube()
    {
        Cube cube = _pool.Get();

        cube.LifeTimeEnded += ReleaseObject;
    }

    private Cube Create()
    {
        Cube cube = Instantiate(_prefab);

        return cube;
    }

    private void ReleaseObject(Cube cube)
    {
        cube.LifeTimeEnded -= ReleaseObject;
        _pool.Release(cube);
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = GetRandomPoint(_transform);
        cube.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.gameObject.SetActive(true);
    }

    private void Destroy(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private Vector3 GetRandomPoint(Transform transform)
    {
        float xPosition = Random.Range(_minRange, _maxRange + 1) + transform.position.x;
        float yPosition = transform.position.y;
        float zPosition = Random.Range(_minRange, _maxRange + 1) + transform.position.z;

        return new Vector3(xPosition, yPosition, zPosition);
    }
}