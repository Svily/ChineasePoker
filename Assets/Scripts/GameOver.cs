using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    GameObject exit;

	void Start () {
        exit = transform.Find("Exit").gameObject;
        exit.transform.GetComponent<UIButton>().onClick.Add(new EventDelegate(Exit));
	}

    void Exit()
    {
        Application.Quit();
    }
	
}
