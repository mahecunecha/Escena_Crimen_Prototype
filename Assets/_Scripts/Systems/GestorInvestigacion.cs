using UnityEngine;
using System.Collections.Generic;

namespace PrototypeTerror.Investigation
{
    public class GestorInvestigacion : MonoBehaviour
    {
        public static GestorInvestigacion Instance;

        private HashSet<string> pistasDescubiertas;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                pistasDescubiertas = new HashSet<string>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool EvaluarPista(FichaPista ficha)
        {
            if (string.IsNullOrEmpty(ficha.idPrerrequisito) || pistasDescubiertas.Contains(ficha.idPrerrequisito))
            {
                pistasDescubiertas.Add(ficha.idPista);
                return true;
            }
            return false;
        }
    }
}