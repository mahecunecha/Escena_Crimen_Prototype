using UnityEngine;
using UnityEngine.UIElements;

namespace PrototypeTerror.Investigation
{
    public class VentanaPistaController : MonoBehaviour
    {
        public static VentanaPistaController Instance;

        private UIDocument uiDocument;
        private VisualElement rootVisualElement;
        private VisualElement overlay;
        private Label lblTitulo;
        private Label lblContenido;
        private Button btnCerrar;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            if (uiDocument == null) return;

            rootVisualElement = uiDocument.rootVisualElement;
            if (rootVisualElement == null) return;

            overlay = rootVisualElement.Q<VisualElement>("overlay");
            lblTitulo = rootVisualElement.Q<Label>("lbl-titulo");
            lblContenido = rootVisualElement.Q<Label>("lbl-contenido");
            btnCerrar = rootVisualElement.Q<Button>("btn-cerrar");

            if (btnCerrar != null)
            {
                btnCerrar.clicked += OcultarVentana;
            }

            if (overlay != null)
            {
                overlay.style.display = DisplayStyle.None;
            }
        }

        private void OnDisable()
        {
            if (btnCerrar != null)
            {
                btnCerrar.clicked -= OcultarVentana;
            }
        }

        public void MostrarVentana(string titulo, string contenido)
        {
            if (lblTitulo != null) lblTitulo.text = titulo;
            if (lblContenido != null) lblContenido.text = contenido;
            
            if (overlay != null)
            {
                overlay.style.display = DisplayStyle.Flex;
            }

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }

        public void OcultarVentana()
        {
            if (overlay != null)
            {
                overlay.style.display = DisplayStyle.None;
            }

            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }
}