using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    private GameObject backGame;
    private GameObject exit;
    private GameObject interactionPanel;
	void Start () {

        backGame = transform.Find("BackGame").gameObject;
        exit = transform.Find("Exit").gameObject;
        interactionPanel = UICamera.mainCamera.gameObject.transform.Find("InteractionPanel").gameObject;

        backGame.GetComponent<UIButton>().onClick.Add(new EventDelegate(BackGame));
        exit.GetComponent<UIButton>().onClick.Add(new EventDelegate(Exit));
    }
	
    void BackGame()
    {
        Time.timeScale = 1;
        interactionPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void Exit()
    {
        Application.Quit();
    }
	
}
