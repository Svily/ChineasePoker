using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //底分
    private int baseScore;
    //倍数
    private int multiple;
    //欢乐豆数
    private int playerBeans;
    private int AI1Beans;
    private int AI2Beans;
    //游戏面板
    GameObject gamePanel;
    //倍数提示
    GameObject multipleTip;
    UILabel multipleTipLable;

    //玩家手牌
    HandCards player;
    HandCards computerOne;
    HandCards computerTwo;

    //结束结算相关
    GameObject gameOver;
    GameObject playerOverInfo;
    GameObject computerOneOverInfo;
    GameObject computerTwoOverInfo;
    GameObject pokerPrefab;
    UISprite pokerSprite;
    void Start()
    {
        //初始化底分，倍数，玩家欢乐豆数
        baseScore = 30;
        multiple = 15;
        //初始化欢乐豆数
        playerBeans = 3000;
        AI1Beans = 3000;
        AI2Beans = 3000;

        InitStart();
    }

    public void InitStart()
    {
        pokerPrefab = Resources.Load("poker") as GameObject;
        //挂载脚本
        GameObject.Find("StartPanel").AddComponent<StartGame>();
    }

    /// <summary>
    /// 加倍
    /// </summary>
    public void DoubleMultiple()
    {
        multiple *= 2;
        multipleTipLable.text = "当前倍数:" + multiple;
    }

    /// <summary>
    /// 获取地主
    /// </summary>
    /// <returns></returns>
    HandCards GetLandLord()
    {
        if(player.AccessIdentity == Identity.Landlord)
        {
            return player;
        }else if (computerOne.AccessIdentity == Identity.Landlord)
        {
            return computerOne;
        }
        return computerTwo;
    }

    /// <summary>
    /// 计算最终得分
    /// </summary>
    /// <param name="landlord"></param>
    /// <param name="isLandlordWin"></param>
    void CalculateScore(HandCards landlord, bool isLandlordWin)
    {
        int score = baseScore * multiple;
        //地主得分
        int landlordScore = 0;
        //农民得分
        int famerScore = 0;
        //玩家得分
        int playScore = 0;
        int AI1Score = 0;
        int AI2Score = 0;
        //根据是否是地主胜利更新得分
        if (isLandlordWin)
        {
            landlordScore = +score;
            famerScore = -(score / 2);
        }
        else
        {
            landlordScore = -score;
            famerScore = +(score / 2);
        }

        //根据身份设置各玩家得分
        if (landlord.cType == CharacterType.Player)
        {
            playScore = landlordScore;
            AI1Score = famerScore;
            AI2Score = famerScore;
        }

        if (landlord.cType == CharacterType.ComputerOne)
        {
            playScore = famerScore;
            AI1Score = landlordScore;
            AI2Score = famerScore;
        }

        if (landlord.cType == CharacterType.ComputerTwo)
        {
            playScore = famerScore;
            AI1Score = famerScore;
            AI2Score = landlordScore;
        }

        //计算欢乐豆
        playerBeans += playScore;
        AI1Beans += AI1Score;
        AI2Beans += AI2Score;

        string playerFinal = playScore > 0 ? "+" + playScore.ToString() : playScore.ToString();
        string AI1Final = AI1Score > 0 ? "+" + AI1Score.ToString() : AI1Score.ToString();
        string AI2Final = AI2Score > 0 ? "+" + AI2Score.ToString() : AI2Score.ToString();

        //显示底分信息
        playerOverInfo.transform.Find("BaseScore").transform.GetComponent<UILabel>().text = baseScore.ToString();
        computerOneOverInfo.transform.Find("BaseScore").transform.GetComponent<UILabel>().text = baseScore.ToString();
        computerTwoOverInfo.transform.Find("BaseScore").transform.GetComponent<UILabel>().text = baseScore.ToString();

        //显示倍数信息
        playerOverInfo.transform.Find("Multiple").transform.GetComponent<UILabel>().text = multiple.ToString();
        computerOneOverInfo.transform.Find("Multiple").transform.GetComponent<UILabel>().text = multiple.ToString();
        computerTwoOverInfo.transform.Find("Multiple").transform.GetComponent<UILabel>().text = multiple.ToString();

        //显示结算得分信息
        playerOverInfo.transform.Find("Final").transform.GetComponent<UILabel>().text = playerFinal;
        computerOneOverInfo.transform.Find("Final").transform.GetComponent<UILabel>().text = AI1Final;
        computerTwoOverInfo.transform.Find("Final").transform.GetComponent<UILabel>().text = AI2Final;

        //更新欢乐豆数量显示
        UpdateBeans();
    }

    /// <summary>
    /// 欢乐豆显示
    /// </summary>
    public void UpdateBeans()
    {
        gamePanel.transform.Find("Player").transform.Find("HappyBeans").transform.GetComponent<UILabel>().text = "欢乐豆:" + playerBeans.ToString();
        gamePanel.transform.Find("ComputerOne").transform.Find("HappyBeans").transform.GetComponent<UILabel>().text = "欢乐豆:" + AI1Beans.ToString();
        gamePanel.transform.Find("ComputerTwo").transform.Find("HappyBeans").transform.GetComponent<UILabel>().text = "欢乐豆:" + AI2Beans.ToString();
    }

    /// <summary>
    /// 初始化游戏面板
    /// </summary>
    public void InitGame()
    {
        //关闭开始面板
        GameObject.Find("StartPanel").SetActive(false);

        //加载游戏桌面
        gamePanel = NGUITools.AddChild(UICamera.mainCamera.gameObject, Resources.Load("GamePanel") as GameObject);
        gamePanel.name = gamePanel.name.Replace("(Clone)", "");

        //挂载脚本
        gamePanel.transform.Find("Player").gameObject.AddComponent<PlayCard>();
        gamePanel.transform.Find("Player").gameObject.AddComponent<HandCards>();
        gamePanel.transform.Find("Player").gameObject.AddComponent<AI>();

        gamePanel.transform.Find("ComputerOne").gameObject.AddComponent<HandCards>();
        gamePanel.transform.Find("ComputerOne").gameObject.AddComponent<AI>();

        gamePanel.transform.Find("ComputerTwo").gameObject.AddComponent<HandCards>();
        gamePanel.transform.Find("ComputerTwo").gameObject.AddComponent<AI>();

        //倍数
        multipleTip = GameObject.Find("GamePanel").transform.Find("Desk").transform.Find("MultipleTip").gameObject;
        multipleTipLable = multipleTip.GetComponent<UILabel>();

        //获取玩家手牌
        player = GameObject.Find("GamePanel").transform.Find("Player").transform.GetComponent<HandCards>();
        computerOne = GameObject.Find("GamePanel").transform.Find("ComputerOne").transform.GetComponent<HandCards>();
        computerTwo = GameObject.Find("GamePanel").transform.Find("ComputerTwo").transform.GetComponent<HandCards>();

        //初始化玩家身份
        gamePanel.transform.Find("Player").GetComponent<HandCards>().cType = CharacterType.Player;
        //初始化电脑身份
        gamePanel.transform.Find("ComputerOne").GetComponent<HandCards>().cType = CharacterType.ComputerOne;
        gamePanel.transform.Find("ComputerTwo").GetComponent<HandCards>().cType = CharacterType.ComputerTwo;

    }

    /// <summary>
    /// 加载交互面板
    /// </summary>
    public void InitInteraction()
    {
        GameObject iteraction = NGUITools.AddChild(UICamera.mainCamera.gameObject, (GameObject)Resources.Load("InteractionPanel"));
        iteraction.AddComponent<Interaction>();
        iteraction.name = iteraction.name.Replace("(Clone)", "");
    }

    /// <summary>
    /// 加载菜单面板
    /// </summary>
    public void InitMenu()
    {
        GameObject menu = NGUITools.AddChild(UICamera.mainCamera.gameObject, (GameObject)Resources.Load("MenuPanel"));
        menu.AddComponent<Menu>();
        menu.name = menu.name.Replace("(Clone)", "");
        menu.SetActive(false);
    }


    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver(HandCards handCards)
    {
        GameObject.Find("InteractionPanel").SetActive(false);
        //加载结束面板
        gameOver = NGUITools.AddChild(UICamera.mainCamera.gameObject, Resources.Load("GameOverPanel") as GameObject);
        gameOver.name = gameOver.name.Replace("(Clone)", "");
        gameOver.AddComponent<GameOver>();

        playerOverInfo = gameOver.transform.Find("Player").gameObject;
        computerOneOverInfo = gameOver.transform.Find("ComputerOne").gameObject;
        computerTwoOverInfo = gameOver.transform.Find("ComputerTwo").gameObject;
        

        //显示提示信息
        UILabel tip = gameOver.transform.Find("Tip").GetComponent<UILabel>();

        if(handCards.AccessIdentity == Identity.Farmer)
        {
            CalculateScore(GetLandLord(), false);
            tip.text = "农民获得胜利";
        }
        else if (handCards.AccessIdentity == Identity.Landlord)
        {
            CalculateScore(handCards, true);
            tip.text = "地主获得胜利";
        }
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame()
    {
        //销毁sprite
        DestroyAllSprites();
        //收牌
        BackToDeck();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        //洗牌
        Deck.Instance.Shuffle();
        //发牌
        Destroy(GameObject.Find("GameOverPanel"));
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    public void DealCards()
    {
        CharacterType currentCharacter = CharacterType.Player;

        //洗牌
        Deck.Instance.Shuffle();

        
        for (int i = 0; i < 51; i++)
        {
            //如果是桌面，跳过置为player
            if (currentCharacter == CharacterType.Desk)
            {
                currentCharacter = CharacterType.Player;
            }
            //发牌
            DealTo(currentCharacter);
            //下一个人
            currentCharacter++;
        }

        //底盘发给出牌区
        for (int i = 0; i < 3; i++)
        {
            DealTo(CharacterType.Desk);
        }
        //显示各个身份的sprite
        for (int i = 1; i < 5; i++)
        {
            MakeHandCardsSprite((CharacterType)i, false);
        }
    }

    /// <summary>
    /// 清空显示的卡牌
    /// </summary>
    public void DestroyAllSprites()
    {
        CardSprite[][] cardSprites = new CardSprite[3][];
        cardSprites[0] = GameObject.Find("Player").GetComponentsInChildren<CardSprite>();
        cardSprites[1] = GameObject.Find("ComputerOne").GetComponentsInChildren<CardSprite>();
        cardSprites[2] = GameObject.Find("ComputerTwo").GetComponentsInChildren<CardSprite>();

        for (int i = 0; i < cardSprites.GetLength(0); i++)
        {
            for (int j = 0; j < cardSprites[i].Length; j++)
            {
                cardSprites[i][j].Destroy();
            }
        }
    }


    /// <summary>
    /// 收牌
    /// </summary>
    public void BackToDeck()
    {
        HandCards[] handCards = new HandCards[3];
        handCards[0] = GameObject.Find("Player").GetComponent<HandCards>();
        handCards[1] = GameObject.Find("ComputerOne").GetComponent<HandCards>();
        handCards[2] = GameObject.Find("ComputerTwo").GetComponent<HandCards>();

        for (int i = 0; i < handCards.Length; i++)
        {
            while (handCards[i].CardsCount != 0)
            {
                Card last = handCards[i][handCards[i].CardsCount - 1];
                Deck.Instance.AddCard(last);
                handCards[i].PopCard(last);
            }
        }
    }


    /// <summary>
    /// 发底牌
    /// </summary>
    /// <param name="type"></param>
    public void DealDeskCard(CharacterType type)
    {
        GameObject parentObj = GameObject.Find(type.ToString());
        HandCards cards = parentObj.GetComponent<HandCards>();
        //销毁底牌精灵
        CardSprite[] sprites = GameObject.Find("Desk").GetComponentsInChildren<CardSprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].Destroy();
        }

        while (DeskCardsCache.Instance.CardsCount != 0)
        {
            Card card = DeskCardsCache.Instance.Deal();
            cards.AddCard(card);
        }

        MakeHandCardsSprite(type, false);
        //更新身份
        UpdateIndentity(type, Identity.Landlord);
    }

    /// <summary>
    /// 发牌
    /// </summary>
    /// <param name="person"></param>
    void DealTo(CharacterType person)
    {
        if (person == CharacterType.Desk)
        {
            Card movedCard = Deck.Instance.Deal();
            DeskCardsCache.Instance.AddCard(movedCard);
        }
        else
        {
            GameObject playerObj = GameObject.Find(person.ToString());
            HandCards cards = playerObj.GetComponent<HandCards>();

            Card movedCard = Deck.Instance.Deal();
            cards.AddCard(movedCard);
        }
    }

    /// <summary>
    /// sprite
    /// </summary>
    /// <param name="type"></param>
    /// <param name="card"></param>
    /// <param name="selected"></param>
    void MakeSprite(CharacterType type, Card card, bool selected)
    {
        if (!card.isSprite)
        {
            GameObject poker = NGUITools.AddChild(GameObject.Find(type.ToString()), pokerPrefab);
            poker.AddComponent<CardSprite>();
            pokerSprite = poker.gameObject.GetComponent<UISprite>();
            CardSprite sprite = poker.gameObject.GetComponent<CardSprite>();
            sprite.sprite = pokerSprite;
            sprite.Poker = card;
            sprite.Select = selected;
        }
    }

    /// <summary>
    ///显示
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isSelected"></param>
    void MakeHandCardsSprite(CharacterType type, bool isSelected)
    {
        if (type == CharacterType.Desk)
        {
            for (int i = 0; i < DeskCardsCache.Instance.CardsCount; i++)
            {
                MakeSprite(type, DeskCardsCache.Instance[i], isSelected);
            }
        }
        else
        {
            GameObject parentObj = GameObject.Find(type.ToString());
            HandCards cards = parentObj.GetComponent<HandCards>();
            //排序
            cards.Sort();
          
            for (int i = 0; i < cards.CardsCount; i++)
            {
                if (!cards[i].isSprite)
                {
                    MakeSprite(type, cards[i], isSelected);
                }
            }

            //显示剩余手牌数
            UpdateLeftCardsCount(cards.cType, cards.CardsCount);
        }

        //调整位置
        AdjustCardSpritsPosition(type);
    }


    /// <summary>
    /// 调整卡牌位置
    /// </summary>
    /// <param name="type"></param>
    public static void AdjustCardSpritsPosition(CharacterType type)
    {
        //调整出牌区sprite位置
        if (type == CharacterType.Desk)
        {
            DeskCardsCache instance = DeskCardsCache.Instance;
            CardSprite[] cs = GameObject.Find(type.ToString()).GetComponentsInChildren<CardSprite>();
            for (int i = 0; i < cs.Length; i++)
            {
                for (int j = 0; j < cs.Length; j++)
                {
                    if (cs[j].Poker == instance[i])
                    {
                        cs[j].GoToPosition(GameObject.Find(type.ToString()), i);
                    }
                }
            }
        }
        //调整玩家sprite位置
        else
        {
            HandCards hc = GameObject.Find(type.ToString()).GetComponent<HandCards>();
            CardSprite[] cs = GameObject.Find(type.ToString()).GetComponentsInChildren<CardSprite>();
            for (int i = 0; i < hc.CardsCount; i++)
            {
                for (int j = 0; j < cs.Length; j++)
                {
                    if (cs[j].Poker == hc[i])
                    {
                        cs[j].GoToPosition(GameObject.Find(type.ToString()), i);
                    }
                }
            }
        }

    }

    /// <summary>
    /// 获取指定牌组权重
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="rule"></param>
    /// <returns></returns>
    public static int GetWeight(Card[] cards, CardsType rule)
    {
        int totalWeight = 0;
        if (rule == CardsType.ThreeAndOne || rule == CardsType.ThreeAndTwo)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (i < cards.Length - 2)
                {
                    if (cards[i].GetCardWeight == cards[i + 1].GetCardWeight && cards[i].GetCardWeight == cards[i + 2].GetCardWeight)
                    {
                        totalWeight += (int)cards[i].GetCardWeight;
                        totalWeight *= 3;
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < cards.Length; i++)
            {
                totalWeight += (int)cards[i].GetCardWeight;
            }
        }

        return totalWeight;
    }

    /// <summary>
    /// 更新剩余手牌数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="cardsCount"></param>
    public static void UpdateLeftCardsCount(CharacterType type, int cardsCount)
    {
        GameObject obj = GameObject.Find(type.ToString()).transform.Find("LeftPoker").gameObject;
        obj.GetComponent<UILabel>().text = "剩余扑克:" + cardsCount;
    }

    /// <summary>
    /// 更新身份
    /// </summary>
    /// <param name="type"></param>
    /// <param name="identity"></param>
    public static void UpdateIndentity(CharacterType type, Identity identity)
    {
        GameObject obj = GameObject.Find(type.ToString()).transform.Find("Identity").gameObject;
        GameObject.Find(type.ToString()).GetComponent<HandCards>().AccessIdentity = identity;
        obj.GetComponent<UISprite>().spriteName = "Identity_" + identity.ToString();
    }

}
