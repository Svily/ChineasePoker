using UnityEngine;
using System.Collections;

/// <summary>
/// 卡片sprite显示
/// </summary>
public class CardSprite : MonoBehaviour
{
    private Card card;
    public UISprite sprite;
    //是否被选中
    private bool isSelected;

    /// <summary>
    /// card实例化对象
    /// </summary>
    public Card Poker
    {
        set
        {
            card = value;
            //默认显示
            card.isSprite = true;
            //显示
            SetSprite();
        }
        get { return card; }
    }

    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool Select
    {
        set { isSelected = value; }
        get { return isSelected; }
    }

    /// <summary>
    /// 设置名称
    /// </summary>
    void SetSprite()
    {
        if (card.Attribution == CharacterType.Player || card.Attribution == CharacterType.Desk)
        {
            sprite.spriteName = card.GetCardName;
        }
        else
        {
            sprite.spriteName = "SmallCardBack1";
        }
    }

    /// <summary>
    /// 销毁sprite
    /// </summary>
    public void Destroy()
    {
        
        card.isSprite = false;
        //销毁
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 调整位置
    /// </summary>
    public void GoToPosition(GameObject parent, int index)
    {
        //按照位置顺序设置深浅度
        sprite.depth = index;

        if (card.Attribution == CharacterType.Player)
        {
            transform.localPosition = parent.transform.Find("CardsStartPoint").localPosition + Vector3.right * 25 * index;
            if (isSelected)
            {
                transform.localPosition += Vector3.up * 10;
            }
        }
        else if (card.Attribution == CharacterType.ComputerOne || card.Attribution == CharacterType.ComputerTwo)
        {
            transform.localPosition = parent.transform.Find("CardsStartPoint").localPosition + Vector3.up * -25 * index;
        }
        else if (card.Attribution == CharacterType.Desk)
        {
            transform.localPosition = parent.transform.Find("PlacePoint").localPosition + Vector3.right * 25 * index;
        }

    }

    /// <summary>
    /// 卡牌点击
    /// </summary>
    public void OnClick()
    {
        if (card.Attribution == CharacterType.Player)
        {
            if (isSelected)
            {
                transform.localPosition -= Vector3.up * 10;
                isSelected = false;
            }
            else
            {
                transform.localPosition += Vector3.up * 10;
                isSelected = true;
            }
        }
    }
}
