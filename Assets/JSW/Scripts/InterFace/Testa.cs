using UnityEngine;

public class Testa : MonoBehaviour, IProduct
{
    public string codeName { get; set; }

    public void Initialize()
    {
        Debug.Log("a�� �����߽��ϴ�.");
    }
}
