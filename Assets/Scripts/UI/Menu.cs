using UnityEngine;

public class Menu : MonoBehaviour
{
    public void PlayNewGame()
    {
        EventHandler.CallStartNewGameEvent();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
