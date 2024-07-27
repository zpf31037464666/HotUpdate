using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    [Space]
    [Header("冲刺")]
    private float dashSpeed=20;
    private float dashPersistTime=.1f;
    private float dashCooldownTime=3f;
    private bool isUseDash = true;
    [HideInInspector] public bool isDashing = false;


    new private Rigidbody2D rigidbody;
    private Animator animator;
    private Joystick joystick;
    private Button dashButton;
    PlayerUI playerUI;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator=GetComponentInChildren<Animator>();
        playerUI=FindAnyObjectByType<PlayerUI>();
        joystick=playerUI.joystick;
        dashButton=playerUI.dashButton;


       dashButton.onClick.AddListener(Dash);
    }
    void Update()
    {
        Move();
        SwitchAnimation();
        Fild();
    }
    void Fild()
    {
        if (joystick.Direction.x>0.1)
        {
            animator. transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (joystick.Direction.x<-0.1)
        {
            animator.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
    void Move()
    {  
        rigidbody.velocity = joystick.Direction * speed;
    }
    void SwitchAnimation()
    {
        if (joystick.Direction != Vector2.zero)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }
    void Dash()
    {
        if (isUseDash)
        {
            Debug.Log("开始冲刺");

            if (joystick.Direction!=Vector2.zero)
            {
                StartCoroutine(DashCoroutine(joystick.Direction));
            }
            else
            {
                var dirction =animator.transform.rotation.y==0?Vector2.right:Vector2.left;

                StartCoroutine(DashCoroutine(dirction.normalized));
            }
            StartCoroutine(nameof(DashCooldTimeCoroutine));
        }
    }
    IEnumerator DashCoroutine(Vector2 dirction)
    {
        isDashing= true;
        var time = dashPersistTime;
        while (time>0)
        {
            rigidbody.velocity = dirction * dashSpeed;
            // dashShadow.CreateShadow();
            time -= Time.deltaTime;
            yield return null;
        }
        rigidbody.velocity = Vector2.zero; // 结束冲刺时将速度归零
        isDashing = false;
    }

    IEnumerator DashCooldTimeCoroutine()
    {
        isUseDash = false;
        int layer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = layer;

        var time = 0f;
        while (time<=dashCooldownTime)
        {
            playerUI.SetDashImageFill(time/dashCooldownTime);
            time += Time.deltaTime;
            yield return null;
        }
        isUseDash = true;

        layer = LayerMask.NameToLayer("Player");
        gameObject.layer = layer;
    }

 
}
