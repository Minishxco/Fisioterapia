using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class Data : MonoBehaviour
{
   
}

[System.Serializable]
public class SlotData
{
    public List<SaveSlot> Pregunta = new List<SaveSlot>();
}

[System.Serializable]
public class SaveSlot
{
    public int id;
    [Header("Pregunta")]
    public string pregunta;
    [Header("Respuestas")]
    public List<Resp> Respuesta = new List<Resp>();
    [Header("DescripcionRespuesta")]
    public string DescRespuesta;
}

[System.Serializable]
public class Resp
{
    [Header("Correcta")]
    public bool correcta;
    [Header("Respuesta")]
    public string respuesta;
}
