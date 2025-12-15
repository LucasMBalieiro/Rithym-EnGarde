using Audio;
using UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class HudController : MonoBehaviour
{
    [SerializeField] private GameObject gameplayHud;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    
    private bool isMuffled = false;

    private void OnEnable()
    {
        UIActions.OnOpenCloseUI += OpenPauseMenu;
    }

    private void OnDisable()
    {
        UIActions.OnOpenCloseUI -= OpenPauseMenu;
    }

    public void OpenGameplayHud()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameplayHud.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        gameplayHud.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void OpenOptionsMenu()
    {
        gameplayHud.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //DEBUG
    public void ToogleSnapshot()
    {
        if (isMuffled)
        {
            SnapshotActions.SetDefaultFilter.Invoke();
            isMuffled = false;
        }
        else
        {
            SnapshotActions.SetMuffledFilter.Invoke();
            isMuffled = true;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
