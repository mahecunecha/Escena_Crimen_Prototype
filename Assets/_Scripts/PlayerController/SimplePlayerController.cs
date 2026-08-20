using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerController : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("El objeto vacío que simula la cabeza del jugador, hijo de este GameObject.")]
    [SerializeField]
    private Transform headTransform;

    [Header("Configuración de Velocidades")] [SerializeField]
    private float velocidadCaminando = 3.5f;

    [SerializeField] private float velocidadCorriendo = 6.0f;
    [SerializeField] private float velocidadAgachado = 1.5f;
    [SerializeField] private float velocidadRotacion = 10f;

    [Header("Suavizado de Movimiento")] [SerializeField]
    private float smoothTime = 0.1f;

    [SerializeField] private float rotationSmoothTime = 0.02f;

    [Header("Input Actions")] [SerializeField]
    private InputActionReference moveAction;

    [SerializeField] private InputActionReference sprintAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference crouchAction;

    [Header("Animación")] [SerializeField] private Animator animatorJugador;

    // Variables internas de física y rotación
    private CharacterController _controller;
    private Vector3 _velocitySmooth;
    private Vector3 _velocitySmoothDerivative;
    private float _currentRotationSmoothVelocity;
    private float _yaw;
    private float _pitch;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (sprintAction != null) sprintAction.action.Enable();
        if (lookAction != null) lookAction.action.Enable();
        if (crouchAction != null) crouchAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (sprintAction != null) sprintAction.action.Disable();
        if (lookAction != null) lookAction.action.Disable();
        if (crouchAction != null) crouchAction.action.Disable();
    }

    private void Start()
    {
        if (!headTransform) Debug.LogError("¡Falta asignar el Transform del Head en el Inspector!");

        // Inicializar ángulos
        _yaw = transform.eulerAngles.y;
        if (headTransform != null)
        {
            _pitch = headTransform.eulerAngles.x;
            if (_pitch > 180f) _pitch -= 360f;
        }

        // Bloquear y ocultar el cursor
        if (PrototypeTerror.Investigation.GameManager.Instance == null ||
            PrototypeTerror.Investigation.GameManager.Instance.EstadoActual !=
            PrototypeTerror.Investigation.GameManager.EstadoJuego.Inicio)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        ManejarCamara();
        ManejarMovimiento();
    }

    private void ManejarCamara()
    {
        Vector2 mouseInput = lookAction != null ? lookAction.action.ReadValue<Vector2>() : Vector2.zero;

        // 1. Calcular nuevos ángulos incrementales
        _yaw += mouseInput.x * velocidadRotacion * Time.deltaTime;
        _pitch -= mouseInput.y * velocidadRotacion * Time.deltaTime;

        // 2. Limitar la vista vertical (Gimbal Lock preventivo)
        _pitch = Mathf.Clamp(_pitch, -89f, 89f);

        // 3. Rotar el cuerpo (Eje Y) con suavizado
        float smoothedYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, _yaw, ref _currentRotationSmoothVelocity,
            rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, smoothedYaw, 0f);

        // 4. Rotar la cabeza (Ejes X e Y) de forma instantánea
        if (headTransform != null)
        {
            headTransform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
    }

    private void ManejarMovimiento()
    {
        // 1. Leer Input de movimiento
        Vector2 inputVector = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;

        // 2. Calcular direcciones relativas a la cabeza, proyectadas de forma plana (sin volar)
        Vector3 forward = headTransform.forward;
        Vector3 right = headTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 direccionMovimiento = (forward * inputVector.y + right * inputVector.x).normalized;

        // 3. Determinar la velocidad actual
        float velocidadActual = 0f;
        if (direccionMovimiento.sqrMagnitude > 0f)
        {
            bool isSprinting = sprintAction != null && sprintAction.action.IsPressed();
            bool isCrouching = crouchAction != null && crouchAction.action.IsPressed();

            if (isCrouching)
            {
                velocidadActual = velocidadAgachado;
            }
            else if (isSprinting)
            {
                velocidadActual = velocidadCorriendo;
            }
            else
            {
                velocidadActual = velocidadCaminando;
            }
        }

        // 4. Suavizar el vector de movimiento general
        Vector3 desiredVelocity = direccionMovimiento * velocidadActual;
        _velocitySmooth =
            Vector3.SmoothDamp(_velocitySmooth, desiredVelocity, ref _velocitySmoothDerivative, smoothTime);

        if (animatorJugador != null)
        {
            // 1. Enviamos la velocidad matemática exacta
            animatorJugador.SetFloat("Velocidad", _velocitySmooth.magnitude);

            // 2. Leemos el input directamente aquí y se lo pasamos al Animator de inmediato
            animatorJugador.SetBool("IsCrouching", crouchAction.action.IsPressed());
        }

        // 5. Aplicar Movimiento y Gravedad
        Vector3 moveVector = _velocitySmooth * Time.deltaTime;

        if (!_controller.isGrounded)
        {
            moveVector.y -= 9.81f * Time.deltaTime; // Gravedad simple
        }

        _controller.Move(moveVector);
    }
}