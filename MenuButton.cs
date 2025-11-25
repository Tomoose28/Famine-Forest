using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color normalColor;
    public Color hoverColor;
    
    private Text text;
    public UIController uIController;

    
    
    void Start()
    {
        text = GetComponent<Text>();
        text.color = normalColor;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = normalColor;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnButtonClicked();
    }
    
    void OnButtonClicked()
    {
         text.color = normalColor; // Set this once at the top
    
        if (text.text == "START")
        {
            uIController.StartGame();
        }
        else if (text.text == "CONTROLS")
        {
            uIController.ControlMenu();
        }
        else if (text.text == "MAIN MENU")
        {
            uIController.ResetGame();
        }
        else if (text.text == "BACK")
        {
            uIController.Back();
        }
        else if (text.text == "RESUME")
        {
            uIController.Resume();
        }
        else if (text.text == "RESTART?") 
        {
            uIController.Restart();
        }
        else if (text.text == "PLAY AGAIN?") // Add this for win screen
        {
            uIController.ResetGame();
        }

        else if (text.text == "EXIT")
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}   
