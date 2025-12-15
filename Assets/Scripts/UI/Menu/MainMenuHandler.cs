using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject MainScreen;
    [SerializeField] private GameObject OptionsScreen;
    [SerializeField] private GameObject QuitConfirmationScreen;
    
    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;
    private float uiVolume;
    
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenMainMenu()
    {
        MainScreen.SetActive(true);
        OptionsScreen.SetActive(false);
        QuitConfirmationScreen.SetActive(false);
    }

    public void OpenOptions()
    {
        MainScreen.SetActive(false);
        OptionsScreen.SetActive(true);
    }

    public void QuitConfirmation()
    {
        MainScreen.SetActive(false);
        QuitConfirmationScreen.SetActive(true);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
