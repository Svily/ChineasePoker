using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    private GameController controller;

    void Start()
    {
        //添加点击事件
        transform.Find("Start").gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate(GameStart));
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    /// <summary>
    /// 初始化游戏，加载游戏面板
    /// </summary>
    void GameStart()
    {
        controller.InitInteraction();
        controller.InitGame();
        controller.InitMenu();
        Destroy(this.gameObject);
    }



}
