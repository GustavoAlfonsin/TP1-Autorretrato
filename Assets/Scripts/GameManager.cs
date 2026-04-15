using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text txtEscritura;
    public TMP_Text txtDescripcionTarea;

    private string textoActual = "";
    private string fraseActual;
    [SerializeField] private TareasYFrases enunciados;
    public Tarea tareaActual;

    public Slider _slider;
    public TMP_Text txtPositivo;
    public TMP_Text txtNegativo;

    //Tiempo pensamiento negativo
    public float tiempoEntrePensamientos = 5f;
    public float tiempoVisible = 7f;

    public float daño = 8f;
    public float aumento = 10f;

    // Fin del juego
    public GameObject panelFin;
    public TMP_Text txtResultado;

    // Panel de inicio
    public GameObject panelInicio;
    // Start is called before the first frame update
    void Start()
    {
        enunciados = this.GetComponent<TareasYFrases>();
        panelFin.SetActive(false);
        panelInicio.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Empezar()
    {
        tareaActual = enunciados.darTareaRandom();
        txtDescripcionTarea.text = tareaActual.descripcion;
        txtNegativo.text = "";
        StartCoroutine(cicloPensamientos());
        nuevaFrase();
        panelInicio.SetActive(false);
        Time.timeScale = 1f;
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
                verificarFrase();
            }
            else
            {
                textoActual += c;
            }
        }

        txtEscritura.text = textoActual;
    }

    IEnumerator cicloPensamientos()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntrePensamientos);

            string fraseN = tareaActual.recibirNegativo();
            txtNegativo.text = fraseN;

            yield return new WaitForSeconds(tiempoVisible);

            _slider.value -= daño;
            if(_slider.value <= _slider.minValue)
            {
                _slider.value = _slider.minValue;
                mostrarFinDelJuego(false);
            }
            txtNegativo.text = "";
        }
    }

    void nuevaFrase()
    {
        fraseActual = tareaActual.recibirPositivo();
        txtPositivo.text = fraseActual;

        textoActual = "";
        txtEscritura.text = "";
    }

    void verificarFrase()
    {
        if (textoActual.Trim().ToLower() == fraseActual.Trim().ToLower())
        {
            _slider.value += aumento;
            if (_slider.value >= _slider.maxValue)
            {
                _slider.value = _slider.maxValue;
                mostrarFinDelJuego(true);
            }
            Debug.Log("CORRECTO!!!");
        }
        else
        {
            Debug.Log("Incorrecto =(");
        }

        nuevaFrase();
    }

    private void mostrarFinDelJuego(bool ganaste)
    {
        panelFin.SetActive(true);
        if (ganaste)
        {
            txtResultado.text = "TE ANISMASTE A HACERLO";
        }
        else
        {
            txtResultado.text = "No te animaste a hacelo";
        }

        Time.timeScale = 0f;
    }

    public void reiniciarElJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void salirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        Application.Quit();
    }
}
