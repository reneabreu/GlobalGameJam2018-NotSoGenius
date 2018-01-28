using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEvents : MonoBehaviour {

    public void SetGameToLocal() {
        GeniusManager.Instance.currentGameMode = GameMode.Local;
        GameObject.Find("CanvasGlobal").transform.Find("Game").gameObject.SetActive(true);
        GameObject.Find("CanvasGlobal").transform.Find("MenuPrePlay").gameObject.SetActive(false);
        GameObject.Find("CanvasGlobal").transform.Find("Game/Instruction").gameObject.SetActive(true);
        GeniusManager.Instance.currentGameState = GameState.GameStart;
    }

    public void Play() {
        GameObject.Find("CanvasGlobal").transform.Find("MenuPrePlay").gameObject.SetActive(true);
        GameObject.Find("CanvasGlobal").transform.Find("Menu").gameObject.SetActive(false);
    }

    public void Replay() {
        GeniusManager.Instance.currentGameState = GameState.GameStart;
        GameObject.Find("CanvasGlobal").transform.Find("GameOver").gameObject.SetActive(false);
    }
}
