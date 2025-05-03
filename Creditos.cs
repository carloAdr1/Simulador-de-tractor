using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    public AudioSource sfxAlgo;
    public AudioClip sfxboton;

    public void Credits()
    {
        StartCoroutine(PlaySoundAndLoad(5));
    }

    private IEnumerator PlaySoundAndLoad(int sceneIndex)
    {
        sfxAlgo.PlayOneShot(sfxboton);
        yield return new WaitForSeconds(sfxboton.length);
        SceneManager.LoadScene(sceneIndex);
    }
}

