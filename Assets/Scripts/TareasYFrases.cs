using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Tarea
{
    public string descripcion;
    public List<string> positivos, negativos;

    public Tarea(string descripcion, List<string> positivos, List<string> negativos)
    {
        this.descripcion = descripcion;
        this.positivos = positivos;
        this.negativos = negativos;
    }

    public string recibirPositivo() 
    {
        int i = UnityEngine.Random.Range(0, positivos.Count);
        return positivos[i];
    }

    public string recibirNegativo() 
    {
        int i = UnityEngine.Random.Range(0, negativos.Count);
        return negativos[i];
    }
}
public class TareasYFrases : MonoBehaviour
{
    public List<Tarea> tareas = new List<Tarea>();
    public string nombreArchivo = "tareas.csv";

    private void Awake()
    {
        cargarCSV();
    }
    void cargarCSV()
    {
        string ruta = Path.Combine(Application.streamingAssetsPath, nombreArchivo);
        if (!File.Exists(ruta))
        {
            Debug.LogError("Archivo no encontrado en: " + ruta);
            return;
        }

        string[] lineas = File.ReadAllLines(ruta);

        for (int i = 1; i < lineas.Length; i++)
        {
            string linea = lineas[i];
            string[] columnas = SepararCSV(linea);
            if (columnas.Length < 3)
            {
                Debug.LogWarning("Linea mal formada: " + linea);
                continue;
            }

            string descripcion = columnas[0];
            List<string> positivos = new List<string>(columnas[1].Split('|'));
            List<string> negativos = new List<string>(columnas[2].Split('|'));

            Tarea nuevaTarea = new Tarea(descripcion, positivos, negativos);

            tareas.Add(nuevaTarea);
        }

        Debug.Log("Tareas cargadas: " + tareas.Count);
    }

    private string[] SepararCSV(string linea)
    {
        return new List<string>(linea.Split(',')).ToArray();
    }

    public Tarea darTareaRandom()
    {
        int i = UnityEngine.Random.Range(0, tareas.Count);
        Debug.Log($"Tamaño de la lista: {tareas.Count}, indice elegido: {i}");
        return tareas[i];
    }
}
