using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [Range(0, 1)]
    public float rotateSpeed;

    public GameObject alice;
    public GameObject mainCharacter;
    public Dropdown selectCharacter;

    private int _character = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        alice.transform.LookAt(transform.position);
        mainCharacter.transform.LookAt(transform.position);

        if (PlayerPrefs.HasKey("CharacterRecord"))
        {
            selectCharacter.value = PlayerPrefs.GetInt("CharacterRecord");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed, Space.World);
    }

    public void ChangeScene(int id)
    {
        PlayerPrefs.SetInt("CharacterRecord", _character);
        PlayerPrefs.Save();
        SceneManager.LoadScene(id);
    }

    public void ChangeCharacter()
    {
        // 0 is alice, 1 is main character
        _character = selectCharacter.value;
        if (_character == 0)
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
}
