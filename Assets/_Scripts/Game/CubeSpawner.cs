using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform _plane;

    private int _maxCubeCountInScene = 3;
    private int _destroyedCount = 0;

    private int _minSecondToSpawnNewCube = 1;
    private int _maxSecondToSpawnNewCube = 3;

    private static float _maxRandomizeValueForSpawnCube;

    [SerializeField] private TextMeshProUGUI _destroyedCountUI;

    private List<Cube> _allCubes = new List<Cube>();
    #endregion

    #region UnityMethods
    private void Start()
    {
        _maxRandomizeValueForSpawnCube = (_plane.transform.localScale.x * 5) - _cubePrefab.transform.localScale.x / 2;

        SpawnCubes();

        Bullet.OnBulletHitCube.AddListener(OnBulletDestroy);
    }
    #endregion

    #region Methods
    public void OnBulletDestroy(Cube cube)
    {
        _destroyedCount += 1;

        StartCoroutine(SpawnNewCube_Coroutine());

        _allCubes.Remove(cube);

        _destroyedCountUI.text = $"Destroyed: {_destroyedCount}";

        print(_allCubes.Count);
    }

    private void SpawnCubes()
    {
        for (int i = 0; i < _maxCubeCountInScene; i++)
        {
            SpawnCube();
        }
    }

    public void SpawnCube()
    {
        Cube cube = Instantiate(_cubePrefab, GetNewPos(), Quaternion.identity).GetComponent<Cube>();

        _allCubes.Add(cube);
    }

    public static Vector3 GetNewPos()
    {
        return new Vector3(Random.Range(-_maxRandomizeValueForSpawnCube, _maxRandomizeValueForSpawnCube), 0, Random.Range(-_maxRandomizeValueForSpawnCube, _maxRandomizeValueForSpawnCube));
    }
    #endregion

    #region Coroutines
    private IEnumerator SpawnNewCube_Coroutine()
    {
        yield return new WaitForSeconds(Random.Range(_minSecondToSpawnNewCube, _maxSecondToSpawnNewCube));

        if (_allCubes.Count < _maxCubeCountInScene)
            SpawnCube();
    }
    #endregion
}
