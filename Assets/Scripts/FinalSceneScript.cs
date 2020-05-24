using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalSceneScript : MonoBehaviour
{
    public GameObject mainCharacter;
    public GameObject alice;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("CharacterRecord"))
        {
            if (PlayerPrefs.GetInt("CharacterRecord") == 0)
            {
                alice.SetActive(true);
                mainCharacter.SetActive(false);
            }
            else
            {
                alice.SetActive(false);
                mainCharacter.SetActive(true);
            }
        }
        else
        {
            alice.SetActive(false);
            mainCharacter.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
