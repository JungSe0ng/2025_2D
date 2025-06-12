using UnityEngine;

public class SimplePlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A(-1), D(1)
        float v = Input.GetAxisRaw("Vertical");   // W(1), S(-1)

        Vector3 move = new Vector3(h, v, 0).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}