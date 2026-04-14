using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_Text txtEscritura;

    private string textoActual = "";
    [SerializeField] private TareasYFrases enunciados;
    // Start is called before the first frame update
    void Start()
    {
        enunciados = this.GetComponent<TareasYFrases>();
    }

    // Update is called once per frame
    void Update()
    {
        mostrarEscritura();
    }

    private void mostrarEscritura()
    {
        string input = Input.inputString;

        foreach (char c in input)
        {
            if (c == '\b')
            {
                if (textoActual.Length > 0)
                {
                    textoActual = textoActual.Substring(0,textoActual.Length - 1);
                }
            }
            else if (c == '\n' || c == '\r')
            {
                Debug.Log("Texto final: " + textoActual);
                textoActual = "";
            }
            else
            {
                textoActual += c;
            }
        }

        txtEscritura.text = textoActual;
    }
}
