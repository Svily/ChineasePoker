using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 交互面板
/// </summary>
public class Interaction : MonoBehaviour
{
    //发牌
    private GameObject deal;
    //出牌
    private GameObject playCards;
    //不出
    private GameObject disPlayCards;
    //抢地主
    private GameObject callLandlord;
    //不抢地主
    private GameObject disCallLandlord;
    //菜单
    private GameObject menu;
    //不出提示
    private GameObject tip;

    private GameController controller;
    //开启托管按钮
    private GameObject autoPlay;
    //关闭托管按钮
    private GameObject disAutoPlay;
    //玩家AI
    private AI playerAI;
  

    void Start()
    {
        InitComponets();
    }

    private void Update()
    {
        //游戏中，玩家回合外才能启用托管
        if (disAutoPlay.activeSelf || OrderController.Instance.currentAuthority == CharacterType.Player || OrderController.Instance.currentAuthority == CharacterType.Desk)
        {
            autoPlay.SetActive(false);
        }
        else
        {
            autoPlay.SetActive(true);
        }
    }

    void InitComponets()
    {
        //初始化按钮
        deal = gameObject.transform.Find("Deal").gameObject;
        playCards = gameObject.transform.Find("PlayCards").gameObject;
        disPlayCards = gameObject.transform.Find("DisPlayCards").gameObject;
        callLandlord = gameObject.transform.Find("CallLandlord").gameObject;
        disCallLandlord = gameObject.transform.Find("DisCallLandlord").gameObject;
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        menu = gameObject.transform.Find("Menu").gameObject;
        tip = GameObject.Find("GamePanel").transform.Find("Player").transform.Find("Tip").gameObject;
        autoPlay = gameObject.transform.Find("AutoPlay").gameObject;
        disAutoPlay = gameObject.transform.Find("DisAutoPlay").gameObject;
        playerAI = GameObject.Find("GamePanel").transform.Find("Player").GetComponent<AI>();

        //添加委托事件
        deal.GetComponent<UIButton>().onClick.Add(new EventDelegate(Deal));
        playCards.GetComponent<UIButton>().onClick.Add(new EventDelegate(PlayCards));
        disPlayCards.GetComponent<UIButton>().onClick.Add(new EventDelegate(DisPlayCards));
        callLandlord.GetComponent<UIButton>().onClick.Add(new EventDelegate(CallLandlord));
        disCallLandlord.GetComponent<UIButton>().onClick.Add(new EventDelegate(DisCallLandlord));
        menu.GetComponent<UIButton>().onClick.Add(new EventDelegate(Menu));
        autoPlay.GetComponent<UIButton>().onClick.Add(new EventDelegate(AutoPlay));
        disAutoPlay.GetComponent<UIButton>().onClick.Add(new EventDelegate(DisAutoPlay));
        
        //绑定激活按钮事件
        OrderController.Instance.activeButton += ActiveCardButton;

        //初始禁用各按钮
        playCards.SetActive(false);
        disPlayCards.SetActive(false);
        callLandlord.SetActive(false);
        disCallLandlord.SetActive(false);
        menu.GetComponent<UIButton>().isEnabled = true;
        autoPlay.SetActive(false);
        disAutoPlay.SetActive(false);
    }

    /// <summary>
    /// 激活出牌按钮
    /// </summary>
    /// <param name="canPlay"></param>
    void ActiveCardButton(bool canPlay)
    {
        playCards.SetActive(true);
        disPlayCards.SetActive(true);
        tip.SetActive(false);
        disPlayCards.GetComponent<UIButton>().isEnabled = canPlay;
    }

    /// <summary>
    /// 发牌
    /// </summary>
    public void Deal()
    {
        controller.DealCards();
        callLandlord.SetActive(true);
        disCallLandlord.SetActive(true);
        deal.SetActive(false);
        
    }

    /// <summary>
    /// 出牌
    /// </summary>
    void PlayCards()
    {
        PlayCard playCard = GameObject.Find("Player").GetComponent<PlayCard>();
        if (playCard.CheckSelectCards())
        {
            playCards.SetActive(false);
            disPlayCards.SetActive(false);
        }
    }

    /// <summary>
    /// 不出
    /// </summary>
    void DisPlayCards()
    {
       
        OrderController.Instance.Turn();
        playCards.SetActive(false);
        disPlayCards.SetActive(false);
        tip.SetActive(true);
    }

    /// <summary>
    /// 抢地主
    /// </summary>
    void CallLandlord()
    {
        //玩家的地主
        controller.DealDeskCard(CharacterType.Player);
        OrderController.Instance.Init(CharacterType.Player);
        callLandlord.SetActive(false);
        disCallLandlord.SetActive(false);
    }

    /// <summary>
    /// 不抢
    /// </summary>
    void DisCallLandlord()
    {
        //随机产生地主
        int index = Random.Range(2, 4);
        controller.DealDeskCard((CharacterType)index);
        OrderController.Instance.Init((CharacterType)index);
        callLandlord.SetActive(false);
        disCallLandlord.SetActive(false);
    }

    /// <summary>
    /// 菜单按钮
    /// </summary>
    void Menu()
    {
        GameObject menuPanel = UICamera.mainCamera.transform.Find("MenuPanel").gameObject;

        menuPanel.SetActive(true);

        this.gameObject.SetActive(false);

        Time.timeScale = 0;
    }

    /// <summary>
    /// 玩家托管
    /// </summary>
    void AutoPlay()
    {
        //解绑按钮事件
        OrderController.Instance.activeButton -= ActiveCardButton;
        disAutoPlay.SetActive(true);
        playerAI.PlayerAutoPlay();
        autoPlay.SetActive(false);
    }

    /// <summary>
    /// 取消托管
    /// </summary>
    void DisAutoPlay()
    {
        //恢复绑定
        OrderController.Instance.activeButton += ActiveCardButton;
        autoPlay.SetActive(true);
        playerAI.DisAutoPlay();
        disAutoPlay.SetActive(false);
    }

}
