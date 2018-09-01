using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayCard : MonoBehaviour
{
    GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameController").transform.GetComponent<GameController>();
    }

    /// <summary>
    /// 遍历选中的牌和sprite
    /// </summary>
    public bool CheckSelectCards()
    {
        CardSprite[] sprites = this.GetComponentsInChildren<CardSprite>();

        //找出所有选中的牌
        List<Card> selectedCardsList = new List<Card>();
        List<CardSprite> selectedSpriteList = new List<CardSprite>();

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].Select)
            {
                selectedSpriteList.Add(sprites[i]);
                selectedCardsList.Add(sprites[i].Poker);
            }
        }
        //排序
        CardRules.SortCards(selectedCardsList, true);
        //出牌
        return CheckPlayCards(selectedCardsList, selectedSpriteList);
    }

    /// <summary>
    /// 对选中的牌进行出牌检测
    /// </summary>
    /// <param name="selectedCardsList"></param>
    /// <param name="selectedSpriteList"></param>
    bool CheckPlayCards(List<Card> selectedCardsList, List<CardSprite> selectedSpriteList)
    {
        Card[] selectedCardsArray = selectedCardsList.ToArray();
        //出牌类型
        CardsType type;
        //检测是否符合出牌规则，符合则出牌
        if (CardRules.PopEnable(selectedCardsArray, out type))
        {
            //获取当前出牌类型
            CardsType rule = DeskCardsCache.Instance.Rule;
            if (OrderController.Instance.Biggest == OrderController.Instance.Type)
            {
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (DeskCardsCache.Instance.Rule == CardsType.None)
            {
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            //炸弹
            else if (type == CardsType.Boom && rule != CardsType.Boom)
            {
                gameController.DoubleMultiple();
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (type == CardsType.JokerBoom)
            {
                gameController.DoubleMultiple();
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (type == CardsType.Boom && rule == CardsType.Boom && GameController.GetWeight(selectedCardsArray, type) > DeskCardsCache.Instance.TotalWeight)
            {
                gameController.DoubleMultiple();
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
            else if (GameController.GetWeight(selectedCardsArray, type) > DeskCardsCache.Instance.TotalWeight)
            {
                PlayCards(selectedCardsList, selectedSpriteList, type);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 玩家出牌
    /// </summary>
    /// <param name="selectedCardsList"></param>
    /// <param name="selectedSpriteList"></param>
    void PlayCards(List<Card> selectedCardsList, List<CardSprite> selectedSpriteList, CardsType type)
    {
        HandCards player = GameObject.Find("Player").GetComponent<HandCards>();
        //清空出牌区
        DeskCardsCache.Instance.Clear();
        //更新出牌类型
        DeskCardsCache.Instance.Rule = type;

        for (int i = 0; i < selectedSpriteList.Count; i++)
        {
            //从手牌移除
            player.PopCard(selectedSpriteList[i].Poker);
            //添加并在出牌区显示
            DeskCardsCache.Instance.AddCard(selectedSpriteList[i].Poker);
            selectedSpriteList[i].transform.parent = GameObject.Find("Desk").transform;
        }

        //排序
        DeskCardsCache.Instance.Sort();
        //调整位置
        GameController.AdjustCardSpritsPosition(CharacterType.Desk);
        GameController.AdjustCardSpritsPosition(CharacterType.Player);
        //更新手牌数
        GameController.UpdateLeftCardsCount(CharacterType.Player, player.CardsCount);
        //无手牌则胜利
        if (player.CardsCount == 0)
        {
            GameObject.Find("GameController").GetComponent<GameController>().GameOver(player);
        }
        else
        {
            //设置为最大出牌者，轮转
            OrderController.Instance.Biggest = CharacterType.Player;
            OrderController.Instance.Turn();
        }
    }
}
