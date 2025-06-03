using UnityEngine;

public class BossMissile : MonoBehaviour, IBullet
{
    public float speed = 10f;
    public float rotateSpeed = 200f;
    public float lifeTime = 5f;
    public int damage = 20;
    private float timer = 0f;
    private Transform target;
    private Vector3 shootDirection = Vector3.right;
    private bool isTracking = false;

    public void Shoot(Vector3 direction)
    {
        shootDirection = direction.normalized;
        isTracking = false;
    }

    public void Fire(Transform targetTransform)
    {
        target = targetTransform;
        isTracking = true;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        if (isTracking && target != null)
        {
            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.right).z;
            transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(shootDirection * speed * Time.deltaTime);
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
        // 폭발 이펙트 등 추가 가능
        Destroy(gameObject);
    }
} 