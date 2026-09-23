using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    #region Ui Settings
    [Header("UI Text Settings")]
    [SerializeField] private Text ScoreA;
    [SerializeField] private Text ScoreH;
    [SerializeField] private Text WaveNum;
    [Header("UI Panel Settings")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Image[] HShips;
    [SerializeField] private GameObject _gamePausePanel;
    public static event Action<GameState> OnStateSet;
    #endregion
    #region base Functions
    private void OnEnable()
    {
        ScoreManager.WhenScoreChnaged += UpdateScoreUi;
        PlayerController.WhenExtraLifeUsed += WhenHealthChange;

        WaveManager.OnWaveNumberChanged += UpdateWaveUI;
        GameController.OnGameStateChanged += WhenStateChange;
    }

    private void WhenHealthChange(int _CurrentH, BasePlayerSO PsO)
    {
        if (_CurrentH >= PsO.MinHealth)
        {
            HShips[_CurrentH].gameObject.SetActive(false);
        }
    }
    public void RestartGameB() { SFXManager.Instance.PlayOnShot(); OnStateSet?.Invoke(GameState.Start); SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    public void MainMenuB() { SFXManager.Instance.PlayOnShot(); OnStateSet?.Invoke(GameState.Start); SceneManager.LoadScene(0); }
    public void ResumeB() { SFXManager.Instance.PlayOnShot(); OnStateSet?.Invoke(GameState.Resume); }
    public void GalaxyLeaveB() { SFXManager.Instance.PlayOnShot(); Application.Quit();}
    private void WhenStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Stop:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _gamePausePanel.SetActive(true);
                break;
            case GameState.GameOver:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _gamePausePanel.SetActive(false);
                _gameOverPanel.SetActive(true);
                break;
            case GameState.Start:
            case GameState.Resume:
            default:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _gameOverPanel.SetActive(false);
                _gamePausePanel.SetActive(false);
                break;
        }
    }

    private void OnDisable()
    {
        ScoreManager.WhenScoreChnaged -= UpdateScoreUi;
        PlayerController.WhenExtraLifeUsed -= WhenHealthChange;

        WaveManager.OnWaveNumberChanged -= UpdateWaveUI;
        GameController.OnGameStateChanged -= WhenStateChange;
    }
    #endregion
    #region UiController Fuctions
    private void UpdateWaveUI(int _waveNum = 0) => WaveNum.text = "Wave:" + _waveNum.ToString();
    private void UpdateScoreUi(int ActScore = 0, int HigScore = 0)
    { ScoreA.text = ActScore.ToString(); ScoreH.text = "High-Score: " + HigScore.ToString(); }
    #endregion
}
