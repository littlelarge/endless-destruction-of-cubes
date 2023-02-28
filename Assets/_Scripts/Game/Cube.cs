using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    #region Variables
    [SerializeField] private float _speed = 2;

    private int _minSecondToJerkToNextPos = 3;
    private int _maxSecondToJerkToNextPos = 6;
    #endregion

    #region UnityMethods
    private void Start()
    {
        Move();
    }
    #endregion

    #region Methods
    private void Move()
    {
        StartCoroutine(Move_Coroutine());
    }

    private void WaitingForStart()
    {
        StartCoroutine(WaitingForStart_Coroutine());
    }
    #endregion

    #region Coroutines
    private IEnumerator Move_Coroutine()
    {
        Vector3 newPos = CubeSpawner.GetNewPos();

        while(transform.position != newPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, _speed * Time.deltaTime);

            yield return null;
        }

        WaitingForStart();
    }

    private IEnumerator WaitingForStart_Coroutine()
    {
        yield return new WaitForSeconds(Random.Range(_minSecondToJerkToNextPos, _maxSecondToJerkToNextPos));

        Move();
    }
    #endregion
}
