using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Reflection;

public class quiz_game : MonoBehaviour
{
    public bool leerDatosNuevos;


    public int vidas, monedas;
    public float tiempoReloj, resetReloj;
    public TextMeshProUGUI pregunta, timeText, timeReloj, vidaText, MonedaText;
    public TextMeshProUGUI[] respuestas;
    private int id, respuesta;
    private bool pause = true;
    public Image[] imageButton;
    public Image barra;
    public GameObject PanelIncorrecto, PanelCorrecto, PanelPause, PanelPlay, PanelMenu, PanelStore, PanelSettings, PanelSalir, PanelLevels;
    public GameObject BtnTienda, RelojIcon, BtnSettings, BtnAudioOn, BtnAudioOff;
    public GameObject BtnTema;
    private int tema;

    public string nameFileData = "dataPlayer";//dataPlayer.json
    private string filePath;
    [SerializeField] public SlotData dataPlayer = new SlotData();
    public Save save;

    private List<int> ListRandom = new List<int>();
    public float time = 12, timeReset;

    public AudioListener audioListener;
    public AudioSource audioSource;
    public float volume = 0.5f;
    public AudioClip ButtonClip, SelectClip, CorrectClip, IncorrectClip, AlertClip, BuyClip, NotificationAudio;

    public GameObject Particle;
    public GameObject PanelAdvertencia;
    public Image Advertencia;
    public TextMeshProUGUI advertenciaText;

    //public AdsInitializer Ad;
    public Themes colorPallette;

    public float timeElapsed;
    public AdsManager adsManager;

    private float timeAdd = 300f, timeVideo = 300f;
    private bool video;

    public Level level;
    public GameObject panelCorazon;

    void Start()
    {
        video = false;
        string filePath = Application.persistentDataPath + "/time.txt";
        if (File.Exists(filePath))
        {
            string time = File.ReadAllText(filePath);
            DateTime savedTime = DateTime.Parse(time);
            timeElapsed = (float)(DateTime.Now - savedTime).TotalSeconds;
        }

        if (!PlayerPrefs.HasKey("corazon"))
        {
            PlayerPrefs.SetInt("corazon", vidas);
        }
        if (!PlayerPrefs.HasKey("monedas"))
        {
            PlayerPrefs.SetInt("monedas", monedas);
        }
        vidas = PlayerPrefs.GetInt("corazon");
        monedas = PlayerPrefs.GetInt("monedas");
        vidaText.text = vidas.ToString();
        MonedaText.text = monedas.ToString();
        resetReloj = tiempoReloj;
        timeReset = time;
        ReadData();


        tema = PlayerPrefs.GetInt("tema");
        BtnTema.transform.GetChild(0).GetComponent<Image>().color = colorPallette.Color1[tema];

    }

    public void Update()
    {
        if(video)
        {
            timeVideo -= Time.deltaTime;
            if (timeVideo <= 0)
            {
                video = false;
            }
        }

        if(timeAdd >= 0)
        {
            timeAdd -= Time.deltaTime;
        }

        if (!pause)
        {
            time -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelStore.SetActive(false);
            PanelSettings.SetActive(false);
            PanelLevels.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Escape) && PanelMenu.activeSelf && !PanelStore.activeSelf && !PanelSettings.activeSelf)
        {
            PanelSalir.SetActive(true);
        }

        if (time > 0){
            timeText.text = time.ToString("F0");
            barra.fillAmount = (((time * 100)/timeReset)/100);
        }
        else if(!PanelIncorrecto.activeSelf)
        {
            Invoke("ShowPanelIncorrect", 0f);
        }


        if (vidas < 10)
        {

            timeElapsed += Time.deltaTime;
            tiempoReloj = resetReloj - timeElapsed;
        }
        else
        {
            timeElapsed = 0;
        }

