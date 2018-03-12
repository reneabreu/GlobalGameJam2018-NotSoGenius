using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	public void ShowTutorial() {
        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial() {
        PlayerPrefs.SetInt("ShowTutorial", 0);

        GameObject.Find("CanvasGlobal").transform.Find("Tutorial").gameObject.SetActive(true);

        yield return new WaitForSeconds(GameObject.Find("CanvasGlobal").transform.Find("Tutorial").gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.5f);


        GameObject.Find("CanvasGlobal").transform.Find("MenuPrePlay").gameObject.SetActive(true);
        GameObject.Find("CanvasGlobal").transform.Find("Tutorial").gameObject.SetActive(false);
    }
}
