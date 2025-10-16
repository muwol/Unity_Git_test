using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    bool isLeft = true;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ENDPOINT"))
        {
            if (isLeft)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                isLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isLeft = true;
            }
        }
    }
}
