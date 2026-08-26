using UnityEngine;

namespace PrototypeTerror.Investigation
{
    public class PuertaTeletransporte : MonoBehaviour, IInteractable
    {
        [Header("Configuración de Teletransporte")]
        [SerializeField] private Transform puntoDestino;

        public void Interactuar()
        {
            if (puntoDestino != null)
            {
                Debug.Log($"[PuertaTeletransporte] Iniciando teletransporte a {puntoDestino.position}");
                CharacterController playerController = FindObjectOfType<SimplePlayerController>()?.GetComponent<CharacterController>();
                
                if (playerController != null && TransicionUIController.Instance != null)
                {
                    TransicionUIController.Instance.IniciarTeletransporte(puntoDestino, playerController);
                }
                else
                {
                    Debug.LogWarning("[PuertaTeletransporte] Faltan referencias para el teletransporte (Jugador o TransicionUIController).");
                }
            }
            else
            {
                Debug.LogWarning("[PuertaTeletransporte] No se ha asignado un punto de destino.");
            }
        }
    }
}
