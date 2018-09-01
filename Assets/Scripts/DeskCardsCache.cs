using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 出牌区
/// </summary>
public class DeskCardsCache
{
    //单例对象
    private static DeskCardsCache instance;
    private List<Card> library;
    private CharacterType ctype;
    //当前出牌类型
    private CardsType rule;

    /// <summary>
    /// 获取单例对象
    /// </summary>
    public static DeskCardsCache Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DeskCardsCache();
            }
            return instance;
        }
    }

    //出牌类型设置和获取
    public CardsType Rule
    {
        set { rule = value; }
        get { return rule; }
    }

    /// <summary>
    /// 根据索引获取牌
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Card this[int index]
    {
        get
        {
            return library[index];
        }
    }

    /// <summary>
    /// 获取牌的数量
    /// </summary>
    public int CardsCount
    {
        get { return library.Count; }
    }

    /// <summary>
    /// 最小权重
    /// </summary>
    public int MinWeight
    {
        get { return (int)library[0].GetCardWeight; }
    }

    /// <summary>
    /// 总权重
    /// </summary>
    public int TotalWeight
    {
        get
        {
            return GameController.GetWeight(library.ToArray(), rule);
        }
    }

    /// <summary>
    /// 构造函数，初始化数据
    /// </summary>
    private DeskCardsCache()
    {
        library = new List<Card>();
        //默认为desk
        ctype = CharacterType.Desk;
        rule = CardsType.None;
    }

    /// <summary>
    /// 发牌
    /// </summary>
    public Card Deal()
    {
        Card ret = library[library.Count - 1];
        library.Remove(ret);
        return ret;
    }

    /// <summary>
    /// 向出牌区中添加牌
    /// </summary>
    /// <param name="card"></param>
    public void AddCard(Card card)
    {
        card.Attribution = ctype;
        library.Add(card);
    }

    /// <summary>
    /// 清空桌面已出牌
    /// </summary>
    public void Clear()
    {
        if (library.Count != 0)
        {
            CardSprite[] cardSprites = GameObject.Find("Desk").GetComponentsInChildren<CardSprite>();
            for (int i = 0; i < cardSprites.Length;i ++)
            {
                cardSprites[i].transform.parent = null;
                cardSprites[i].Destroy();
            }

            while (library.Count != 0)
            {
                Card card = library[library.Count - 1];
                library.Remove(card);
                Deck.Instance.AddCard(card);
            }

            rule = CardsType.None;
        }
    }

    /// <summary>
    /// 排序
    /// </summary>
    public void Sort()
    {
        CardRules.SortCards(library, false);
    }
}
