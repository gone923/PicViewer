using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class PicPlay : MonoBehaviour
{
    public Image pic;
    public GameObject panel;
    public InputField picPath;
    public InputField timeDevide;
    public Button play;
    public Button pause;
    public Text pauseText;
    public Button hide;
    public Button quit;
    private string path;
    private PicData picData;
    public bool isPlaying = false;
    public bool isPaused = false;
    public int indexToPlay = 0;
    private float timeRecord = 0;
    // Start is called before the first frame update
    void Start()
    {
        play.onClick.AddListener(OnPlayButton);
        pause.onClick.AddListener(OnPauseButton);
        hide.onClick.AddListener(OnHideButton);
        quit.onClick.AddListener(Application.Quit);
        timeDevide.text = "1000";
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            //Debug.Log("Current time: " + Time.time);
            if(Time.time - timeRecord > picData.data[indexToPlay].delay/ float.Parse(timeDevide.text))
            {
                if(indexToPlay < picData.data.Count-1)
                {
                    indexToPlay++;
                }
                else
                {                    
                    indexToPlay = 0;
                }
                CutPic(indexToPlay);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //panel.SetActive(true);
            panel.transform.localScale = Vector3.one;
        }
    }

    public void OnPlayButton()
    {
        Resources.UnloadUnusedAssets();
        path = picPath.text;
        if(path != null && path.Length > 0 )
        {
            string jsonPath = path + "/animation.json";
            if (File.Exists(jsonPath))
            {
                string jsonData = File.ReadAllText(jsonPath);
                Debug.Log(jsonData);
                picData = new PicData();
                picData.data = JsonConvert.DeserializeObject<List<PicRef>>(jsonData);
            }
            else
            {
                Debug.LogError("File " + jsonPath + " doesn't exist.");
            }
        }
        if(picData != null && picData.data.Count > 0 ) 
        { 
            isPlaying = true;
            CutPic(0);
        }
    }

    private void CutPic(int index)
    {
        Resources.UnloadUnusedAssets();
        indexToPlay = index;
        timeRecord = Time.time;
        StartCoroutine(LoadPic(path + "/" + picData.data[indexToPlay].file));
    }

    private IEnumerator LoadPic( string url )
    {
        double startTime = Time.time;
        WWW www = new WWW(url);
        yield return www;
        if(www != null && string.IsNullOrEmpty(www.error))
        {
            Texture2D texture = www.texture;
            Sprite sptite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
            pic.sprite = sptite;
            startTime= (double) Time.time - startTime ;
            Debug.Log("www加载用时 ： " + startTime);
        }
    }

    public void OnPauseButton()
    {
        if (isPaused)
        {
            Time.timeScale = 1.0f;
            pauseText.text = "暂停";
        }
        else
        {
            Time.timeScale = 0f;
            pauseText.text = "继续";
        }
        isPaused = !isPaused;
    }

    public void OnHideButton()
    {
        //panel.SetActive(false);
        panel.transform.localScale = Vector3.zero;
    }
}
[SerializeField]
public class PicRef
{
    public string file;
    public float delay;
}

[SerializeField]
public class PicData
{
    public List<PicRef> data;
}
