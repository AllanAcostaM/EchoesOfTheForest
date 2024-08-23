using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;
    public float crouchHeight = 0.5f;
    public float standingHeight = 2f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isCrouching = false;

    private Camera playerCamera;
    private float cameraPitch = 0f;


    public static PlayerController Instance { get; private set; }

    void Awake()
    {
        // Asegurarse de que sólo haya una instancia del PlayerController
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Obtener el CharacterController y la cámara del jugador
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Llamar funciones de movimiento y control de la cámara
        MovePlayer();
        ControlCamera();
    }

    private void MovePlayer()
    {
        // Comprobar si el jugador está en el suelo
        isGrounded = controller.isGrounded;

        // Obtener input de movimiento horizontal y vertical (WASD)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Determinar la velocidad según si el jugador está corriendo o agachado
        float currentSpeed = isCrouching ? crouchSpeed : (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);

        // Movimiento del jugador relativo a su orientación
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Saltar si el jugador está en el suelo y presiona Espacio
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        // Aplicar gravedad
        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Si está en el suelo, reiniciar la velocidad vertical
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        // Alternar entre agacharse y ponerse de pie
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCrouch();
        }
    }

    private void ControlCamera()
    {
        // Obtener input del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotar el jugador en el eje Y (horizontal)
        transform.Rotate(Vector3.up * mouseX);

        // Limitar el ángulo de visión vertical (pitch)
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        // Rotar la cámara en el eje X (vertical)
        playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;
    }

    private void ToggleCrouch()
    {
        // Alternar entre agacharse y ponerse de pie
        if (isCrouching)
        {
            controller.height = standingHeight;
            isCrouching = false;
        }
        else
        {
            controller.height = crouchHeight;
            isCrouching = true;
        }
    }
}