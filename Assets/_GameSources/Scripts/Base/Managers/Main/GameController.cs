using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] float _deathDelay;
    private  WaitForSecondsRealtime _deathDelayTime;
    public static GameState ActiveState { get; private set; }

    #region Base Functions
    private void Start()
    {
        _deathDelayTime = new(_deathDelay);
        SetState(GameState.Start);
    }
    private void OnEnable()
    {
        PlayerController.OnDied += HandlePlayerDied;
        InputManager.OnGameStateRequested += SetState;
        UiController.OnStateSet += SetState;
    }

    private void OnDisable()
    {
        PlayerController.OnDied -= HandlePlayerDied;
        InputManager.OnGameStateRequested -= SetState;
        UiController.OnStateSet -= SetState;
    }
    #endregion

    #region GameController Functions
    private void HandlePlayerDied(BasePlayerSO _, Vector2 position){
        StartCoroutine(ActivateGameOver());
    }

    private IEnumerator ActivateGameOver()
    {
        yield return _deathDelayTime;
        SetState(GameState.GameOver);
    }

    public void SetState(GameState _state) {
        if (ActiveState == _state) return;
        ActiveState = _state;
        print(ActiveState);
        OnGameStateChanged?.Invoke(ActiveState);
    }
    #endregion
}