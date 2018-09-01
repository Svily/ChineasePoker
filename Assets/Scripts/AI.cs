using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    //不出提示
    private GameObject tip;
    GameController gameController;
    //玩家是否开启托管
    public bool isPlayerAutoPlay = false;

    void Start()
    {
        //绑定自动出牌函数   
        OrderController.Instance.AI += AutoPlayCard;

        tip = transform.Find("Tip").gameObject;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    /// <summary>
    /// 开启托管
    /// </summary>
    public void PlayerAutoPlay()
    {
        
        isPlayerAutoPlay = true;
        //绑定自动出牌
        OrderController.Instance.PlayerAI += AutoPlayCard;
    }

    /// <summary>
    /// 取消托管
    /// </summary>
    public void DisAutoPlay()
    {
        isPlayerAutoPlay = false;
        //解绑
        OrderController.Instance.PlayerAI -= AutoPlayCard;
    }

    /// <summary>
    /// 不出提示
    /// </summary>
    public void ShowTip()
    {
        tip.SetActive(true);
    }
    public void DisShowTip()
    {
        tip.SetActive(false);
    }

    /// <summary>
    /// 自动出牌
    /// </summary>
    void AutoPlayCard(bool isNone)
    {
        
        HandCards handCards = gameObject.GetComponent<HandCards>();

        //第一个出牌后，开启协程，延迟2.0s，AI再出牌
        if (OrderController.Instance.Type == handCards.cType)
        { 
            StartCoroutine(DelayPlayCards(isNone));
        }

    }

    /// <summary>
    /// 延时出牌
    /// </summary>
    /// <returns></returns>
    public IEnumerator DelayPlayCards(bool isNone)
    {
        DisShowTip();
        //延迟2.0s
        yield return new WaitForSeconds(2.0f);
        //获取当前出牌类型，为none则为第一出牌人
        CardsType rule = isNone ? CardsType.None : DeskCardsCache.Instance.Rule;
        //出牌区牌组总权重
        int deskWeight = DeskCardsCache.Instance.TotalWeight;
        List<Card> playCardsList = new List<Card>();
        //根据当前出牌类型找相应牌组，找到就出牌，找不到跳过本轮
        switch (rule)
        {
            //第一个出牌
            case CardsType.None:
                playCardsList = FirstPlayCard();
                if (playCardsList.Count != 0)
                {
                    RemoveCards(playCardsList);
                    PlayCards(playCardsList, GetSprite(playCardsList));
                }
                break;
            case CardsType.JokerBoom:
                ShowTip();
                OrderController.Instance.Turn();
                break;
            case CardsType.Boom:
                playCardsList = FindBoom(GetAllCards(), deskWeight);
                if (playCardsList.Count != 0)
                {
                    gameController.DoubleMultiple();
                    RemoveCards(playCardsList);
                    PlayCards(playCardsList, GetSprite(playCardsList));
                    
                }
                else
                {
                    ShowTip();
                    OrderController.Instance.Turn();
                }
                break;
            case CardsType.Single:
                playCardsList = FindSingle(GetAllCards(), deskWeight, false);
                if (playCardsList.Count != 0)
                {
                    RemoveCards(playCardsList);
                    PlayCards(playCardsList, GetSprite(playCardsList));
                }
                else
                {
                    ShowTip();
                    OrderController.Instance.Turn();
                }
                break;
            case CardsType.Double:
                playCardsList = FindDouble(GetAllCards(), deskWeight, false);
                if (playCardsList.Count != 0)
                {
                    RemoveCards(playCardsList);
                    PlayCards(playCardsList, GetSprite(playCardsList));
                }
                else
                {
                    ShowTip();
                    OrderController.Instance.Turn();
                }
                break;
            case CardsType.OnlyThree:
                playCardsList = FindOnlyThree(GetAllCards(), deskWeight);
                if (playCardsList.Count != 0)
                {
                    RemoveCards(playCardsList);
                    PlayCards(playCardsList, GetSprite(playCardsList));
                }
                else
                {
                    ShowTip();
                    OrderController.Instance.Turn();
                }
                break;
            case CardsType.ThreeAndOne:
                playCardsList = FindThreeAndOne(GetAllCards(), deskWeight);
                if (playCardsList.Count != 0)
                {
                    RemoveCards(playCardsList);
                    PlayCards(playCardsList, GetSprite(playCardsList));
                }
                else
                {
                    ShowTip();
                    OrderController.Instance.Turn();
                }
                break;
            case CardsType.ThreeAndTwo:
                playCardsList = FindThreeAndTwo(GetAllCards(), deskWeight);
                if (playCardsList.Count != 0)
                {
                    RemoveCards(playCardsList);
                    PlayCards(playCardsList, GetSprite(playCardsList));
                }
                else
                {
                    ShowTip();
                    OrderController.Instance.Turn();
                }
                break;
            case CardsType.Straight:
                playCardsList = FindStraight(GetAllCards(), DeskCardsCache.Instance.MinWeight, DeskCardsCache.Instance.CardsCount);
                if (playCardsList.Count != 0)
                {
                    RemoveCards(playCardsList);
                    PlayCards(playCardsList, GetSprite(playCardsList));
                }
                else
                {
                    List<Card> boom1 = FindBoom(GetAllCards(), 0);
                    if (boom1.Count != 0)
                    {
                        RemoveCards(boom1);
                        PlayCards(boom1, GetSprite(boom1));
                        gameController.DoubleMultiple();
                    }
                    else
                    {
                        ShowTip();
                        OrderController.Instance.Turn();
                    }
                }
                break;
            case CardsType.DoubleStraight:
                playCardsList = FindDoubleStraight(GetAllCards(), DeskCardsCache.Instance.MinWeight, DeskCardsCache.Instance.CardsCount);
                if (playCardsList.Count != 0)
                {
                    RemoveCards(playCardsList);
                    PlayCards(playCardsList, GetSprite(playCardsList));
                }
                else
                {

                    List<Card> boom2 = FindBoom(GetAllCards(), 0);
                    if (boom2.Count != 0)
                    {
                        RemoveCards(boom2);
                        PlayCards(boom2, GetSprite(boom2));
                        gameController.DoubleMultiple();
                    }
                    else
                    {
                        ShowTip();
                        OrderController.Instance.Turn();
                    }
                }
                break;
            case CardsType.TripleStraight:
                List<Card> boom = FindBoom(GetAllCards(), 0);
                if (boom.Count != 0)
                {
                    RemoveCards(boom);
                    PlayCards(boom, GetSprite(boom));
                    gameController.DoubleMultiple();
                }
                else
                {
                    ShowTip();
                    OrderController.Instance.Turn();
                }
                break;
        }
    }

    /// <summary>
    /// 第一出牌人为电脑
    /// </summary>
    /// <returns></returns>
    public List<Card> FirstPlayCard()
    {
        DeskCardsCache.Instance.Rule = CardsType.None;

        List<Card> list = new List<Card>();
       
        //按连子，连对，三带二，三带一，三不带，对子，单张顺序出牌
        list = FindStraight(GetAllCards(), 0, 6);

        if (list.Count == 0)
        {
            list = FindDoubleStraight(GetAllCards(), 0, 6);
        }
        if (list.Count == 0)
        {
            list = FindThreeAndTwo(GetAllCards(), 0);
        }
        if (list.Count == 0)
        {
            list = FindThreeAndOne(GetAllCards(),0);
        }
        if (list.Count == 0)
        {
            list = FindOnlyThree(GetAllCards(), 0);
        }
        //优先出对子
        if (list.Count == 0)
        {
            list = FindDouble(GetAllCards(), 0, true);
        }
        if (list.Count == 0)
        {
            list = FindSingle(GetAllCards(), 0, true);
        }
        return list;
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="selectedCardsList"></param>
    /// <param name="selectedSpriteList"></param>
    void PlayCards(List<Card> selectedCardsList, List<CardSprite> selectedSpriteList)
    {
        Card[] selectedCardsArray = selectedCardsList.ToArray();
        CardsType type;

        //符合桌上出牌类型则出牌
        if (CardRules.PopEnable(selectedCardsArray, out type))
        {
            HandCards AIHandCards = gameObject.GetComponent<HandCards>();
            //清空桌面已出的牌
            DeskCardsCache.Instance.Clear();
            //更新出牌类型
            DeskCardsCache.Instance.Rule = type;
            //显示已出牌
            for (int i = 0; i < selectedSpriteList.Count; i++)
            {
                DeskCardsCache.Instance.AddCard(selectedSpriteList[i].Poker);
                selectedSpriteList[i].transform.parent = GameObject.Find("Desk").transform;
                selectedSpriteList[i].Poker = selectedSpriteList[i].Poker;
            }
            //排序
            DeskCardsCache.Instance.Sort();
            //调整手牌和出牌区牌序
            GameController.AdjustCardSpritsPosition(CharacterType.Desk);
            GameController.AdjustCardSpritsPosition(AIHandCards.cType);
            //更新剩余手牌数
            GameController.UpdateLeftCardsCount(AIHandCards.cType, AIHandCards.CardsCount);
            //手牌为0游戏结束
            if (AIHandCards.CardsCount == 0)
            {
                GameObject.Find("GameController").GetComponent<GameController>().GameOver(AIHandCards);
            }
            else
            {
                //更新当前最大出牌者
                OrderController.Instance.Biggest = AIHandCards.cType;
                //下一轮
                OrderController.Instance.Turn();
            }
        }
    }

    /// <summary>
    /// 获取所有手牌
    /// </summary>
    /// <param name="exclude"></param>
    /// <returns></returns>
    List<Card> GetAllCards(List<Card> exclude = null)
    {
        List<Card> cards = new List<Card>();
        HandCards allCards = gameObject.GetComponent<HandCards>();
        bool isContinue = false;
        for (int i = 0; i < allCards.CardsCount; i++)
        {
            isContinue = false;
            if (exclude != null)
            {
                for (int j = 0; j < exclude.Count; j++)
                {
                    if (allCards[i] == exclude[j])
                    {
                        isContinue = true;
                        break;
                    }

                }
            }

            if (!isContinue)
            {
                cards.Add(allCards[i]);
            }
                
        }
        //排序
        CardRules.SortCards(cards, true);

        return cards;
    }

    /// <summary>
    /// 获取sprite
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    List<CardSprite> GetSprite(List<Card> cards)
    {
        HandCards t = gameObject.GetComponent<HandCards>();
        CardSprite[] sprites = GameObject.Find(t.cType.ToString()).GetComponentsInChildren<CardSprite>();

        List<CardSprite> selectedSpriteList = new List<CardSprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            for (int j = 0; j < cards.Count; j++)
            {
                if (cards[j] == sprites[i].Poker)
                {
                    selectedSpriteList.Add(sprites[i]);
                    break;
                }
            }
        }

        return selectedSpriteList;
    }

    /// <summary>
    /// 移除手牌
    /// </summary>
    /// <param name="cards"></param>
    void RemoveCards(List<Card> cards)
    {
        HandCards allCards = gameObject.GetComponent<HandCards>();

        for (int j = 0; j < cards.Count; j++)
        {
            for (int i = 0; i < allCards.CardsCount; i++)
            {
                if (cards[j] == allCards[i])
                {
                    allCards.PopCard(cards[j]);
                    break;
                }
            }
        }
    }

   /// <summary>
   /// 找炸弹
   /// </summary>
   /// <param name="allCards"></param>
   /// <param name="weight"></param>
   /// <returns></returns>
    List<Card> FindBoom(List<Card> allCards, int weight)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i <= allCards.Count - 4)
            {
                //普通炸弹
                if (allCards[i].GetCardWeight == allCards[i + 1].GetCardWeight &&
                    allCards[i].GetCardWeight == allCards[i + 2].GetCardWeight &&
                    allCards[i].GetCardWeight == allCards[i + 3].GetCardWeight)
                {
                    int totalWeight = (int)allCards[i].GetCardWeight + (int)allCards[i + 1].GetCardWeight + (int)allCards[i + 2].GetCardWeight+ (int)allCards[i + 4].GetCardWeight;

                    if (totalWeight > weight)
                    {
                        ret.Add(allCards[i]);
                        ret.Add(allCards[i + 1]);
                        ret.Add(allCards[i + 2]);
                        ret.Add(allCards[i + 3]);
                        break;
                    }
                }
            }
        }

        //王炸
        if (ret.Count == 0)
        {
            for (int j = 0; j < allCards.Count; j++)
            {
                if (j < allCards.Count - 1)
                {
                    if (allCards[j].GetCardWeight == Weight.SJoker &&
                        allCards[j + 1].GetCardWeight == Weight.LJoker)
                    {
                        ret.Add(allCards[j]);
                        ret.Add(allCards[j + 1]);
                    }
                }

            }
        }

        return ret;
    }

    /// <summary>
    /// 找单张
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    List<Card> FindSingle(List<Card> allCards, int weight, bool canEqual)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (canEqual)
            {
                if ((int)allCards[i].GetCardWeight >= weight)
                {
                    ret.Add(allCards[i]);
                    break;
                }
            }
            else
            {
                if ((int)allCards[i].GetCardWeight > weight)
                {
                    ret.Add(allCards[i]);
                    break;
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// 找对子
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    List<Card> FindDouble(List<Card> allCards, int weight, bool canEqual)
    {
        List<Card> ret = new List<Card>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i < allCards.Count - 1)
            {
                if (allCards[i].GetCardWeight == allCards[i + 1].GetCardWeight)
                {
                    int totalWeight = (int)allCards[i].GetCardWeight + (int)allCards[i + 1].GetCardWeight;

                    if (canEqual)
                    {
                        if (totalWeight >= weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            break;
                        }
                    }
                    else
                    {
                        if (totalWeight > weight)
                        {
                            ret.Add(allCards[i]);
                            ret.Add(allCards[i + 1]);
                            break;
                        }
                    }

                }
            }
        }

        return ret;
    }

    /// <summary>
    /// 三不带
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    List<Card> FindOnlyThree(List<Card> allCards, int weight)
    {
        List<Card> ret = new List<Card>();

        for (int i = 0; i < allCards.Count; i++)
        {
            if (i <= allCards.Count - 3)
            {
                if (allCards[i].GetCardWeight == allCards[i + 1].GetCardWeight && allCards[i].GetCardWeight == allCards[i + 2].GetCardWeight)
                {
                    int totalWeight = (int)allCards[i].GetCardWeight + (int)allCards[i + 1].GetCardWeight + (int)allCards[i + 2].GetCardWeight;

                    if (totalWeight >= weight)
                    {
                        ret.Add(allCards[i]);
                        ret.Add(allCards[i + 1]);
                        ret.Add(allCards[i + 2]);
                        break;
                    }


                }
            }
        }

        return ret;
    }

    /// <summary>
    /// 三代一
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    List<Card> FindThreeAndOne(List<Card> allCards, int weight)
    {
        List<Card> three = FindOnlyThree(allCards, weight);
        if (three.Count != 0)
        {
            List<Card> leftCards = GetAllCards(three);
            List<Card> one = FindSingle(leftCards, (int)Weight.Three, true);
            three.AddRange(one);
        }
        else
        {
            three.Clear();
        }


        return three;
    }

    /// <summary>
    /// 三带二
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    List<Card> FindThreeAndTwo(List<Card> allCards, int weight)
    {
        List<Card> three = FindOnlyThree(allCards, weight);
        if (three.Count != 0)
        {
            List<Card> leftCards = GetAllCards(three);
            List<Card> two = FindDouble(leftCards, (int)Weight.Three, true);

            three.AddRange(two);
        }
        else
        {
            three.Clear();
        }


        return three;

    }

    /// <summary>
    /// 连子
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="minWeight"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    List<Card> FindStraight(List<Card> allCards, int minWeight, int length)
    {
        List<Card> ret = new List<Card>();
        int counter = 1;
        List<int> indeies = new List<int>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i < allCards.Count - 4)
            {
                int weight = (int)allCards[i].GetCardWeight;

                if (weight > minWeight)
                {
                    counter = 1;
                    indeies.Clear();

                    for (int j = i + 1; j < allCards.Count; j++)
                    {
                        if (allCards[j].GetCardWeight > Weight.One)
                            break;
                        if ((int)allCards[j].GetCardWeight - weight == counter)
                        {
                            counter++;
                            indeies.Add(j);
                        }

                        if (counter == length)
                            break;
                    }
                }


            }
            if (counter == length)
            {
                indeies.Insert(0, i);
                break;
            }

        }

        if (counter == length)
        {
            for (int i = 0; i < indeies.Count; i++)
            {
                ret.Add(allCards[indeies[i]]);
            }
        }

        return ret;
    }

    /// <summary>
    /// 连对
    /// </summary>
    /// <param name="allCards"></param>
    /// <param name="minWeight"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    List<Card> FindDoubleStraight(List<Card> allCards, int minWeight, int length)
    {
        List<Card> ret = new List<Card>();
        int counter = 0;
        List<int> indeies = new List<int>();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (i < allCards.Count - 4)
            {
                int weight = (int)allCards[i].GetCardWeight;
                if (weight > minWeight)
                {
                    counter = 0;
                    indeies.Clear();

                    int circle = 0;
                    for (int j = i + 1; j < allCards.Count; j++)
                    {
                        if (allCards[j].GetCardWeight > Weight.One)
                            break;

                        if ((int)allCards[j].GetCardWeight - weight == counter)
                        {
                            circle++;
                            if (circle % 2 == 1)
                            {
                                counter++;
                            }
                            indeies.Add(j);
                        }

                        if (counter == length / 2)
                            break;
                    }
                }
            }
            if (counter == length / 2)
            {
                indeies.Insert(0, i);
                break;
            }

        }

        if (counter == length / 2)
        {
            for (int i = 0; i < indeies.Count; i++)
            {
                ret.Add(allCards[indeies[i]]);
            }
        }

        return ret;
    }
    
}
