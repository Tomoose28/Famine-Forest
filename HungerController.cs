using UnityEngine;
using UnityEngine.UI;

public class HungerController : MonoBehaviour
{
    public Image hungerBar;
    public int hunger;
    public int maxHunger;
    public Transform spawnPoint;
    public GameObject player;
    public Lighting lighting;
    public UIController uIController;

    public PlayerMovement playerMovement;
    void Start()
    {
        hunger = maxHunger;
    }

    void Update()
    {
        
    }

    public void Starve(int amount)
    {
        hunger -= amount;
        hungerBar.fillAmount = hunger /100f;
        if (hunger <= 0)
        {
            hunger = 0;
            Death();
        }
    }

    public void Feed(int amount)
    {
        hunger += amount;
        hunger = Mathf.Clamp(hunger, 0, maxHunger);
        hungerBar.fillAmount = hunger /100f;
    }

    public void Death()
    {
        uIController.DeathMenu();
    }

     public void Respawn()
    {
        // This is what happens when clicking "Restart" on death screen
        player.transform.position = spawnPoint.position;
        lighting.ClearScreen();
        Feed(maxHunger);
        lighting.SetLight(player.transform.position);
        lighting.SetGroundLight(player.transform.position);
        playerMovement.canFlicker = true;
    }

    public void Check()
    {
        string tileType = lighting.CheckTile(player.transform.position);
        if (tileType == "berry")
        {
            Feed(33);
        }
        if (tileType == "mushroom")
        {
            Starve(40);
        }
    }
}
