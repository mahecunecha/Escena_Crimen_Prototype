using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

namespace PrototypeTerror.Investigation
{
    [RequireComponent(typeof(UIDocument))]
    public class TransicionUIController : MonoBehaviour
    {
        public static TransicionUIController Instance;

        [SerializeField] private float duracionFade = 0.5f;

        private UIDocument uiDocument;
        private VisualElement panelTransicion;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            if (uiDocument != null && uiDocument.rootVisualElement != null)
            {
                panelTransicion = uiDocument.rootVisualElement.Q<VisualElement>("PanelTransicion");
                if (panelTransicion != null)
                {
                    panelTransicion.style.opacity = 0f;
                    panelTransicion.pickingMode = PickingMode.Ignore;
                }
            }
        }

        public void IniciarTeletransporte(Transform destino, CharacterController playerController)
        {
            if (panelTransicion == null)
            {
                Debug.LogWarning("[TransicionUIController] No se encontró el PanelTransicion. Teletransportando directamente.");
                RealizarTrasladoFisico(destino, playerController);
                return;
            }

            StartCoroutine(RutinaFade(destino, playerController));
        }

        private IEnumerator RutinaFade(Transform destino, CharacterController playerController)
        {
            GameManager.Instance.IniciarTransicion();

            // Fade Out
            float tiempo = 0f;
            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFade);
                panelTransicion.style.opacity = alpha;
                yield return null;
            }
            panelTransicion.style.opacity = 1f;

            // TRASLADO FÍSICO
            RealizarTrasladoFisico(destino, playerController);

            // Fade In
            tiempo = 0f;
            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
                panelTransicion.style.opacity = alpha;
                yield return null;
            }
            panelTransicion.style.opacity = 0f;

            GameManager.Instance.FinalizarTransicion();
        }

        private void RealizarTrasladoFisico(Transform destino, CharacterController playerController)
        {
            if (playerController != null && destino != null)
            {
                playerController.enabled = false;
                playerController.transform.position = destino.position;
                playerController.transform.rotation = destino.rotation;
                playerController.enabled = true;
            }
        }
    }
}