        if (tiempoReloj >= 0 && vidas < 10)
        {
            // tiempoReloj -= Time.deltaTime;
            int minutos = (int)tiempoReloj / 60;
            int Segundos = (int)tiempoReloj - (minutos * 60);
            timeReloj.text = minutos + " : " + Segundos + " ".ToString();
            RelojIcon.SetActive(true);
        }else
        {
            while (timeElapsed > resetReloj)
            {
                if (vidas < 10)
                {
                    UpdateVida(1);
                    timeElapsed -= resetReloj;
                }
                else
                {
                    break;
                }
            }
            tiempoReloj = resetReloj;
            RelojIcon.SetActive(false);
        }
    }

    public void ReadData()
    {
        Clean();

        if (leerDatosNuevos)
        {
            //nuevos datos
            filePath = Path.Combine(Application.dataPath, "Json/" + (nameFileData + ".json"));
            Debug.Log(filePath);
            string data = File.ReadAllText(filePath);//Leer Fisioterapiadata.json
            dataPlayer = JsonUtility.FromJson<SlotData>(data);
            save.dataPlayer = dataPlayer;

        }
        else
        {
            //datos existentes
            dataPlayer = save.dataPlayer;
        }

        id = level.Pregunta;//la pregunta con la que se inicia
        ListRandom.Clear();
        //id = UnityEngine.Random.Range(0, dataPlayer.Pregunta.Count);
        
        pregunta.text = dataPlayer.Pregunta[id].pregunta;

        for (int i = 0; i < respuestas.Length; i++)
        {
            /*int UniqueRandom = UniqueRandomInt(0, respuestas.Length);
            respuestas[i].text = dataPlayer.Pregunta[id].Respuesta[UniqueRandom].respuesta;
            if(dataPlayer.Pregunta[id].Respuesta[UniqueRandom].correcta)
            {
                respuesta = i;
            }*/

            respuestas[i].text = dataPlayer.Pregunta[id].Respuesta[i].respuesta;
            if (dataPlayer.Pregunta[id].Respuesta[i].correcta)
            {
                respuesta = i;
            }
        }
    }

    [Obsolete]
    public void Button(int num)
    {
        AudioButton(SelectClip);
        if (num == respuesta)
        {
            pause = true;
            imageButton[num].color = colorPallette.Color3[colorPallette.numPallette];
            PanelPause.SetActive(true);
            UpdateCoint(24);
            Invoke("ShowPanelCorrect", 1f);
            //Debug.Log("Bien");
            if(timeAdd <= 0)
            {
                adsManager.LlamarInterstical();
                timeAdd = 300f;
            }
            level.Puntaje();
            level.Actualizar();
        }
        else
        {
            pause = true;
            imageButton[num].color = colorPallette.Color4[colorPallette.numPallette];
            PanelPause.SetActive(true);
            Invoke("ShowPanelIncorrect", 1f);
            //Debug.Log("Mal");
            UpdateVida(-1);
            if (timeAdd <= 0)
            {
                adsManager.LlamarInterstical();
                timeAdd = 300f;
            }
            level.Actualizar();
        }
    }

    public void NewQuiz()
    {
        if (monedas >= 50)
        {
            UpdateCoint(-50);
            NextQuiz();
        }
        else
        {
            ShowAdvertencia("No tienes suficientes monedas", false);
        }
    }

    public void NextQuiz()
    {
        if(vidas > 0)
        {
            AudioButton(ButtonClip);
            ReadData();
            PanelCorrecto.SetActive(false);
            PanelIncorrecto.SetActive(false);
            pause = false;
            time = timeReset;
            Clean();
            PanelPlay.SetActive(true);
            PanelMenu.SetActive(false);
        }
        else
        {
            ShowAdvertencia("No tienes suficientes vidas", false);
        }
        
    }

    public void UpdateQuiz()
    {
        if (monedas >= 30 )
        {
            List<int> availableButtons = Enumerable.Range(0, imageButton.Length).Where(i => i != respuesta).ToList();
            int quitar = availableButtons[UnityEngine.Random.Range(0, availableButtons.Count)];
            imageButton[quitar].color = Color.gray;
            UpdateCoint(-30);
        }
        else if(monedas < 30)
        {
            ShowAdvertencia("No tienes suficientes monedas", false);
        }
    } 

    public void Menu()
    {
        AudioButton(ButtonClip);
        BtnTienda.SetActive(true);
        BtnSettings.SetActive(true);
        pause = true;
        PanelPlay.SetActive(false);
        PanelMenu.SetActive(true);
        PanelIncorrecto.SetActive(false);
        PanelCorrecto.SetActive(false);
    }

    public void Play()//entrar 
    {
        if (vidas > 0)
        {
            
            AudioButton(ButtonClip);
            BtnTienda.SetActive(false);
            BtnSettings.SetActive(false);
            NextQuiz();
        }
        else
        {
            ShowAdvertencia("No tienes suficientes vidas", false);
        }
    }

    public void Store()
    {
        AudioButton(ButtonClip);
        PanelStore.SetActive(true);
    }

    public void Ajustes()
    {
        AudioButton(ButtonClip);
        PanelSettings.SetActive(true);
    }

    public void Audio(bool activeAudio)
    {
        if(activeAudio)
        {
            BtnAudioOn.SetActive(false);
            BtnAudioOff.SetActive(true);
            AudioListener.volume = 0;
        }
        else
        {
            BtnAudioOff.SetActive(false);
            BtnAudioOn.SetActive(true);
            AudioListener.volume = 1;
            AudioButton(NotificationAudio);
        }
    }

    
    public void Tema()
    {
        if (tema < colorPallette.Color1.Length-1)
        {
            tema++;
            Temas();
        }
        else
        {
            tema = 0;
            Temas();
        }
        
    }

    private void Temas()
    {
        AudioButton(ButtonClip);
        colorPallette.numPallette = tema;
        BtnTema.transform.GetChild(0).GetComponent<Image>().color = colorPallette.Color1[tema];
        colorPallette.Tema();
    }

    [Obsolete]
    public void Store(int select)
    {
        switch (select)
        {
            case 0:
                if(monedas >= 150)
                {
                    UpdateVida(1);
                    UpdateCoint(-150);
                    ShowAdvertencia("Haz comprado 1 vida",true);
                }
                else
                {
                    ShowAdvertencia("No tienes suficientes monedas", false);
                }
                break;
            case 1:
                if(!video)
                {
                    //Ad.PlayVideoAd();//se activa el video
                    adsManager.LlamarRecompensado();
                    timeVideo = 300f;
                    video = true;
                }
                else
                {
                    ShowAdvertencia("No puedes ver un anuncio por ahora!!", false);
                }
                    
                break;
            case 2:
                if (monedas >= 350)
                {
                    UpdateVida(5);
                    UpdateCoint(-350);
                    ShowAdvertencia("Haz comprado 5 vidas", true);
                }
                else
                {
                    ShowAdvertencia("No tienes suficientes monedas", false);
                }
                break;
            case 3:
                if (monedas >= 600)
                {
                    ShowAdvertencia("Haz comprado 10 vidas", true);
                    UpdateVida(10);
                    UpdateCoint(-600);
                }
                else
                {
                    ShowAdvertencia("No tienes suficientes monedas", false);
                }
                break;
            default:
                break;
        }
    }

    private void UpdateCoint(int num)
    {
        monedas += num;
        PlayerPrefs.SetInt("monedas", PlayerPrefs.GetInt("monedas") + num);
        MonedaText.text = monedas.ToString();
    }

    public void UpdateVida(int num)
    {
        vidas += num;
        PlayerPrefs.SetInt("corazon", PlayerPrefs.GetInt("corazon") + num);
        vidaText.text = vidas.ToString();
    }

    public void AudioButtonExample()
    {
        AudioButton(ButtonClip);
    }

    private void AudioButton(AudioClip Clip)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(Clip, volume);
        }
    }

    private void ShowPanelCorrect()
    {
        AudioButton(CorrectClip);
        PanelCorrecto.SetActive(true);
        PanelPause.SetActive(false);
        Instantiate(Particle);
    }

    public void ShowCorazon()
    {
        panelCorazon.SetActive(true);
        Invoke("HideWarning", 0.3f);
    }

    public void ShowAdvertencia(string message, bool blue)
    {
        advertenciaText.text = message;
        PanelAdvertencia.SetActive(true);

        if(!blue)//incorrect
        {
            Advertencia.color = colorPallette.Color4[colorPallette.numPallette];
            AudioButton(AlertClip);
        }
        else//correct
        {
            AudioButton(BuyClip);
            Advertencia.color = colorPallette.Color5[colorPallette.numPallette];
        }
        

        Invoke("HideWarning", 1f);
    }

    public void HideWarning()
    {
        PanelAdvertencia.SetActive(false);
        panelCorazon.SetActive(false);
    }

    private void ShowPanelIncorrect()
    {
        AudioButton(IncorrectClip);
        PanelIncorrecto.SetActive(true);
        PanelPause.SetActive(false);
    }

    public int UniqueRandomInt(int min, int max)
    {
        int val = UnityEngine.Random.Range(min, max);
        while (ListRandom.Contains(val))
        {
            val = UnityEngine.Random.Range(min, max);
            if (ListRandom.Count >= max)
            {
                return val;
            }
        }
        ListRandom.Add(val);
        return val;
    }

    private void Clean()
    {
        for (int i = 0; i < imageButton.Length; i++)
        {
            imageButton[i].color = Color.white;
        }
    }

    // Guardar la hora actual en un archivo de configuración cuando se cierra el juego
    void OnApplicationQuit()
    {
        string filePath = Application.persistentDataPath + "/time.txt";
        string time = DateTime.Now.ToString();
        File.WriteAllText(filePath, time);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
