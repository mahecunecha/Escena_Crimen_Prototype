using UnityEngine;

namespace PrototypeTerror.Investigation
{
    public class ObjetoInteractuable : MonoBehaviour, IInteractable
    {
        [SerializeField] private FichaPista fichaPista;

        public FichaPista Ficha => fichaPista;

        public void Interactuar()
        {
            if (fichaPista != null)
            {
                bool exito = GestorInvestigacion.Instance.EvaluarPista(fichaPista);
                if (exito)
                {
                    VentanaPistaController.Instance.MostrarVentana(fichaPista.nombreVisual, fichaPista.textoDesbloqueado);
                }
                else
                {
                    VentanaPistaController.Instance.MostrarVentana(fichaPista.nombreVisual, fichaPista.textoBloqueado);
                }
            }
        }
    }
}