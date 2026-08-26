using UnityEngine;

namespace PrototypeTerror.Investigation
{
    public class DetectorInteraccion : MonoBehaviour
    {
        [SerializeField] private float distanciaInteraccion = 3.0f;
        [SerializeField] private LayerMask capaInteractuable;
        [SerializeField] private Camera camaraPrincipal;

        private void Awake()
        {
            if (camaraPrincipal == null)
            {
                camaraPrincipal = Camera.main;
            }
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.EstadoActual != GameManager.EstadoJuego.Gameplay) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
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
}