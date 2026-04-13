using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool _isMenuPanelActive = false;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _leftStick;
    [SerializeField] private TextMeshProUGUI _spellModeText;
    private bool _isSpellMode1 = true;
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

    public void UpdateSpellModeText()
    { 
        if (_isSpellMode1)
        {
            _spellModeText.text = "Spell Mode: 2";
            _isSpellMode1 = false;
        }
        else
        {
            _spellModeText.text = "Spell Mode: 1";
            _isSpellMode1 = true;
        }
    }
}
