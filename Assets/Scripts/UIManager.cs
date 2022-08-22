using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // handle to text
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoCountText;

    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    private Player _player;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _scoreText.text = "0";
        _ammoCountText.text = "15/15";

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.Log("GameManager doesn't exist");
            Debug.Log("GameManager doesn't exist");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = playerScore.ToString();
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoCountText.text = ammoCount.ToString() + "/15";

    }
    // declare Ammo count text
    // function to update Ammo text


    public void UpdateLives(int currentLives)
    {
        if (currentLives > -1)
        {
            _LivesImg.sprite = _liveSprites[currentLives];
        }

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(TextFlicker());
    }
    IEnumerator TextFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
        }
    }


}
