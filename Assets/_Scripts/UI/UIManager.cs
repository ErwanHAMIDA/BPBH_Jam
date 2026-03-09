using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool _isMenuPanelActive = false;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _leftStick;

    public void ShowMenu()
    {
        _menuPanel.SetActive(true);
        _leftStick.SetActive(false);
    }

    public void HideMenu()
    {
        _menuPanel.SetActive(false);
        _leftStick.SetActive(true);
    }

    public void HandleMenuPanel()
    {
        if (_isMenuPanelActive)
        {
            HideMenu();
            _isMenuPanelActive = false;
        }
        else
        {
            ShowMenu();
            _isMenuPanelActive = true;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LV_Main");
    }
}
