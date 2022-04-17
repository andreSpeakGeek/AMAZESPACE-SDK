using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToImagesTest : MonoBehaviour {

    public void Back() {
        SceneManager.LoadScene("ImagesTest");
    }
}
