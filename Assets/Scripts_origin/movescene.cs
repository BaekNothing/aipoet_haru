using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movescene : MonoBehaviour
{
    public void getnext()
    {
        SceneManager.LoadScene(1);
    }

    public void getback()
    {
        SceneManager.LoadScene(0);
    }
}
