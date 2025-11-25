using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject map;
    public PlayerMovement playerMovement;
    public GameObject hungerBar;
    public GameObject startMenu;
    public GameObject controlMenu;
    public GameObject deathMenu;
    public GameObject mainMenu;
    public GameObject winScreen;
    public float winPositionX;
    public HungerController hungerController;
    public Drawing drawing;
    private string previousMenu = "start";

    public Text tipText;
    public List<string> tips;
    void Start()
    {
        StartMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            map.gameObject.SetActive(!map.gameObject.activeInHierarchy);
            playerMovement.canMove = !playerMovement.canMove;
            Cursor.visible = !Cursor.visible;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !startMenu.activeInHierarchy && !controlMenu.activeInHierarchy)
        {
            mainMenu.SetActive(!mainMenu.activeInHierarchy);
            playerMovement.canMove = !playerMovement.canMove;
            Cursor.visible = !Cursor.visible;
        }

    }

    public void StartMenu()
    {
        previousMenu = "start";
        controlMenu.SetActive(false);
        mainMenu.SetActive(false);
        deathMenu.SetActive(false); 
        winScreen.SetActive(false);
        startMenu.SetActive(true);
        playerMovement.canMove = false;
        Cursor.visible = true;


    }

    public void ControlMenu()
    {
        previousMenu = GetCurrentMenu();
        startMenu.SetActive(false);
        mainMenu.SetActive(false);
        controlMenu.SetActive(true);
        playerMovement.canMove = false;
        Cursor.visible = true;
    }

    public void Resume()
    {
        mainMenu.SetActive(false);
        playerMovement.canMove = true;
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        previousMenu = GetCurrentMenu();
        startMenu.SetActive(false);
        controlMenu.SetActive(false);
        mainMenu.SetActive(true);
        playerMovement.canMove = false;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        playerMovement.canMove = true;
        hungerBar.SetActive(true);
        Cursor.visible = false;
    }

    public void Back()
    {
        if (previousMenu == "start")
        {
            StartMenu();
        }
        else if (previousMenu == "main")
        {
            MainMenu();
        }
        else if (previousMenu == "control")
        {
            ControlMenu();
        }
    }

    private string GetCurrentMenu()
    {
        if (startMenu.activeInHierarchy) return "start";
        if (controlMenu.activeInHierarchy) return "control";
        if (mainMenu.activeInHierarchy) return "main";
        return "start";
    }

    public void DeathMenu()
    {
        int n = Random.Range(0, tips.Count);
        tipText.text = tips[n];
        deathMenu.SetActive(true);
        playerMovement.canMove = false;
        Cursor.visible = true;
        hungerBar.SetActive(false); 
    }

    public void Restart()
    {
        deathMenu.SetActive(false);
        hungerController.Respawn();
        hungerBar.SetActive(true);
        playerMovement.canMove = true;
        Cursor.visible = false;
    }


    public void ResetGame()
    {
        
        playerMovement.transform.position = hungerController.spawnPoint.transform.position;
        
        
        hungerController.hunger = hungerController.maxHunger; 
        hungerController.hungerBar.fillAmount = 1f;
        
        
        playerMovement.lighting.ClearScreen();
        playerMovement.lighting.SetLight(playerMovement.transform.position);
        playerMovement.lighting.SetGroundLight(playerMovement.transform.position);
        playerMovement.canFlicker = true;

        drawing.ResetMap();
        
        StartMenu();
    }

    public void WinScreen()
    {
        winScreen.SetActive(true);
        playerMovement.canMove = false;
        Cursor.visible = true;
        hungerBar.SetActive(false);
    }
}
