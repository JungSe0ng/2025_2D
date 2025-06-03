using UnityEngine;

public interface IBullet
{
    void Shoot(Vector3 direction);
    void Fire(Transform target);
} 