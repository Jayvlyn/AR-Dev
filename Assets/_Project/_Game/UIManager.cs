using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject scanPanel;
    public GameObject gamePanel;
    public GameObject endPanel;


    #region BUTTON EVENTS
    public void OnStartClicked() // start button on main menu that will advance player to scan screen where they scan environment before starting actual game
    {
        ShowScanPanel();
    }

    public void OnConfirmScanClicked() // on scan screen to finish scanning and start game
    {
        ShowGamePanel();
    }

    public void OnRestartClicked() // button on end screen to play again with same setup
    {
        ShowGamePanel();
    }

    public void OnMainMenuClicked() // button on end screen to send back to main menu so player can rescan
    {
        ShowStartPanel();
    }
    #endregion

    private void ShowStartPanel()
    {
        startPanel.SetActive(true);
        scanPanel.SetActive(false);
        gamePanel.SetActive(false);
        endPanel.SetActive(false);
    }

    private void ShowScanPanel()
    {
        startPanel.SetActive(false);
        scanPanel.SetActive(true);
        gamePanel.SetActive(false);
        endPanel.SetActive(false);
    }

    private void ShowGamePanel()
    {
        startPanel.SetActive(false);
        scanPanel.SetActive(false);
        gamePanel.SetActive(true);
        endPanel.SetActive(false);
    }

    private void ShowEndPanel()
    {
        startPanel.SetActive(false);
        scanPanel.SetActive(false);
        gamePanel.SetActive(false);
        endPanel.SetActive(true);
    }
}
