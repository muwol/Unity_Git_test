using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    float speed = 8.0f;
    float lifeTime = 2.0f;

    private float spawnTime;
    private Vector2 moveDir;
    public Explosion explosionPrefab;

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        if (Time.time - spawnTime >= lifeTime)
        {
            ReturnPool();
        }
    }

    public void SetDir(Vector2 dir)
    {
        moveDir = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            ReturnPool();
        }

        if (collision.CompareTag("Enemy"))
        {
            Score score = FindAnyObjectByType<Score>();
            score.enemyCountUP();
            Destroy(collision.gameObject);
        }

        var fx = PoolManager.Instance.GetFromPool(explosionPrefab);

        if (fx != null)
        {
            fx.transform.position = transform.position;
            fx.PlayEffect();
        }
        ReturnPool();

    }

    void ReturnPool()
    {
        if (PoolManager.Instance!=null)
        {
            PoolManager.Instance.ReturnPool(this);
        }
    }
}
