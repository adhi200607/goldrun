using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Camera mainCam;

    private float rotationVelocity;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 1.82f;
    [SerializeField] private float runSpeed = 4.28f;
    [SerializeField] private float rotationSmoothTime = 0.08f;

    [Header("Key Binds")]
    public InputAction moveAction;
    public InputAction runAction;
    public InputAction jumpAction;

    [Header("Gravity & Jump")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float terminalVelocity = -50f;
    [SerializeField] private float groundedForce = -2f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask jumpableLayer;
    [SerializeField] private float groundCheckDistance = 0.25f;
    [SerializeField] private Transform groundCheckPoint;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [SerializeField] private float bombDamage = 20f;

    [Header("Health Bar")]
    [SerializeField] private Image healthFG;

    [SerializeField] private float emptyFillAmount = 0f;
    [SerializeField] private float fullFillAmount = 1f;

    [Header("Bomb Blast Effect")]
    [SerializeField] private GameObject enemyBlastEffect;

    public float CurrentHealth => currentHealth;


    // =====================================================
    // AWAKE
    // =====================================================

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        mainCam = Camera.main;
    }


    // =====================================================
    // ENABLE
    // =====================================================

    private void OnEnable()
    {
        moveAction.Enable();
        runAction.Enable();
        jumpAction.Enable();
    }


    private void OnDisable()
    {
        moveAction.Disable();
        runAction.Disable();
        jumpAction.Disable();
    }


    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        currentHealth = maxHealth;

        UpdateHealthBar();

        Debug.Log("PLAYER HEALTH STARTED: " + currentHealth);
    }


    // =====================================================
    // UPDATE
    // =====================================================

    private void Update()
    {
        HandleMovement();
        HandleJumpAndGravity();
        UpdateAnimations();
    }


    // =====================================================
    // MOVEMENT
    // =====================================================

    private void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 inputDirection =
            new Vector3(input.x, 0f, input.y);

        if (inputDirection.sqrMagnitude > 1f)
        {
            inputDirection.Normalize();
        }

        bool isMoving =
            inputDirection.sqrMagnitude > 0.0001f;

        bool isRunning =
            isMoving && runAction.IsPressed();

        float currentSpeed =
            isRunning ? runSpeed : walkSpeed;


        if (isMoving && mainCam != null)
        {
            Vector3 cameraForward =
                mainCam.transform.forward;

            Vector3 cameraRight =
                mainCam.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection =
                cameraForward * inputDirection.z +
                cameraRight * inputDirection.x;

            moveDirection.Normalize();

            float targetAngle =
                Mathf.Atan2(
                    moveDirection.x,
                    moveDirection.z
                ) * Mathf.Rad2Deg;

            float smoothAngle =
                Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    targetAngle,
                    ref rotationVelocity,
                    rotationSmoothTime
                );

            transform.rotation =
                Quaternion.Euler(
                    0f,
                    smoothAngle,
                    0f
                );

            controller.Move(
                moveDirection *
                currentSpeed *
                Time.deltaTime
            );
        }
    }


    // =====================================================
    // JUMP + GRAVITY
    // =====================================================

    private void HandleJumpAndGravity()
    {
        if (groundCheckPoint != null)
        {
            isGrounded = Physics.CheckSphere(
                groundCheckPoint.position,
                groundCheckDistance,
                jumpableLayer,
                QueryTriggerInteraction.Ignore
            );
        }
        else
        {
            isGrounded = controller.isGrounded;
        }


        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = groundedForce;
        }


        if (isGrounded &&
            jumpAction.WasPressedThisFrame())
        {
            velocity.y =
                Mathf.Sqrt(
                    jumpHeight *
                    -2f *
                    gravity
                );

            if (animator != null)
            {
                animator.ResetTrigger("Jump");
                animator.SetTrigger("Jump");
            }
        }


        if (!isGrounded)
        {
            velocity.y +=
                gravity *
                Time.deltaTime;

            velocity.y =
                Mathf.Max(
                    velocity.y,
                    terminalVelocity
                );
        }


        controller.Move(
            Vector3.up *
            velocity.y *
            Time.deltaTime
        );
    }


    // =====================================================
    // ANIMATION
    // =====================================================

    private void UpdateAnimations()
    {
        if (animator == null)
        {
            return;
        }

        Vector2 input =
            moveAction.ReadValue<Vector2>();

        float magnitude =
            Mathf.Clamp01(input.magnitude);

        float speedValue = 0f;

        if (magnitude > 0.01f)
        {
            speedValue =
                runAction.IsPressed()
                ? runSpeed
                : walkSpeed;
        }

        animator.SetFloat(
            "Speed",
            speedValue,
            0.1f,
            Time.deltaTime
        );
    }


    // =====================================================
    // BOMB COLLISION
    // =====================================================

    private void OnControllerColliderHit(
        ControllerColliderHit hit)
    {
        Debug.Log(
            "COLLISION WITH: " +
            hit.gameObject.name
        );


        // Check the object that was actually hit
        if (hit.gameObject.CompareTag("Enemy"))
        {
            HitBomb(hit.gameObject);
            return;
        }


        // Sometimes the collider is on a child object
        if (hit.transform.root.CompareTag("Enemy"))
        {
            HitBomb(hit.transform.root.gameObject);
            return;
        }
    }


    // =====================================================
    // HIT BOMB
    // =====================================================

    private void HitBomb(GameObject bomb)
    {
        Debug.Log("💥 BOMB HIT!");

        // DAMAGE
        TakeDamage(bombDamage);


        // BLAST EFFECT
        if (enemyBlastEffect != null)
        {
            GameObject blast =
                Instantiate(
                    enemyBlastEffect,
                    bomb.transform.position,
                    Quaternion.identity
                );

            Destroy(blast, 2f);
        }


        // DESTROY BOMB
        Destroy(bomb);


        Debug.Log(
            "Bomb destroyed. Current Health = " +
            currentHealth
        );
    }


    // =====================================================
    // TAKE DAMAGE
    // =====================================================

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0f,
                maxHealth
            );


        Debug.Log(
            "❤️ HEALTH: " +
            currentHealth
        );


        // UPDATE UI
        UpdateHealthBar();


        // DEAD
        if (currentHealth <= 0f)
        {
            PlayerDied();
        }
    }


    // =====================================================
    // HEALTH BAR
    // =====================================================

    private void UpdateHealthBar()
    {
        if (healthFG == null)
        {
            Debug.LogWarning(
                "⚠️ HEALTH BAR IMAGE IS NOT ASSIGNED!"
            );

            return;
        }


        float healthPercentage =
            currentHealth / maxHealth;


        healthFG.fillAmount =
            healthPercentage;


        Debug.Log(
            "Health Bar Fill = " +
            healthFG.fillAmount
        );
    }


    // =====================================================
    // PLAYER DIED
    // =====================================================

    private void PlayerDied()
    {
        Debug.Log("💀 PLAYER DIED!");

        moveAction.Disable();
        runAction.Disable();
        jumpAction.Disable();


        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            Debug.LogError(
                "GameManager.Instance is NULL!"
            );
        }
    }


    // =====================================================
    // GIZMOS
    // =====================================================

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null)
        {
            return;
        }

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            groundCheckPoint.position,
            groundCheckDistance
        );
    }
}