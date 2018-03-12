using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {
    Login,
    GameStart,
    RepeatSequence,
    WaitingForSequenceInput,
    WaitingForNewInput,
    WaitingForNextPlayer,
    PlaySequence,
    GameOver
}

public enum GameMode {
    Local,
    Multiplayer
}

public class GeniusManager : Photon.PunBehaviour, IPunObservable {

    public static GeniusManager Instance;                                   // Singleton

    [HideInInspector]
    public GameState currentGameState;                                      // GameState Manager
    [HideInInspector]
    public GameMode currentGameMode;
    public float SequenceInputLimit;                                        // Input Limit in seconds to wait for player insert sequence

    private GameObject[] buttons = new GameObject[6];
    private List<int> sequence = new List<int>();

    /** Information to send via Photon **/
    private int currentGameStateId = 0;
    private bool clicked = false;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        buttons[0] = GameObject.Find("CanvasGlobal").transform.Find("Game/Button1").gameObject;
        buttons[1] = GameObject.Find("CanvasGlobal").transform.Find("Game/Button2").gameObject;
        buttons[2] = GameObject.Find("CanvasGlobal").transform.Find("Game/Button3").gameObject;
        buttons[3] = GameObject.Find("CanvasGlobal").transform.Find("Game/Button4").gameObject;
        buttons[4] = GameObject.Find("CanvasGlobal").transform.Find("Game/Button5").gameObject;
        buttons[5] = GameObject.Find("CanvasGlobal").transform.Find("Game/Button6").gameObject;
    }

    void Start() {
        //currentGameState = GameState.Login;
        SetGameState(1);

    }
    void OnEnable () {

        
    }
	
	// Update is called once per frame
	void Update () {

    }

    int sequenceCounter = 0;

    public void InputHandler(int buttonId) {
        // Waiting for sequence 
        if (currentGameState == GameState.WaitingForSequenceInput) {
            //Debug.Log("Checking if input is correct");
            // Check if counter is in sequence range
            if (sequenceCounter <= sequence.Count) {
                // Player input is correct
                if (buttonId == sequence[sequenceCounter]) {
                    //Debug.Log("Input is correct");
                    // Go to next button in sequence
                    sequenceCounter++;
                    if(sequenceCounter == sequence.Count) {
                        //GameObject.Find("Instruction").GetComponent<Text>().text = "Waiting for new Input";
                        if (currentGameMode == GameMode.Local)
                            SetInstructionText("Waiting for new Input");
                        //currentGameState = GameState.WaitingForNewInput;
                        SetGameState(5);
                        sequenceCounter = 0;
                    }
                }
                // Player is wrong
                else {
                    // Game Over
                    //GameObject.Find("Instruction").GetComponent<Text>().text = "Game Over";
                    if (currentGameMode == GameMode.Local)
                        SetInstructionText("Waiting for Input");

                    //currentGameState = GameState.GameOver;
                    SetGameState(8);
                    sequenceCounter = 0;
                    SetFeedbackText(true, "Game Over");

                    foreach (GameObject btn in buttons)
                        btn.GetComponent<Button>().interactable = false;
                }
            }
        }
        // Waiting for New Input 
        else if (currentGameState == GameState.WaitingForNewInput) {
            // New Input
            //Debug.Log("New button added");
            sequence.Add(buttonId);
            //currentGameState = GameState.WaitingForNextPlayer;
            SetGameState(6);
            if (currentGameMode == GameMode.Multiplayer) {
                photonView.RPC("SendList", PhotonTargets.Others, (int[])sequence.ToArray());
                //GameObject.Find("Instruction").GetComponent<Text>().text = "Waiting for Next Player";
                SetFeedbackText(true, "Waiting for Next Player");

                GameObject.Find("ActionButton").GetComponent<Button>().interactable = false;
            } else if (currentGameMode == GameMode.Local)
                SetInstructionText("Waiting for Next Player");
            foreach (GameObject btn in buttons)
                btn.GetComponent<Button>().interactable = false;

        }
    }

    IEnumerator RepeatSequence() {
        foreach (GameObject btn in buttons)
            btn.GetComponent<Button>().interactable = true;

        yield return new WaitForSeconds(.2f);

        //Debug.Log("Repeating Sequence");
        //GameObject.Find("Instruction").GetComponent<Text>().text = "Repeating Sequence";
        if (currentGameMode == GameMode.Local)
            SetInstructionText("Repeating Sequence");
        for (int currentButton = 0; currentButton < sequence.Count; currentButton++) {
            //Debug.Log("Looking for " + sequence[currentButton].ToString());
            GameObject.Find("Button" +sequence[currentButton].ToString()).GetComponent<ButtonEvent>().Highlight();
            yield return new WaitForSeconds(.35f);
        }

        //currentGameState = GameState.WaitingForSequenceInput;
        SetGameState(4);
        //GameObject.Find("Instruction").GetComponent<Text>().GetComponent<Text>().text = "Waiting for Sequence";
        if (currentGameMode == GameMode.Local)
            SetInstructionText("Waiting for Sequence");
    }

    void SetGameState(int i) {

        currentGameStateId = i;
        switch (i) {
            case 1:
                currentGameState = GameState.Login;
                break;
            case 2:
                currentGameState = GameState.GameStart;
                break;
            case 3:
                currentGameState = GameState.RepeatSequence;
                break;
            case 4:
                currentGameState = GameState.WaitingForSequenceInput;
                break;
            case 5:
                currentGameState = GameState.WaitingForNewInput;
                break;
            case 6:
                currentGameState = GameState.WaitingForNextPlayer;
                break;
            case 7:
                currentGameState = GameState.PlaySequence;
                break;
            case 8:
                currentGameState = GameState.GameOver;
                break;
        }
    }

    public void ActionButton() {

        if (currentGameState == GameState.GameStart || currentGameState == GameState.GameOver) {
             clicked = false;
             sequence.Clear();
             //currentGameState = GameState.WaitingForNewInput;
             SetGameState(5);
             foreach (GameObject btn in buttons)
                btn.GetComponent<Button>().interactable = true;

            //GameObject.Find("Instruction").GetComponent<Text>().text = "Waiting for Input";
            if(currentGameMode == GameMode.Local)
                SetInstructionText("Waiting for Input");
        }

        // Waiting for next player
        if (currentGameState == GameState.WaitingForNextPlayer) {
            SetGameState(3);
            //currentGameState = GameState.RepeatSequence;
            StartCoroutine(RepeatSequence());
        }

    }

    public void SetFeedbackText(bool setActive) {
        GameObject.Find("CanvasGlobal").transform.Find("TextFeedback").gameObject.SetActive(setActive);
    }
    public void SetFeedbackText(bool setActive, string feedback) {
        GameObject.Find("CanvasGlobal").transform.Find("TextFeedback/Text").gameObject.GetComponent<Text>().text = feedback;

        if(currentGameState == GameState.GameOver)
            GameObject.Find("CanvasGlobal").transform.Find("GameOver").gameObject.SetActive(true);
        else
            GameObject.Find("CanvasGlobal").transform.Find("TextFeedback").gameObject.SetActive(setActive);
        //GameObject.Find("CanvasGlobal").transform.Find("GameOver").gameObject.SetActive(false);
    }
    void SetInstructionText(string txt) {
        GameObject.Find("CanvasGlobal").transform.Find("Game/Instruction").gameObject.GetComponent<Text>().text = txt;
    }

    [PunRPC]
    void SendList(int[] myarray) {
        Debug.Log("Received an array of size: " + myarray.Length);
        sequence.Clear();
        sequence.AddRange(myarray);

        SetGameState(6);
        Handheld.Vibrate();
        //GameObject.Find("Instruction").GetComponent<Text>().text = "It's your turn";
        SetFeedbackText(false);
        GameObject.Find("ActionButton").GetComponent<Button>().interactable = true;

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {

        }
        else {

        }
    }
}
