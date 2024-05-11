using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game.Core
{
    public class CommonFunctions : MonoBehaviour
    {
        public void ExitGame()
        {
            Debug.Log("Exiting Application");
            Application.Quit();
        }
    }

}
