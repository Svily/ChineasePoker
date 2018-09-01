using UnityEngine;
using System.Collections;

/// <summary>
/// 角色类型
/// </summary>
public enum CharacterType
{
    Library = 0,
    Player,
    ComputerOne,
    ComputerTwo,
    Desk
}


/// <summary>
/// 花色
/// </summary>
public enum Suits
{
    Club = 0,
    Diamond,
    Heart,
    Spade,
    None
}

/// <summary>
/// 卡牌权值
/// </summary>
public enum Weight
{
    Three = 0,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    One,
    Two,
    SJoker,
    LJoker,
}

/// <summary>
/// 身份
/// </summary>
public enum Identity
{
    Farmer = 0,
    Landlord
}

/// <summary>
/// 出牌类型
/// </summary>
public enum CardsType
{
    
    None = 0,
    //王炸
    JokerBoom,
    //炸弹
    Boom,
    //单张
    Single,
    //对子
    Double,
    //三不带
    OnlyThree,
    //三带一
    ThreeAndOne,
    //三带二
    ThreeAndTwo,
    //连子
    Straight,
    //双连子
    DoubleStraight,
    //飞机
    TripleStraight,
}