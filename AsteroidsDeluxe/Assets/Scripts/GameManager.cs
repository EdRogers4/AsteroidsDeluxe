using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> listEnemies;
    [SerializeField] private Animator _animatorStartScreen;
    [SerializeField] private int _asteroidsToSpawn;
    [SerializeField] private GameObject _prefabAsteroidLarge;
    [SerializeField] private Text _textScore;
    private int _score;
    private int _formatScoreCount;
    private int _formatScoreLength;
    private bool _isStartGame;

    private void Start()
    {
        _formatScoreLength = _textScore.text.Length;
    }

    public void NextLevel()
    {
        for (int i = 0; i < _asteroidsToSpawn; i++)
        {
            var newAsteroid = Instantiate(_prefabAsteroidLarge, transform.position, transform.rotation);
            listEnemies.Add(newAsteroid);
            var scriptAsteroid = newAsteroid.GetComponent<Asteroid>();
            scriptAsteroid.startingAxis = i;
            scriptAsteroid.scriptGameManager = this;
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        listEnemies.Remove(enemy);

        if (listEnemies.Count <= 0)
        {
            
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.Return) && !_isStartGame)
        {
            _isStartGame = true;
            _animatorStartScreen.SetBool("isStart", true);
        }
    }

    public void UpdateScore(int points)
    {
        _score += points;
        _textScore.text = "" + _score;
        _formatScoreCount = _formatScoreLength - _textScore.text.Length;
        _textScore.text = "";

        for (int i = 0; i < _formatScoreCount; i++)
        {
            _textScore.text += "0";
        }

        _textScore.text += "" + _score;
    }
}
