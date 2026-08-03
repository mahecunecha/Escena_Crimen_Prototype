using UnityEngine;

namespace PrototypeTerror.Investigation
{
    [CreateAssetMenu(fileName = "NuevaFichaPista", menuName = "PrototypeTerror/Investigation/FichaPista")]
    public class FichaPista : ScriptableObject
    {
        public string idPista;
        public string nombreVisual;
        public string idPrerrequisito;
        
        [TextArea]
        public string textoBloqueado;
        
        [TextArea]
        public string textoDesbloqueado;
    }
}