using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public InputManager inputManager;

    private float speed = 5f;
    private Vector2 moveDir = Vector2.zero;

    private Rigidbody2D rigidbody;
    private Transform weaponPos;
    private Animator animator;

    private int dashHash = Animator.StringToHash("Dash");
    private int moveHash = Animator.StringToHash("Move");

    private IEnumerator dashCor;
    private bool isDash = false;
    private float dashSpeed = 7f;
    private float dashDuration = 0.3f;
    private float dashCoolTime = 3f;
    private float lastDashTime = 0f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        weaponPos = transform.GetChild(0).GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDash)
        {
            Move();
            Flip();
        }
        if (inputManager.IsMouseRight && Time.time - lastDashTime > dashCoolTime)
        {
            if (dashCor != null)
                StopCoroutine(dashCor);

            dashCor = CorDash(inputManager.MousePosVec - (Vector2)transform.position);
            StartCoroutine(dashCor);
        }
        WeponRotate();
    }

    private void Move()
    {
        if (inputManager.KeyboardVec != Vector2.zero && !animator.GetBool(dashHash))
            animator.SetBool(moveHash, true);
        else
            animator.SetBool(moveHash, false);

        moveDir = inputManager.KeyboardVec.normalized;
        rigidbody.linearVelocity = moveDir * speed;
    }

    private void Flip()
    {
        if (inputManager.MousePosVec.x - transform.position.x < 0)
            transform.localScale = new Vector2(-1f, 1f);
        else
            transform.localScale = Vector2.one;
    }
    private void WeponRotate()
    {
        Vector2 dir = inputManager.MousePosVec - (Vector2)weaponPos.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        weaponPos.rotation = Quaternion.Euler(0, 0, angle);

        if (transform.localScale.x == -1)
            weaponPos.localScale = new Vector2(-1f, -1f);
        else
            weaponPos.localScale = Vector2.one;
    }

    private IEnumerator CorDash(Vector2 direction)
    {
        animator.SetBool(dashHash, true);
        isDash = true;
        rigidbody.linearVelocity = Vector2.zero;
        lastDashTime = Time.time;

        yield return new WaitForSeconds(0.1f);

        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rigidbody.linearVelocity = direction * dashSpeed;
            yield return null; // 한 프레임 대기
        }

        rigidbody.linearVelocity = Vector2.zero;
        animator.SetBool(dashHash, false);
        isDash = false;
    }

}
