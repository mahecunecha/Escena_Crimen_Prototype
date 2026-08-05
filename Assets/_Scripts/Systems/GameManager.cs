using UnityEngine;

namespace PrototypeTerror.Investigation
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public enum EstadoJuego { Inicio, Gameplay, Pausa, Resultado }
        public EstadoJuego EstadoActual { get; private set; } = EstadoJuego.Inicio;
        public event System.Action<EstadoJuego> OnEstadoCambiado;

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
        }

        private void Start()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            
            if (EstadoActual == EstadoJuego.Inicio)
            {
                Time.timeScale = 0f;
                OnEstadoCambiado?.Invoke(EstadoJuego.Inicio);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (EstadoActual == EstadoJuego.Gameplay)
                {
                    PausarJuego();
                }
                else if (EstadoActual == EstadoJuego.Pausa)
                {
                    ReanudarJuego();
                }
            }
        }

        public void IniciarJuego()
        {
            Debug.Log("[GameManager] Ejecutando IniciarJuego()...");
            EstadoActual = EstadoJuego.Gameplay;
            Time.timeScale = 1f;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            OnEstadoCambiado?.Invoke(EstadoJuego.Gameplay);
            Debug.Log($"[GameManager] Estado cambiado a {EstadoActual}. TimeScale: {Time.timeScale}");
        }

        public void PausarJuego()
        {
            EstadoActual = EstadoJuego.Pausa;
            Time.timeScale = 0f;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            OnEstadoCambiado?.Invoke(EstadoJuego.Pausa);
        }

        public void ReanudarJuego()
        {
            EstadoActual = EstadoJuego.Gameplay;
            Time.timeScale = 1f;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            OnEstadoCambiado?.Invoke(EstadoJuego.Gameplay);
        }

        public void Victoria()
        {
            EstadoActual = EstadoJuego.Resultado;
            Time.timeScale = 0f;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            OnEstadoCambiado?.Invoke(EstadoJuego.Resultado);
        }

        public void GameOver()
        {
            EstadoActual = EstadoJuego.Resultado;
            Time.timeScale = 0f;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            OnEstadoCambiado?.Invoke(EstadoJuego.Resultado);
        }
    }
}