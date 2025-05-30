using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Mouse
    public Vector2 MouseVec { get; private set; }
    public Vector2 MousePosVec { get; private set; }
    public bool IsMouseLeft { get; private set; }
    public bool IsMouseRight { get; private set; }

    // Keyboard
    public Vector2 KeyboardVec { get; private set; }


    private float mouseX = 0f;
    private float mouseY = 0f;

    private float horInput = 0f;
    private float verInput = 0f;

    void Start()
    {

    }

    void Update()
    {
        MouseInput();
        KeyboardInput();
    }

    private void MouseInput()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        MouseVec = new Vector2(mouseX,mouseY);

        IsMouseLeft = Input.GetMouseButton(0);
        IsMouseRight = Input.GetMouseButton(1);

        MousePosVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void KeyboardInput()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");

        KeyboardVec = new Vector2(horInput, verInput);
    }
}
