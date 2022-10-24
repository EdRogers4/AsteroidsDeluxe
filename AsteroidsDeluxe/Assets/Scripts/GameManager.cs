using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> listEnemies;
    public List<GameObject> listDeathStar;
    public bool isGameOver;
    public bool isRestart;
    public bool isHighScoreReached;
    public bool isHighScoreUI;
    public int[] highScores;
    [SerializeField] private GameObject _drone;
    [SerializeField] private Drone _scriptDrone;
    [SerializeField] private Player _scriptPlayer;
    [SerializeField] private List<Image> _listLifeIcons;
    [SerializeField] private List<Transform> _listSpawnPoints;
    [SerializeField] private Animator _animatorStartScreen;
    [SerializeField] private Animator _animatorHighScore;
    [SerializeField] private int _asteroidsToSpawn;
    [SerializeField] private int _currentRank;
    [SerializeField] private int _currentInitial;
    [SerializeField] private int _currentInput;
    [SerializeField] private int[] _topScores;
    [SerializeField] private GameObject _prefabAsteroidLarge;
    [SerializeField] private GameObject _prefabDeathStarLarge;
    [SerializeField] private Text _textScore;
    [SerializeField] private Text _textBonus;
    [SerializeField] private Text _textTopScore;
    [SerializeField] private Text _textTopInitials;
    [SerializeField] private Text[] _textHighScore;
    [SerializeField] private Text[] _textInitials;
    [SerializeField] private Text[] _textInput;
    [SerializeField] private Image[] _underline;
    [SerializeField] private Image _currentScore;
    [SerializeField] private string[] _stringInput;
    [SerializeField] private float _respawnDroneMin;
    [SerializeField] private float _respawnDroneMax;
    [SerializeField] private float _respawnDeathStarMin;
    [SerializeField] private float _respawnDeathStarMax;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioDestroyDeathStarLarge;
    [SerializeField] private AudioClip _audioDestroyDeathStarMedium;
    [SerializeField] private AudioClip _audioDestroyDeathStarSmall;
    [SerializeField] private AudioClip _audioDestroyDrone;
    [SerializeField] private AudioClip _audioExplosionLarge;
    [SerializeField] private AudioClip _audioExplosionMedium;
    [SerializeField] private AudioClip _audioExplosionSmall;
    [SerializeField] private AudioClip _audioShieldOff;
    [SerializeField] private AudioClip _audioShieldOn;
    [SerializeField] private AudioClip _audioSpawnDrone;
    [SerializeField] private AudioClip _audioSpawnPlayer1;
    [SerializeField] private AudioClip _audioSpawnPlayer2;
    [SerializeField] private AudioClip _audioStartClick;
    [SerializeField] private AudioClip _audioBonus;
    
    private int _score;
    private int _bonus;
    private int _formatScoreCount;
    private int _formatScoreLength;
    private bool _isStartGame;
    private bool _isStartInitials;

    private void Start()
    {
        _formatScoreLength = _textScore.text.Length;
        _currentRank = 10;
        _bonus = 10000;
        int.TryParse(_textHighScore[0].text, out _topScores[0]);
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
            NextLevel();
        }
    }

    public void RemoveDeathStar(GameObject deathStar)
    {
        listDeathStar.Remove(deathStar);

        if (listDeathStar.Count <= 0)
        {
            StartCoroutine(SpawnDeathStar());
        }
    }
    
    public void GameOver()
    {
        _animatorStartScreen.SetBool("isGameOver", true);
        _animatorStartScreen.SetBool("isStart", false);
        isGameOver = true;
    }

    public void ResetStartButton()
    {
        for (int i = 0; i < listEnemies.Count; i++)
        {
            Destroy(listEnemies[i]);
        }
        
        listEnemies.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartButton()
    {
        if (!_isStartGame)
        {
            _isStartGame = true;
            _animatorStartScreen.SetBool("isStart", true);
            _animatorStartScreen.SetBool("isGameOver", false);
            isGameOver = false;
            StartCoroutine(SpawnDrone());
        }
    }

    public void SetLives(int lives)
    {
        for (int i = 0; i < _listLifeIcons.Count; i++)
        {
            _listLifeIcons[i].enabled = i < lives;
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.Return) && !_isStartGame && !isHighScoreUI)
        {
            StartButton();
            PlaySoundStartClick();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (isRestart && _audioSource.volume > 0)
        {
            _audioSource.volume -= 0.000155f;
        }

        if (isHighScoreUI && isHighScoreReached)
        {
            if (Input.GetKeyDown(KeyCode.Return) && _currentInitial < 3)
            {
                _currentInitial += 1;
                _currentInput = 1;
                _animatorHighScore.SetInteger("input", _currentInitial + 1);
                PlaySoundBonus();
                RecordInitials();

                if (_currentInitial >= 3)
                {
                    isRestart = true;
                    _animatorHighScore.SetBool("isRestart", true);

                    for (int i = 0; i < _underline.Length; i++)
                    {
                        _underline[i].enabled = false;
                        _textInput[i].text = "   ";
                    }
                }
            }
            
            if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                _currentInput -= 1;
                CheckCurrentInput();
            }

            if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.RightArrow)))
            {
                _currentInput += 1;
                CheckCurrentInput();
            }
        }
    }

    private void CheckCurrentInput()
    {
        if (_currentInput < 0)
        {
            _currentInput = _stringInput.Length - 1;
        }
        else if (_currentInput > _stringInput.Length - 1)
        {
            _currentInput = 0;
        }
        
        PlaySoundStartClick();
        _textInput[_currentInitial].text = _stringInput[_currentInput];
    }

    private void RecordInitials()
    {
        _textInitials[_currentRank].text = "";
        
        for (int i = 0; i < _currentInitial; i++)
        {
            _textInitials[_currentRank].text += _textInput[i].text;
        }
    }

    public void RecordHighScore()
    {
        _textScore.text = "" + _score;
        _formatScoreCount = _formatScoreLength - _textScore.text.Length;
        Debug.Log("Format score count: " + _formatScoreCount);
        FormatScore(_textHighScore[_currentRank]);
        _currentScore.rectTransform.position 
            = new Vector3(_textHighScore[_currentRank].rectTransform.position.x, _textHighScore[_currentRank].rectTransform.position.y);
        _textInitials[_currentRank].text = "   ";
    }

    public void StartInitials()
    {
        _currentInput = 1;
        _animatorHighScore.SetInteger("input", 1);
        
        for (int i = 0; i < _underline.Length; i++)
        {
            _underline[i].enabled = true;
            _textInput[i].enabled = true;
        }
    }

    public void UpdateScore(int points)
    {
        _score += points;
        _textScore.text = "" + _score;
        _formatScoreCount = _formatScoreLength - _textScore.text.Length;
        FormatScore(_textScore);

        if (_score >= _bonus)
        {
            _scriptPlayer.lives += 1;
            SetLives(_scriptPlayer.lives);
            _bonus += 10000;
            _textBonus.text = "" + _bonus;
            PlaySoundBonus();
        }

        if (_score > _topScores[0])
        {
            if (_textTopInitials.text != "   ")
            {
                _textTopInitials.text = "   ";
            }
            
            _textTopScore.text = "" + _score;
            _formatScoreCount = _formatScoreLength - _textTopScore.text.Length;
            FormatScore(_textTopScore);
        }

        if (_score > _topScores[_topScores.Length - 1] && !isHighScoreReached)
        {
            isHighScoreReached = true;
        }

        if (_score > _topScores[_currentRank - 1])
        {
            Debug.Log("Current rank:" + _currentRank);
            _currentRank -= 1;
        }
    }

    private void FormatScore(Text text)
    {
        text.text = "";

        for (int i = 0; i < _formatScoreCount; i++)
        {
            text.text += "0";
        }

        text.text += "" + _score;
    }

    public IEnumerator SpawnDrone()
    {
        yield return new WaitForSeconds(Random.Range(_respawnDroneMin, _respawnDroneMax));
        var spawnPoint = Random.Range(0, (_listSpawnPoints.Count));
        _drone.transform.position = _listSpawnPoints[spawnPoint].position;
        _scriptDrone.isMoveRight = spawnPoint < _listSpawnPoints.Count / 2;
        _scriptDrone.collider.enabled = true;
        _scriptDrone.renderer.enabled = true;
        _scriptDrone.particleJet[0].Play();
        _scriptDrone.particleJet[1].Play();
        _scriptDrone.isActive = true;
        PlaySoundSpawnDrone();
    }

    public IEnumerator SpawnDeathStar()
    {
        yield return new WaitForSeconds(Random.Range(_respawnDeathStarMin, _respawnDeathStarMax));
        var newDeathStar = Instantiate(_prefabDeathStarLarge, transform.position, transform.rotation);
        listEnemies.Add(newDeathStar);
        listDeathStar.Add(newDeathStar);
        var scriptDeathStar = newDeathStar.GetComponent<DeathStar>();
        scriptDeathStar.startingAxis = Random.Range(0,5);
        scriptDeathStar.scriptGameManager = this;
        scriptDeathStar.target = _scriptPlayer.transform;
    }

    public void PlaySoundDestroyDeathStarLarge()
    {
        _audioSource.PlayOneShot(_audioDestroyDeathStarLarge, 2.0f);
    }
    
    public void PlaySoundDestroyDeathStarMedium()
    {
        _audioSource.PlayOneShot(_audioDestroyDeathStarMedium, 2.0f);
    }
    
    public void PlaySoundDestroyDeathStarSmall()
    {
        _audioSource.PlayOneShot(_audioDestroyDeathStarSmall, 5.0f);
    }
    
    public void PlaySoundDestroyDrone()
    {
        _audioSource.PlayOneShot(_audioDestroyDrone, 1.5f);
    }
    
    public void PlaySoundExplosionLarge()
    {
        _audioSource.PlayOneShot(_audioExplosionLarge, 1.8f);
    }
    
    public void PlaySoundExplosionMedium()
    {
        _audioSource.PlayOneShot(_audioExplosionMedium, 1.8f);
    }
    
    public void PlaySoundExplosionSmall()
    {
        _audioSource.PlayOneShot(_audioExplosionSmall, 1.8f);
    }
    
    public void PlaySoundShieldOff()
    {
        _audioSource.PlayOneShot(_audioShieldOff, 1.5f);
    }
    
    public void PlaySoundShieldOn()
    {
        _audioSource.PlayOneShot(_audioShieldOn, 1.5f);
    }
    
    private void PlaySoundSpawnDrone()
    {
        _audioSource.PlayOneShot(_audioSpawnDrone, 1.5f);
    }
    
    public void PlaySoundSpawnPlayer()
    {
        _audioSource.PlayOneShot(_audioSpawnPlayer1, 1.5f);
        _audioSource.PlayOneShot(_audioSpawnPlayer2, 1.5f);
    }
    
    private void PlaySoundStartClick()
    {
        _audioSource.PlayOneShot(_audioStartClick, 1.5f);
    }
    
    private void PlaySoundBonus()
    {
        _audioSource.PlayOneShot(_audioBonus, 4.0f);
    }
}
