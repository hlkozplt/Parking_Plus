using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ana_menu : MonoBehaviour
{
   

    public void baslat_btn()
    {
        SceneManager.LoadScene("Scenes/bolumler");
        Time.timeScale = 1.0f;
    }

    public void cikis_btn()
    {
        Application.Quit();
    }
}
