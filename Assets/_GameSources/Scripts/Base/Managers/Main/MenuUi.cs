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
        _aSource.PlayOneShot(_clip);
        StartCoroutine(Delay(_clip.length));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }
    public void QuitGame() { 
        _aSource.PlayOneShot(_clip);
        StartCoroutine(Delay(_clip.length));
            Application.Quit(); 
    }

    private IEnumerator  Delay(float _time)
    {
        yield return new WaitForSeconds(_time);
    }
}
