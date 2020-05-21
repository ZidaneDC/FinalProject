using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonManager : MonoBehaviour
{
    public void PlayButton(string startScene)
    {
        SceneManager.LoadScene(startScene);
    }
}
