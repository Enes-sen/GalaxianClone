using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    private AudioSource _aSource;
   [SerializeField] private AudioClip _clip;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _aSource = GetComponent<AudioSource>();
        
    }
    public void StartGame() {
        StartCoroutine(GameDelay(0.12f));
    }
    public void QuitGame() {
        StartCoroutine(QuitDelay(0.12f));
    }

    private IEnumerator  QuitDelay(float _time)
    {
        _aSource.PlayOneShot(_clip);
        yield return new WaitForSeconds(_time);
        Application.Quit();
    }
    private IEnumerator GameDelay(float _time)
    {
        _aSource.PlayOneShot(_clip);
        yield return new WaitForSeconds(_time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
