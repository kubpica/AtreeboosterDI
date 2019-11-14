using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAdditivly : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("_additivly", LoadSceneMode.Additive);
    }
}
