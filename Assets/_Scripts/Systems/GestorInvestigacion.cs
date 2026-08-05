using UnityEngine;
using System.Collections.Generic;

namespace PrototypeTerror.Investigation
{
    public class GestorInvestigacion : MonoBehaviour
    {
        public static GestorInvestigacion Instance;

        private HashSet<string> pistasDescubiertas;
        private int erroresCometidos = 0;
        private const int MAX_ERRORES = 3;

        public int ErroresCometidos => erroresCometidos;
        public int MaxErrores => MAX_ERRORES;

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
                if (ficha.esPistaFinal)
                {
                    GameManager.Instance.Victoria();
                }
                return true;
            }
            
            erroresCometidos++;
            if (erroresCometidos >= MAX_ERRORES)
            {
                GameManager.Instance.GameOver();
            }
            return false;
        }
    }
}