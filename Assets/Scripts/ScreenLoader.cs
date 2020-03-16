using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenLoader : MonoBehaviour
{
    [SerializeField] float delaySeconds = 3.5f;
    int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitAndLoad());
        }
    }
    private IEnumerator WaitAndLoad()
    {

        yield return new WaitForSeconds(delaySeconds);
        GetComponent<Animator>().SetTrigger("To_Fade_OUT");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
