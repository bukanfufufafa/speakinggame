using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditPanel;
    private bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (creditPanel != null)
        {
            creditPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pause == false)
        {
            pause = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pause == true)
        {
            pause = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void back()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        pause = false;
    }

    public void openSettings()
    {
        settingsPanel.SetActive(true);

    }

    public void closeSettings()
    {
        settingsPanel.SetActive(false);

    }

    public void mainMenu()
    {
        SceneManager.LoadScene("mainmenu");
    }

    public void restart()
    {
        pause = false ;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("tes");
    }
}
