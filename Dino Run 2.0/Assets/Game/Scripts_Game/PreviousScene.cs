using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviousScene : MonoBehaviour
{
    public void GoBack()
    {
        SceneManager.LoadScene("Reset");
    }
}
