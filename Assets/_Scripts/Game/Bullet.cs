using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    #region Variables
    [SerializeField] private float _radiusOfCheck = 1;

    public static UnityEvent<Cube> OnBulletHitCube = new UnityEvent<Cube>();

    [SerializeField] private LayerMask _layerMask;
    #endregion

    #region UnityMethods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject != null && collision.transform.gameObject.CompareTag("Plane"))
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _radiusOfCheck, Vector3.up, 1, _layerMask);

            if (hits != null && hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    Cube cube = hits[i].transform.parent.gameObject.GetComponent<Cube>();

                    if (cube != null)
                    {
                        OnBulletHitCube.Invoke(cube);
                    }

                    Destroy(hits[i].transform.parent.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radiusOfCheck);
    }
    #endregion
}
