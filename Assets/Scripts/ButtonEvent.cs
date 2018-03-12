using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : Photon.PunBehaviour, IPunObservable {

    public int buttonId;
    public Sprite highlightedImg;
    public AudioClip SFX;
    public Sprite[] animationFrames;
    public float animationDelay;

    void Start() {

    }
    public void Clicked() {
        //Debug.Log(buttonId.ToString() + " was clicked");
        StartCoroutine(ClickedAnim(true));

        //GetComponent<AudioSource>().PlayOneShot(SFX);
        //GeniusManager.Instance.InputHandler(buttonId);
    }

    void ClickTest() { 
}


    [PunRPC]
    public void Highlight() { 
        Debug.Log("Highlight");
        //StartCoroutine(ChangeColor());

        StartCoroutine(ClickedAnim(false));

    }
    public IEnumerator ChangeColor() {
        Sprite originalImg = GetComponent<Image>().sprite;

        GetComponent<AudioSource>().PlayOneShot(SFX);
        GetComponent<Button>().enabled = false;
        GetComponent<Image>().sprite = highlightedImg;
        yield return new WaitForSeconds(.15f);
        GetComponent<Image>().sprite = originalImg;
        GetComponent<Button>().enabled = true;
    }
    public IEnumerator ClickedAnim(bool addInput) {
        GetComponent<AudioSource>().PlayOneShot(SFX);

        for (int i = 0; i < animationFrames.Length; i++) {
            GetComponent<Image>().sprite = animationFrames[i];
            yield return new WaitForSeconds(animationDelay);
        }
        if(addInput)
            GeniusManager.Instance.InputHandler(buttonId);

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            // We own this player: send the others our data
            //stream.SendNext(sequence);
            //stream.SendNext(currentGameState);
            //stream.SendNext(Instance.currentGameStateId);
        }
        else {
            // Network player, receive data
            //Instance.sequence = (List<ButtonId>)stream.ReceiveNext();
            //Instance.currentGameState = (GameState)stream.ReceiveNext();
            //SetGameState((int)stream.ReceiveNext());

        }
    }
}
