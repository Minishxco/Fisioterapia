using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Linq;
using System;

public class Level : MonoBehaviour
{
    public GameObject PanelNivel, ButtonNivel;
    public GameObject[] Levels;
    public TextMeshProUGUI[] Correctas;
    public int NivelDesbloqueado, Pregunta;
    public quiz_game qg;
    public TextMeshProUGUI CompletadosCorrect, CompletadosIncorrect;
    public int[] puntaje;
    public int[] total;
    public GameObject[] Bloques;
    public int[] Secciones, inicio;
    public int Sec;

    public Progreso progreso;
    public bool win;

    public GameObject panelEstrellas, PanelWin;
    public GameObject[] Estrellas;
    public TextMeshProUGUI TextEstrella, TextProgreso;

    public GameObject continuar;

    public void Start()
    {
        ActualizarBotones();
        if (Pregunta > 0)
        {
            continuar.SetActive(true);
        }
    }

    public void ActualizarBotones()
    {
        if (PlayerPrefs.HasKey("puntaje"))
        {
            string intArrayString1 = PlayerPrefs.GetString("puntaje");
            int[] intArray1 = intArrayString1.Split(',').Select(s => int.Parse(s)).ToArray();
            if (intArray1.Length > 0)
            //if (progreso.puntaje.Length > 0)
            {
                //puntaje = progreso.puntaje;
                puntaje = intArray1;
            }
        }

        //obtener Sec
        //Sec = progreso.Sec;
        Sec = PlayerPrefs.GetInt("Sec");


        for (int i = 0; i < Sec && Sec <= 43; i++)
        {
            Levels[i].GetComponent<UnityEngine.UI.Button>().interactable = true;
            Correctas[i].text = puntaje[i] + "/" + total[i].ToString();

            Levels[i].transform.GetChild(0).gameObject.SetActive(false);
            Levels[i].transform.GetChild(1).gameObject.SetActive(false);
            Levels[i].transform.GetChild(2).gameObject.SetActive(false);

            float estrellas = (puntaje[i] * 100) / total[i];
            if (estrellas > 90)
            {
                Levels[i].transform.GetChild(0).gameObject.SetActive(true);
                Levels[i].transform.GetChild(1).gameObject.SetActive(true);
                Levels[i].transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (estrellas > 60)
            {
                Levels[i].transform.GetChild(0).gameObject.SetActive(true);
                Levels[i].transform.GetChild(1).gameObject.SetActive(true);
                Levels[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            else if (estrellas > 30)
            {
                Levels[i].transform.GetChild(0).gameObject.SetActive(true);
                Levels[i].transform.GetChild(1).gameObject.SetActive(false);
                Levels[i].transform.GetChild(2).gameObject.SetActive(false);
            }

        }

        if (Sec <= 0)
        {
            ButtonNivel.SetActive(false);
        }

        //Obtener Pregunta
        //Pregunta = progreso.Pregunta;
        Pregunta = PlayerPrefs.GetInt("Pregunta");
        TextProgreso.text = Pregunta.ToString();//Mostrar en que pregunta va el usuario
    }

    public void Actualizar()
    {
        if (Pregunta == 849)
        {
            Debug.Log("Win");
            win = true;
            Sec = 0;
            Pregunta = 0;
        }
        else
        {
            Correctas[Sec].text = puntaje[Sec] + "/" + total[Sec].ToString();
            CompletadosCorrect.text = "Nivel " + (Sec + 1) + "\n" + puntaje[Sec] + "/" + total[Sec].ToString();
            CompletadosIncorrect.text = "Nivel " + (Sec + 1) + "\n" + puntaje[Sec] + "/" + total[Sec].ToString();
            SiguientePregunta();
        }

    }

    public void Puntaje()
    {
        if (Pregunta == 849)
        {
            Debug.Log("Win");
        }
        else
        {
            puntaje[Sec]++;
        }

    }


    public void SiguientePregunta()
    {
        if(Pregunta > 0)
        {
            continuar.SetActive(true);
        }

        if (Pregunta < qg.dataPlayer.Pregunta.Count)
        {
            Pregunta++;
        }

        //if (Pregunta >= progreso.Pregunta)
        if (Pregunta >= PlayerPrefs.GetInt("Pregunta"))
        {

            //guardar la pregunta que se lleva hasta el momento
            //progreso.Pregunta = Pregunta;
            PlayerPrefs.SetInt("Pregunta", Pregunta);
        }


        //guardar puntaje del nivel
        //progreso.puntaje = puntaje;
        if (puntaje.Length > 0)
        {
            //Actualizar
            string intArrayString = string.Join(",", puntaje.Select(i => i.ToString()).ToArray());
            PlayerPrefs.SetString("puntaje", intArrayString);
        }

        if (Pregunta == Secciones[Sec])
        {
            Levels[Sec].GetComponent<UnityEngine.UI.Button>().interactable = true;

            Invoke("ShowPanelStars", 1f);
            Debug.Log("Nivel " + Sec + " Completado");
            Sec++;
            //if (Sec >= progreso.Sec)
            if (Sec >= PlayerPrefs.GetInt("Sec"))
            {
                //Guardar nivel
                //progreso.Sec = Sec;
                PlayerPrefs.SetInt("Sec", Sec);
            }

            ButtonNivel.SetActive(true);

        }
    }

    public void SelectLevel(int level)
    {
        if(qg.vidas > 1)
        {
            Sec = level;
            puntaje[Sec] = 0;
            Pregunta = inicio[level];//donde inicia cada bloque de preguntas
            PanelNivel.SetActive(false);
            qg.Play();
            qg.UpdateVida(-1);
            qg.ShowCorazon();
        }
        else
        {
            qg.ShowAdvertencia("Necesitas más vidas", false);
        }
        
    }

    public void Continuar()
    {
        if (!win)
        {
            //Sec = progreso.Sec;
            //Pregunta = progreso.Pregunta;//donde continua la pregunta
            Sec = PlayerPrefs.GetInt("Sec");
            Pregunta = PlayerPrefs.GetInt("Pregunta");
        }
        qg.Play();
    }

    public void Niveles()
    {
        ActualizarBotones();
        PanelNivel.SetActive(true);
        qg.AudioButtonExample();
    }

    public int bloque;
    public void Next()
    {
        qg.AudioButtonExample();
        for (int i = 0; i < Bloques.Length; i++)
        {
            Bloques[i].SetActive(false);
        }
        bloque++;
        if (bloque >= Bloques.Length)
        {
            bloque = 0;
        }

        Bloques[bloque].SetActive(true);
    }

    public void Previous()
    {
        qg.AudioButtonExample();
        for (int i = 0; i < Bloques.Length; i++)
        {
            Bloques[i].SetActive(false);
        }
        bloque--;
        if (bloque < 0)
        {
            bloque = Bloques.Length - 1;
        }
        Bloques[bloque].SetActive(true);
    }

    private void ShowPanelStars()//mejorar esta parte
    {
        panelEstrellas.SetActive(true);
        //Nivel Sec Completado
        TextEstrella.text = "Nivel " + Sec + " Completado";

        Estrellas[0].SetActive(false);
        Estrellas[1].SetActive(false);
        Estrellas[2].SetActive(false);
        float estrellas = (puntaje[Sec-1] * 100) / total[Sec-1];
        if (estrellas > 90)
        {
            Estrellas[0].SetActive(true);
            Estrellas[1].SetActive(true);
            Estrellas[2].SetActive(true);
        }
        else if (estrellas > 60)
        {
            Estrellas[0].SetActive(true);
            Estrellas[1].SetActive(true);
            Estrellas[2].SetActive(false);
        }
        else if (estrellas > 30)
        {
            Estrellas[0].SetActive(true);
            Estrellas[1].SetActive(false);
            Estrellas[2].SetActive(false);
        }

    }

    public void DesactivarPanelEstrellas()
    {
        panelEstrellas.SetActive(false);
        qg.Menu();
        Niveles();
    }
}
