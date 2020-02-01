using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //Reference
    [SerializeField]
    private GameObject player;


    private GameObject healthBar;
    private GameObject loseMenuPanel;
    private GameObject winMenuPanel;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            healthBar = transform.Find("Health Bar").gameObject;
            loseMenuPanel = transform.Find("Lose Button Group").gameObject;
            winMenuPanel = transform.Find("Win Button Group").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(10, player.GetComponent<PlayerController>().playerHealth);
    }

    public void showGameOverMenu()
    {
        Time.timeScale = 0;
        loseMenuPanel.SetActive(true);
    }

    public void showGameWinMenu()
    {
        Time.timeScale = 0;
        winMenuPanel.SetActive(true);
        
    }

    public void loadMainGame()
    {
        SceneManager.LoadScene("HeshScene");
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void quit()
    {
        Application.Quit();
    }
}
