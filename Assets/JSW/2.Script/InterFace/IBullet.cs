using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    public void Shoot(Vector3 direction);
    public void Fire(Transform target);

    //총알을 꺼내온다 -> 위치랑 회전값 변경 부모 위치 큐에서 빼오기
    public IBullet OutBullet(Transform parent, Vector3 position, Quaternion rotation);

    //총알을 넣는다 -> 위치랑 회전값 변경 부모 위치, 큐에 다시 넣기
    public IBullet InBullet(Transform parent);
}