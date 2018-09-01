using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 手牌
/// </summary>
public class HandCards : MonoBehaviour
{
    //角色
    public CharacterType cType;
    //手牌
    private List<Card> library;
    //身份
    private Identity identity;

    void Start()
    {
        //默认身份为农民
        identity = Identity.Farmer;
        library = new List<Card>();
    }



    /// <summary>
    /// 手牌数
    /// </summary>
    public int CardsCount
    {
        get { return library.Count; }
    }

    /// <summary>
    /// 设置和获取身份
    /// </summary>
    public Identity AccessIdentity
    {
        set
        {
            identity = value;
        }
        get { return identity; }
    }

    /// <summary>
    /// 由索引获取牌
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Card this[int index]
    {
        get { return library[index]; }
    }

    /// <summary>
    /// 由牌位置获取索引
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public int this[Card card]
    {
        get { return library.IndexOf(card); }
    }

    /// <summary>
    /// 添加手牌
    /// </summary>
    /// <param name="card"></param>
    public void AddCard(Card card)
    {
        card.Attribution = cType;
        library.Add(card);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <returns></returns>
    public void PopCard(Card card)
    {
        //从手牌移除
        library.Remove(card);
    }

    /// <summary>
    /// 手牌排序
    /// </summary>
    public void Sort()
    {
        CardRules.SortCards(library, false);
    }
}
