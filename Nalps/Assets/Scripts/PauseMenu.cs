using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject statsMenu;
    public GameObject inventoryMenu;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        PlayerScript playerData = GameObject.Find("PlayerNalp").GetComponent<PlayerScript>();
        Player player = playerData.player;
        StatsDisplay display = statsMenu.GetComponent<StatsDisplay>();
        display.setLabels(player.Hp, player.Strength, player.Resistance, player.Speed, player.Level);

    }

    public void StatsMenu()
    {
        Debug.Log("Here");
        if (!statsMenu.activeSelf)
        {
            statsMenu.SetActive(true);
            inventoryMenu.SetActive(false);
        }
    }

    public void InventoryMenu()
    {
        if (!inventoryMenu.activeSelf)
        {
            inventoryMenu.SetActive(true);
            statsMenu.SetActive(false);
        }
    }
}
