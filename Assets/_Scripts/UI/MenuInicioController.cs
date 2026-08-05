using UnityEngine;
using UnityEngine.UIElements;

namespace PrototypeTerror.Investigation
{
    [RequireComponent(typeof(UIDocument))]
    public class MenuInicioController : MonoBehaviour
    {
        private UIDocument uiDocument;
        private VisualElement contenedorInicio;
        private Button btnIniciar;
        private Button btnSalirInicio;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            var root = uiDocument.rootVisualElement;
            if (root != null)
            {
                contenedorInicio = root.Q<VisualElement>("ContenedorInicio");
                btnIniciar = root.Q<Button>("btnIniciar");
                btnSalirInicio = root.Q<Button>("btnSalirInicio");

                if (contenedorInicio != null) contenedorInicio.pickingMode = PickingMode.Ignore;
                if (btnIniciar != null) btnIniciar.pickingMode = PickingMode.Position;
                if (btnSalirInicio != null) btnSalirInicio.pickingMode = PickingMode.Position;

                Debug.Log($"[UI_Inicio] Buscando botón... btnIniciar es null: {btnIniciar == null}");

                if (btnIniciar != null)
                {
                    btnIniciar.clicked += () => 
                    {
                        Debug.Log("[UI_Inicio] ¡EVENTO CLICKED DISPARADO EN EL BOTÓN!");
                        if (GameManager.Instance != null) GameManager.Instance.IniciarJuego();
                        else Debug.LogError("[UI_Inicio] GameManager.Instance es NULL al hacer clic!");
                    };
                }
                if (btnSalirInicio != null)
                {
                    btnSalirInicio.clicked += () => Application.Quit();
                }
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnEstadoCambiado += ActualizarVisibilidad;
                ActualizarVisibilidad(GameManager.Instance.EstadoActual);
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnEstadoCambiado -= ActualizarVisibilidad;
            }
        }

        private void ActualizarVisibilidad(GameManager.EstadoJuego nuevoEstado)
        {
            Debug.Log($"[UI_Inicio] Cambiando visibilidad para estado {nuevoEstado}. Contenedor null: {contenedorInicio == null}");
            if (contenedorInicio != null)
            {
                if (nuevoEstado == GameManager.EstadoJuego.Inicio)
                {
                    contenedorInicio.style.display = DisplayStyle.Flex;
                }
                else
                {
                    contenedorInicio.style.display = DisplayStyle.None;
                }
            }
        }
    }
}
