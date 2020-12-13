using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{

    public float range = 5f;

    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (clickPos - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, layerMask);

            if (hit.collider != null)
            {
                if(hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<EnemyRPG>().Damage();
                }
                Debug.DrawLine(transform.position, hit.point, Color.red, 1f);
            }
            else
            {
                Vector3 temp = transform.position + (direction * range);

                Debug.DrawLine(transform.position, temp , Color.white, 2f);
            }
        }
    }
}
