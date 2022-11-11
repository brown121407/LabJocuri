using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public bool Won => _score >= 5;
    
    private TextMeshProUGUI _timerText;
    private float _timer = 0;
    public List<GameObject> pickups = new();
    
    private uint _score = 0;
    private TextMeshProUGUI _scoreText;
    private GameObject _congratsTextObject;
    private GameObject _canvas;
    [SerializeField]
    private TMP_InputField input;

    private static int _noGoodPickups = 2;
    private static int _noBadPickups = 1;

    private void Awake()
    {
        var goodPickup = Resources.Load("Prefabs/Pickup") as GameObject;
        var badPickup = Resources.Load("Prefabs/Bad Pickup") as GameObject;

        for (var i = 0; i < _noGoodPickups; i++)
        {
            var pickup = Instantiate(goodPickup, Vector3.zero, Quaternion.identity);
            pickup.SetActive(false);
            pickups.Add(pickup);
        }
        
        for (var i = 0; i < _noBadPickups; i++)
        {
            var pickup = Instantiate(badPickup, Vector3.zero, Quaternion.identity);
            pickup.SetActive(false);
            pickups.Add(pickup);
        }
        
        _canvas = GameObject.Find("Canvas");
        _scoreText = _canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _congratsTextObject = _canvas.transform.GetChild(1).gameObject;
        input.onEndEdit.AddListener(SaveBestPlayer);
    }

    private void Start()
    {
        GameObject.FindWithTag(Tags.Player);
        _timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();

        StartCoroutine(RunTimer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("DELETED PLAYER PREFS");
        }
    }

    private IEnumerator RunTimer()
    {
        while (true)
        {
            _timer += 0.1f;
            _timerText.text = _timer.ToString("F1");
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public static Vector3 RandomPosition(Vector3 origin)
    {
        while (true)
        {
            var position = new Vector3
            {
                x = Random.Range(-4.5f, 4.5f), 
                y = 0.5f, 
                z = Random.Range(-4.5f, 4.5f)
            };

            if (Vector3.Distance(position, origin) <= 2.0f) continue;
            
            return position;
        }
    }
    
    public void UpdateScore()
    {
        _score++;
        if (Won)
        {
            if (PlayerPrefs.HasKey("BestTime"))
            {
                var bestTime = PlayerPrefs.GetFloat("BestTime");
                if (_timer < bestTime)
                {
                    UpdateHighScore();
                }
                else
                {
                    _congratsTextObject.GetComponent<TextMeshProUGUI>().text = $"Your time is: {_timer}. Best time is: {bestTime} by {PlayerPrefs.GetString("Player")}";
                }
            }
            else
            {
                UpdateHighScore();
            }

            _congratsTextObject.SetActive(true);
        }
        _scoreText.text = $"Score: {_score}";
    }

    private void UpdateHighScore()
    {
        input.gameObject.SetActive(true);
        PlayerPrefs.SetFloat("BestTime", _timer);
        _congratsTextObject.GetComponent<TextMeshProUGUI>().text = $"Congrats! New best time is: {_timer}";
    }

    public void SaveBestPlayer(string text)
    {
        PlayerPrefs.SetString("Player", text);
        Debug.Log(PlayerPrefs.GetString("Player"));
        input.gameObject.SetActive(false);
    }
}
