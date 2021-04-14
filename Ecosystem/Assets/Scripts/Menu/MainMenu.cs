using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{

  public class MainMenu : MonoBehaviour
  {
    public void PlayGame()
    {
      SceneManager.LoadScene(OptionsMenu.World);
    }

    public void QuitGame()
    {
      Application.Quit();
    }

  }
}