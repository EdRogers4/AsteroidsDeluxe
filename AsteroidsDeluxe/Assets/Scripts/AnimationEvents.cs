using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private GameManager _scriptGameManager;
    [SerializeField] private Player _scriptPlayer;

    public void PlayerReady()
    {
        StartCoroutine(_scriptGameManager.SpawnDeathStar());
        _scriptGameManager.NextLevel();
        _scriptGameManager.SetLives(2);
        _scriptPlayer.PlayerStart();
    }

    public void ResetStartButton()
    {
        _scriptGameManager.ResetStartButton();
    }
}
