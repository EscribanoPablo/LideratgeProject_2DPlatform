using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : PlayerDetector
{
    protected override void DoSomething()
    {
        SceneManager.LoadScene("MainMenú_Portada");
    }
}
