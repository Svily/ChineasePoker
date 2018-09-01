﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardRules
{
    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="ascending"></param>
    public static void SortCards(List<Card> cards, bool ascending)
    {
        cards.Sort((Card a, Card b) =>
            {
                //是否降序排列
                if (!ascending)
                {
                    //优先按权重降序，再按花色升序
                    return -a.GetCardWeight.CompareTo(b.GetCardWeight) * 2 + a.GetCardSuit.CompareTo(b.GetCardSuit);
                }
                else
                {
                    //按权重升序
                    return a.GetCardWeight.CompareTo(b.GetCardWeight);
                }
                    
            }
        );
    }

    /// <summary>
    /// 单张
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsSingle(Card[] cards)
    {
        if (cards.Length == 1)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 对子
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsDouble(Card[] cards)
    {
        if (cards.Length == 2)
        {
            if (cards[0].GetCardWeight == cards[1].GetCardWeight)
            {
                return true;
            }
                
        }

        return false;
    }

    /// <summary>
    /// 顺子
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsStraight(Card[] cards)
    {

        if (cards.Length < 5 || cards.Length > 12)
        {
            return false;
        }
            
        for (int i = 0; i < cards.Length - 1; i++)
        {
            Weight w = cards[i].GetCardWeight;
            if (cards[i + 1].GetCardWeight - w != 1)
            {
                return false;
            }
            //不能超过A
            if (w > Weight.One || cards[i + 1].GetCardWeight > Weight.One)
            {
                return false;
            }
                
        }

        return true;
    }

    /// <summary>
    /// 是否是双连子
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsDoubleStraight(Card[] cards)
    {
        //不能少于6张，且为双数
        if (cards.Length < 6 || cards.Length % 2 != 0)
        {
            return false;
        }
        for (int i = 0; i < cards.Length; i += 2)
        {
            if (cards[i + 1].GetCardWeight != cards[i].GetCardWeight)
            {
                return false;
            }

            if (i < cards.Length - 2)
            {
                if (cards[i + 2].GetCardWeight - cards[i].GetCardWeight != 1)
                {
                    return false;
                }

                //不能超过A
                if (cards[i].GetCardWeight > Weight.One || cards[i + 2].GetCardWeight > Weight.One)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 飞机
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsTripleStraight(Card[] cards)
    {
        //不能少于6张且为3的倍数
        if (cards.Length < 6 || cards.Length % 3 != 0)
        {
            return false;
        }

        for (int i = 0; i < cards.Length; i += 3)
        {
            if (cards[i + 1].GetCardWeight != cards[i].GetCardWeight)
            {
                return false;
            }
            if (cards[i + 2].GetCardWeight != cards[i].GetCardWeight)
            {
                return false;
            }
            if (cards[i + 1].GetCardWeight != cards[i + 2].GetCardWeight)
            {
                return false;
            }
            if (i < cards.Length - 3)
            {
                if (cards[i + 3].GetCardWeight - cards[i].GetCardWeight != 1)
                {
                    return false;
                }

                //不能超过A
                if (cards[i].GetCardWeight > Weight.One || cards[i + 3].GetCardWeight > Weight.One)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 三不带
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsOnlyThree(Card[] cards)
    {
        if (cards.Length % 3 != 0)
        {
            return false;
        }
        if (cards[0].GetCardWeight != cards[1].GetCardWeight)
        {
            return false;
        }
        if (cards[1].GetCardWeight != cards[2].GetCardWeight)
        {
            return false;
        }
        if (cards[0].GetCardWeight != cards[2].GetCardWeight)
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// 三带一
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsThreeAndOne(Card[] cards)
    {
        if (cards.Length != 4)
        {
            return false;
        }

        if (cards[0].GetCardWeight == cards[1].GetCardWeight && cards[1].GetCardWeight == cards[2].GetCardWeight)
        {
            return true;
        }
        else if (cards[1].GetCardWeight == cards[2].GetCardWeight && cards[2].GetCardWeight == cards[3].GetCardWeight)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 三代二
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsThreeAndTwo(Card[] cards)
    {
        if (cards.Length != 5)
        {
            return false;
        }

        if (cards[0].GetCardWeight == cards[1].GetCardWeight && cards[1].GetCardWeight == cards[2].GetCardWeight)
        {
            if (cards[3].GetCardWeight == cards[4].GetCardWeight)
            {
                return true;
            }
        }

        else if (cards[2].GetCardWeight == cards[3].GetCardWeight && cards[3].GetCardWeight == cards[4].GetCardWeight)
        {
            if (cards[0].GetCardWeight == cards[1].GetCardWeight)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 炸弹
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsBoom(Card[] cards)
    {
        if (cards.Length != 4)
        {
            return false;
        }

        if (cards[0].GetCardWeight != cards[1].GetCardWeight)
        {
            return false;
        }
        if (cards[1].GetCardWeight != cards[2].GetCardWeight)
        {
            return false;
        }
        if (cards[2].GetCardWeight != cards[3].GetCardWeight)
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// 王炸
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsJokerBoom(Card[] cards)
    {
        if (cards.Length != 2)
        {
            return false;
        }
        if (cards[0].GetCardWeight == Weight.SJoker && cards[1].GetCardWeight == Weight.LJoker)
        {
            return true;
        }
        else if (cards[0].GetCardWeight == Weight.LJoker && cards[1].GetCardWeight == Weight.SJoker)
        {
           
            return true;
            
        }

        return false;
    }

    /// <summary>
    /// 判断是否符合出牌规则
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool PopEnable(Card[] cards, out CardsType type)
    {
        type = CardsType.None;
        bool isRule = false;
        switch (cards.Length)
        {
            case 1:
                isRule = true;
                type = CardsType.Single;
                break;
            case 2:
                if (IsDouble(cards))
                {
                    isRule = true;
                    type = CardsType.Double;
                }
                else if (IsJokerBoom(cards))
                {
                    isRule = true;
                    type = CardsType.JokerBoom;
                }
                break;
            case 3:
                if (IsOnlyThree(cards))
                {
                    isRule = true;
                    type = CardsType.OnlyThree;
                }
                break;
            case 4:
                if (IsBoom(cards))
                {
                    isRule = true;
                    type = CardsType.Boom;
                }
                else if (IsThreeAndOne(cards))
                {
                    isRule = true;
                    type = CardsType.ThreeAndOne;
                }

                break;
            case 5:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = CardsType.Straight;
                }
                else if (IsThreeAndTwo(cards))
                {
                    isRule = true;
                    type = CardsType.ThreeAndTwo;
                }
                break;
            case 6:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = CardsType.Straight;
                }
                else if (IsTripleStraight(cards))
                {
                    isRule = true;
                    type = CardsType.TripleStraight;
                }
                else if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = CardsType.DoubleStraight;
                }
                break;
            case 7:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = CardsType.Straight;
                }
                break;
            case 8:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = CardsType.Straight;
                }
                else if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = CardsType.DoubleStraight;
                }
                break;
            case 9:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = CardsType.Straight;
                }
                else if (IsOnlyThree(cards))
                {
                    isRule = true;
                    type = CardsType.OnlyThree;
                }
                break;
            case 10:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = CardsType.Straight;
                }
                else if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = CardsType.DoubleStraight;
                }
                break;

            case 11:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = CardsType.Straight;
                }
                break;
            case 12:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = CardsType.Straight;
                }
                else if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = CardsType.DoubleStraight;
                }
                else if (IsOnlyThree(cards))
                {
                    isRule = true;
                    type = CardsType.OnlyThree;
                }
                break;
            case 13:
                break;
            case 14:
                if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = CardsType.DoubleStraight;
                }
                break;
            case 15:
                if (IsOnlyThree(cards))
                {
                    isRule = true;
                    type = CardsType.OnlyThree;
                }
                break;
            case 16:
                if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = CardsType.DoubleStraight;
                }
                break;
            case 17:
                break;
            case 18:
                if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = CardsType.DoubleStraight;
                }
                else if (IsOnlyThree(cards))
                {
                    isRule = true;
                    type = CardsType.OnlyThree;
                }
                break;
            case 19:
                break;
            case 20:
                if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = CardsType.DoubleStraight;
                }
                break;
            default:
                break;
        }

        return isRule;
    }
}
