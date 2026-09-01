using UnityEngine;
using UnityEngine.InputSystem;

namespace PrototypeTerror.Investigation
{
    public class DetectorInteraccion : MonoBehaviour
    {
        [SerializeField] private float distanciaInteraccion = 3.0f;
        [SerializeField] private LayerMask capaInteractuable;
        [SerializeField] private Camera camaraPrincipal;
        [SerializeField] private InputActionReference interaccionAction;

        private void Awake()
        {
            if (camaraPrincipal == null)
            {
                camaraPrincipal = Camera.main;
            }
        }

        private void OnEnable()
        {
            if (interaccionAction != null)
            {
                interaccionAction.action.performed += RealizarInteraccion;
            }
        }

        private void OnDisable()
        {
            if (interaccionAction != null)
            {
                interaccionAction.action.performed -= RealizarInteraccion;
            }
        }

        private void RealizarInteraccion(InputAction.CallbackContext context)
        {
            if (GameManager.Instance != null && GameManager.Instance.EstadoActual != GameManager.EstadoJuego.Gameplay) return;

            Ray ray = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, distanciaInteraccion, capaInteractuable))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interactuar();
                }
            }
        }
    }
}