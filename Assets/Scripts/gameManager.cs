using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using JetBrains.Annotations;

public class gameManager : MonoBehaviour
{
    float limit = 30.0f;
    public Text timeText;
    public GameObject panel;
    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject endTxt;
    public AudioSource audioSource;
    public AudioClip match;
    public AudioClip wrong;
    public GameObject audioManager;
    public static gameManager I;
    public GameObject timeTxt;

    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        int[] mokokos = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        mokokos = mokokos.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 16; i++)
        {

            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);

            string mokokoName = "mokoko" + mokokos[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(mokokoName);
        }

        initGame();
    }

    void initGame()
    {
        Time.timeScale = 1.0f;
        limit = 30.0f;
    }


    // Update is called once per frame
    void Update()
    {
        limit -= Time.deltaTime;

        if (limit <= 10)
        {
            GameObject.Find("timeTxt").GetComponent<Text>().color = Color.red;
        }

        if (limit < 0)
        {
            limit = 0.0f;
            panel.SetActive(true);
            Time.timeScale = 0.0f;
            AudioEnd();
        }

        timeText.text = limit.ToString("N2");
    }

    public void retry()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void isMatched()
    {
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            audioSource.PlayOneShot(match);

            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();
            int cardsLeft = GameObject.Find("cards").transform.childCount;
            if (cardsLeft == 2)
            {
                endTxt.SetActive(true);
                Invoke("GameEnd", 0.5f);
            }
        }
        else
        {
            audioSource.PlayOneShot(wrong);

            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();
        }

        firstCard = null;
        secondCard = null;
    }

    void GameEnd()
    {
        Time.timeScale = 0.0f;
        endTxt.SetActive(true);
        AudioEnd();
    }

    void AudioEnd()
    {
        audioManager am = audioManager.GetComponent<audioManager>();
        am.GameEnd();
    
    }

}
