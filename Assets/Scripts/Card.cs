using UnityEngine;
using System.Collections;

/// <summary>
/// 牌类
/// </summary>
public class Card
{
    //卡牌名称
    private string cardName;
    //权重
    private Weight weight;
    //花色
    private Suits color;
    //归属
    private CharacterType belongTo;
    //是否显示sprite
    private bool makedSprite;
    //带参构造函数
    public Card(string name, Weight weight, Suits color, CharacterType belongTo)
    {
        this.makedSprite = false;
        this.cardName = name;
        this.weight = weight;
        this.color = color;
        this.belongTo = belongTo;
    }

    /// <summary>
    /// 返回牌名
    /// </summary>
    public string GetCardName
    {
        get { return cardName; }
    }

    /// <summary>
    /// 返回权重
    /// </summary>
    public Weight GetCardWeight
    {
        get { return weight; }
    }

    /// <summary>
    /// 返回花色
    /// </summary>
    public Suits GetCardSuit
    {
        get { return color; }
    }

    /// <summary>
    /// 是否显示sprite
    /// </summary>
    public bool isSprite
    {
        set { makedSprite = value; }
        get { return makedSprite; }
    }

    /// <summary>
    /// 归属
    /// </summary>
    public CharacterType Attribution
    {
        set { belongTo = value; }
        get { return belongTo; }
    }

}
