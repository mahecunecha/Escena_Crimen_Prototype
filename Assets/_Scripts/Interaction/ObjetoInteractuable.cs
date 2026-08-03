using UnityEngine;

namespace PrototypeTerror.Investigation
{
    public class ObjetoInteractuable : MonoBehaviour
    {
        [SerializeField] private FichaPista fichaPista;

        public FichaPista Ficha => fichaPista;
    }
}