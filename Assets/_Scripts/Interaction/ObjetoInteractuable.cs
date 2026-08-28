using UnityEngine;

namespace PrototypeTerror.Investigation
{
    public class ObjetoInteractuable : MonoBehaviour, IInteractable
    {
        [SerializeField] private FichaPista fichaPista;
        [SerializeField] private AudioSource audioSourcePista;
        [SerializeField] private AudioClip sonidoDesbloqueo;
        [SerializeField] private bool variarPitch = false;

        public FichaPista Ficha => fichaPista;

        public void Interactuar()
        {
            if (fichaPista != null)
            {
                bool exito = GestorInvestigacion.Instance.EvaluarPista(fichaPista);
                if (exito)
                {
                    VentanaPistaController.Instance.MostrarVentana(fichaPista.nombreVisual, fichaPista.textoDesbloqueado);

                    if (audioSourcePista != null && sonidoDesbloqueo != null)
                    {
                        if (variarPitch)
                        {
                            audioSourcePista.pitch = Random.Range(0.9f, 1.1f);
                        }
                        audioSourcePista.PlayOneShot(sonidoDesbloqueo);
                    }
                }
                else
                {
                    VentanaPistaController.Instance.MostrarVentana(fichaPista.nombreVisual, fichaPista.textoBloqueado);
                }
            }
        }
    }
}