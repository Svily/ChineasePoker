using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 牌库
/// </summary>
public class Deck
{
    //单例对象
    private static Deck instance;
    
    private List<Card> library;
    private CharacterType ctype;

    //获取单例对象
    public static Deck Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Deck();
            }
            return instance;
        }
    }

    /// <summary>
    /// 获取牌库中牌的数量
    /// </summary>
    public int CardsCount
    {
        get { return library.Count; }
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
    /// 私有构造
    /// </summary>
    private Deck()
    {
        library = new List<Card>();
        ctype = CharacterType.Library;
        if(library.Count == 0)
        {
            CreateDeck();
        }
    }

    /// <summary>
    /// 创建牌
    /// </summary>
    void CreateDeck()
    {
        //创建普通扑克
        for (int color = 0; color < 4; color++)
        {
            for (int value = 0; value < 13; value++)
            {
                Weight weight = (Weight)value;
                Suits suit = (Suits)color;
                string name = suit.ToString() + weight.ToString();
                Card card = new Card(name, weight, suit, ctype);
                library.Add(card);
            }
        }

        //创建大小王
        Card SJoker = new Card("SJoker", Weight.SJoker, Suits.None, ctype);
        Card LJoker = new Card("LJoker", Weight.LJoker, Suits.None, ctype);
        library.Add(SJoker);
        library.Add(LJoker);
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    public void Shuffle()
    {
        if (CardsCount == 54)
        {
            System.Random random = new System.Random();
            List<Card> newList = new List<Card>();
            //打乱顺序
            foreach (Card item in library)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }

            library.Clear();
            //重组牌
            foreach (Card item in newList)
            {
                library.Add(item);
            }

            newList.Clear();
        }
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
    /// 向牌库中添加牌
    /// </summary>
    /// <param name="card"></param>
    public void AddCard(Card card)
    {
        card.Attribution = ctype;
        library.Add(card);
    }

}
