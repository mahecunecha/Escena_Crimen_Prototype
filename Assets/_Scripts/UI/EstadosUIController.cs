using UnityEngine;
using UnityEngine.UIElements;

namespace PrototypeTerror.Investigation
{
    [RequireComponent(typeof(UIDocument))]
    public class EstadosUIController : MonoBehaviour
    {
        private UIDocument uiDocument;

        private VisualElement contenedorRaiz;
        private Label lblTitulo;
        private Label lblMensaje;
        private Button btnReanudar;
        private Button btnSalir;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            var root = uiDocument.rootVisualElement;
            if (root != null)
            {
                contenedorRaiz = root.Q<VisualElement>("ContenedorRaiz");
                lblTitulo = root.Q<Label>("lblTitulo");
                lblMensaje = root.Q<Label>("lblMensaje");
                btnReanudar = root.Q<Button>("btnReanudar");
                btnSalir = root.Q<Button>("btnSalir");

                if (contenedorRaiz != null) contenedorRaiz.pickingMode = PickingMode.Ignore;
                if (btnReanudar != null) btnReanudar.pickingMode = PickingMode.Position;
                if (btnSalir != null) btnSalir.pickingMode = PickingMode.Position;

                if (btnReanudar != null)
                {
                    btnReanudar.clicked += () => GameManager.Instance.ReanudarJuego();
                }
                if (btnSalir != null)
                {
                    btnSalir.clicked += () => Application.Quit();
                }
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnEstadoCambiado += ActualizarPantalla;
                ActualizarPantalla(GameManager.Instance.EstadoActual);
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnEstadoCambiado -= ActualizarPantalla;
            }
        }

        private void ActualizarPantalla(GameManager.EstadoJuego nuevoEstado)
        {
            if (nuevoEstado == GameManager.EstadoJuego.Gameplay)
            {
                if (contenedorRaiz != null)
                {
                    contenedorRaiz.style.display = DisplayStyle.None;
                }
            }
            else if (nuevoEstado == GameManager.EstadoJuego.Pausa)
            {
                if (contenedorRaiz != null)
                {
                    contenedorRaiz.style.display = DisplayStyle.Flex;
                    lblTitulo.text = "PAUSA";
                    lblMensaje.text = "El juego está detenido.";
                    btnReanudar.style.display = DisplayStyle.Flex;
                }
            }
            else if (nuevoEstado == GameManager.EstadoJuego.Resultado)
            {
                if (VentanaPistaController.Instance != null)
                {
                    VentanaPistaController.Instance.OcultarVentana();
                    // OcultarVentana bloquea el cursor, así que debemos restaurarlo para poder hacer clic en "Salir del Juego"
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                    UnityEngine.Cursor.visible = true;
                }

                if (contenedorRaiz != null)
                {
                    contenedorRaiz.style.display = DisplayStyle.Flex;
                    btnReanudar.style.display = DisplayStyle.None;

                    if (GestorInvestigacion.Instance != null && GestorInvestigacion.Instance.ErroresCometidos >= GestorInvestigacion.Instance.MaxErrores)
                    {
                        lblTitulo.text = "GAME OVER";
                        lblMensaje.text = "Has cometido demasiados errores y la trampa se ha activado...";
                    }
                    else
                    {
                        lblTitulo.text = "¡VICTORIA!";
                        lblMensaje.text = "Has descubierto la entrada a las catacumbas secretas.";
                    }
                }
            }
        }
    }
}
