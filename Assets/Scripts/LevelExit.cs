using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision Detected");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadNextLevel(currentSceneIndex));
        }
    }

    IEnumerator LoadNextLevel(int index)
    {
        yield return new WaitForSeconds(levelLoadDelay);
        int nextSceneIndex = index + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
        
    }
}
