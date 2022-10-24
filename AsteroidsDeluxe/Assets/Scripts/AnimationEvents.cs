using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private GameManager _scriptGameManager;
    [SerializeField] private Player _scriptPlayer;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioGameStart;
    [SerializeField] private AudioClip _audioGameOver;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Animator _animatorStartScreen;
    [SerializeField] private Animator _animatorHighScore;

    public void PlayerReady()
    {
        StartCoroutine(_scriptGameManager.SpawnDeathStar());
        _scriptGameManager.NextLevel();
        _scriptGameManager.SetLives(2);
        _scriptPlayer.PlayerStart();
        _audioSource.PlayOneShot(_audioGameStart, 0.5f);
    }

    public void GameOver()
    {
        _audioSource.PlayOneShot(_audioGameOver, 1.5f);
    }

    public void ResetStartButton()
    {
        _scriptGameManager.ResetStartButton();
    }

    public void ToggleUnderlines()
    {
        _scriptGameManager.StartInitials();
    }
    
    public void ToggleCanvasOff()
    {
        _canvas.enabled = false;
        _animatorHighScore.SetBool("isStart", false);
        _animatorStartScreen.SetBool("isRestart", true);
        _scriptGameManager.isHighScoreUI = false;
    }

    public void ToggleCanvasOn()
    {
        Debug.Log("Toggle Canvas");
        _canvas.enabled = true;
        _animatorHighScore.SetBool("isStart", true);
    }
}
