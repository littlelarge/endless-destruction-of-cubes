using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletSpawner : MonoBehaviour
{
    #region Variables
    public Transform SpawnTransform;
    public float AngleInDegrees;
    public GameObject Bullet;

    private Vector3 _targetPosition;
    private float _g = Physics.gravity.y;

    private int _maxBulletsCount = 5;
    private int _currentBulletsCount = 5;
    private float _timeBetweenShots = 0.5f;
    private float _timeToAddNewBullet = 1;

    private bool _canShot = true;

    [SerializeField] private GameObject _uiBulletPrefab;
    [SerializeField] private Transform _bulletGroupRoot;
    [SerializeField] private TextMeshProUGUI _timerUI;
    #endregion

    #region UnityMethods
    private void Start()
    {
        StartCoroutine(Shot_Coroutine());
        StartCoroutine(BulletReplenishment_Coroutine());

        CreateUIBullets();
    }

    private void Update()
    {
        SpawnTransform.localEulerAngles = new Vector3(-AngleInDegrees, 0f, 0f);
    }

    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Gizmos.DrawRay(ray);
    }
    #endregion

    #region Methods
    public void BallisticShot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            _targetPosition = hit.point;
        }

        Vector3 fromTo = _targetPosition - transform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

        SpawnTransform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);

        float x = fromToXZ.magnitude;
        float y = fromTo.y;

        float AngleInRadians = AngleInDegrees * Mathf.PI / 180;

        float v2 = (_g * x * x) / (2 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));

        GameObject newBullet = Instantiate(Bullet, SpawnTransform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = SpawnTransform.forward * v;

        _canShot = false;

        StartCoroutine(DestroyAfter_Coroutine(3, newBullet));
        StartCoroutine(Reloading_Coroutine());

        _currentBulletsCount -= 1;
        Destroy(_bulletGroupRoot.GetChild(_bulletGroupRoot.childCount - 1).gameObject);
    }

    private void CreateUIBullets()
    {
        for (int i = 0; i < _maxBulletsCount; i++)
        {
            Instantiate(_uiBulletPrefab, _bulletGroupRoot);
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator DestroyAfter_Coroutine(float seconds, GameObject gameObject)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }

    private IEnumerator Shot_Coroutine()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0) && _canShot && _currentBulletsCount > 0)
            {
                BallisticShot();
            }

            yield return null;
        }
    }

    private IEnumerator Reloading_Coroutine()
    {
        yield return new WaitForSeconds(_timeBetweenShots);

        _canShot = true;
    }

    private IEnumerator BulletReplenishment_Coroutine()
    {
        while (true)
        {
            if (_currentBulletsCount < _maxBulletsCount)
            {
                for (float _timer = _timeToAddNewBullet; _timer > 0; _timer -= Time.deltaTime)
                {
                    _timerUI.text = _timer.ToString("0.0");

                    yield return null;
                }

                _currentBulletsCount += 1;
                Instantiate(_uiBulletPrefab, _bulletGroupRoot);
            }

            yield return null;
        }
    }
    #endregion
}
