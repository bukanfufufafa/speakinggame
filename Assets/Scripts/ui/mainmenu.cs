using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditPanel;
   
    // Start is called before the first frame update
    void Start()
    {
        settingsPanel.SetActive(false);
        creditPanel.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newGame()
    {
        SceneManager.LoadScene("loading");
    }

    public void openSettings()
    {
        settingsPanel.SetActive(true);
        
    }

    public void closeSettings()
    {
        settingsPanel.SetActive(false);
        
    }

    public void openCredit()
    {
        creditPanel.SetActive(true);
        
    }

    public void closeCredit()
    {
        
        creditPanel.SetActive(false);
    }

    public void quit()
    {
        Application.Quit();
    }
}
