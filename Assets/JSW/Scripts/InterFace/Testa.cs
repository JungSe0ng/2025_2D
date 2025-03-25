using UnityEngine;

public class Testa : MonoBehaviour, IProduct
{
    public string codeName { get; set; }

    public void Initialize()
    {
        Debug.Log("a를 생성했습니다.");
    }
}
