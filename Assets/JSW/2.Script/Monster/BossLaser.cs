using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossLaser : MonoBehaviour, IBullet
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public int damage = 10;
    private float timer = 0f;
    private Vector3 shootDirection = Vector3.right;

    public void Shoot(Vector3 direction)
    {
        shootDirection = direction.normalized;
    }

    public void Fire(Transform target)
    {
        // 레이저는 타겟 추적 없음, 무시
    }

    void Update()
    {
        transform.Translate(shootDirection * speed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 또는 타겟에 데미지 적용
        if (collision.CompareTag("Player"))
        {
            // 예시: PlayerHealth player = collision.GetComponent<PlayerHealth>();
            // if (player != null) player.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    //위치 최기화를 하고 부모 위치에 들어간다. array에 들어간다.

    public IBullet InBullet(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = quaternion.identity;
        gameObject.SetActive(false);
        return this;
    }
    

    public IBullet OutBullet(Transform parent, Vector3 position, Quaternion rotation)
    {
        transform.SetParent(parent);
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
        return this;
    }
} 