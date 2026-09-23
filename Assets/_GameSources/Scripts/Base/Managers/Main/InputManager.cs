using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {  get; private set; }
    public static event Action<GameState> OnGameStateRequested;
    private GameState _current;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (_current == GameState.GameOver) return;
        if (OpenCloseMenu())
        {
            if (_current == GameState.Stop)
                OnGameStateRequested?.Invoke(GameState.Resume);
            else
                OnGameStateRequested?.Invoke(GameState.Stop);
                
        }
    }
    private void OnEnable() => GameController.OnGameStateChanged += HandleGameStateChanged;
    private void OnDisable() => GameController.OnGameStateChanged -= HandleGameStateChanged;

    private void HandleGameStateChanged(GameState _state) => _current = _state;

    public float GetXAxis() => Input.GetAxis("Horizontal");
    private bool OpenCloseMenu() => Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F);
    public bool Shoot() => Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space);
}
