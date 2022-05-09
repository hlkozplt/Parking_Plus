using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bolumler_kod : MonoBehaviour
{
    GameObject bolum_1;

    public void bolum1()
    {
        SceneManager.LoadScene("Scenes/bolum_1");   //bu kod ile sahneyi yeniden yükledik

        Time.timeScale = 1.0f;
    }

}
