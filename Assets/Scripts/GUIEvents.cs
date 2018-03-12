using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEvents : MonoBehaviour {

    int showTutorial = 1;

    void Start() {
        if(PlayerPrefs.HasKey("ShowTutorial"))
            showTutorial = PlayerPrefs.GetInt("ShowTutorial");

        //showTutorial = 1;
    }
    public void SetGameToLocal() {
        GeniusManager.Instance.currentGameMode = GameMode.Local;
        GameObject.Find("CanvasGlobal").transform.Find("Game").gameObject.SetActive(true);
        GameObject.Find("CanvasGlobal").transform.Find("MenuPrePlay").gameObject.SetActive(false);
        GameObject.Find("CanvasGlobal").transform.Find("Game/Instruction").gameObject.SetActive(true);
        GeniusManager.Instance.currentGameState = GameState.GameStart;
    }

    public void Play() {
        if (PlayerPrefs.HasKey("ShowTutorial"))
            showTutorial = PlayerPrefs.GetInt("ShowTutorial");
        else
            showTutorial = 1;

        if (showTutorial == 1)
            GameObject.Find("TutorialManager").GetComponent<TutorialManager>().ShowTutorial();
        else
            GameObject.Find("CanvasGlobal").transform.Find("MenuPrePlay").gameObject.SetActive(true);

        GameObject.Find("CanvasGlobal").transform.Find("Menu").gameObject.SetActive(false);
    }

    public void Replay() {
        GeniusManager.Instance.currentGameState = GameState.GameStart;
        GameObject.Find("CanvasGlobal").transform.Find("GameOver").gameObject.SetActive(false);
    }

    public void Menu() {
        GameObject.Find("CanvasGlobal").transform.Find("Menu").gameObject.SetActive(true);
        GameObject.Find("CanvasGlobal").transform.Find("MenuPrePlay").gameObject.SetActive(false);
    }

    public void ErasePlayerPrefs() {
        PlayerPrefs.DeleteAll();
        Debug.Log("Erased Player Prefs");
    }
}
