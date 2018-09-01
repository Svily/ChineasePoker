using UnityEngine;
using System.Collections;

//定义委托
public delegate void CardEventHandle(bool flag);

public class OrderController
{
    //当前最大出牌者
    private CharacterType biggest;
    //当前出牌者
    public CharacterType currentAuthority;
    //单例对象
    private static OrderController instance;
    //电脑AI
    public event CardEventHandle AI;
    //玩家AI
    public event CardEventHandle PlayerAI;
    //是否托管
    bool isPlayerAuto;
    //按键激活
    public event CardEventHandle activeButton;
    //获取单例对象
    public static OrderController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new OrderController();
            }
            return instance;
        }
    }

    /// <summary>
    /// 当前出牌者
    /// </summary>
    public CharacterType Type
    {
        get { return currentAuthority; }
    }

    /// <summary>
    /// 最大出牌者
    /// </summary>
    public CharacterType Biggest
    {
        set { biggest = value; }
        get { return biggest; }
    }

    /// <summary>
    /// 默认构造函数
    /// </summary>
    private OrderController()
    {
        //默认为desk出牌
        currentAuthority = CharacterType.Desk;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="type"></param>
    public void Init(CharacterType type)
    {
        currentAuthority = type;
        Biggest = type;
        if (currentAuthority == CharacterType.Player)
        {
            //初始为玩家出牌，激活出牌按钮，禁用不出按钮
            activeButton(false);
        }
        else
        {
            //电脑自动出牌
            AI(true);
        }
    }

    /// <summary>
    /// 出牌轮转
    /// </summary>
    public void Turn()
    {
        isPlayerAuto = GameObject.Find("GamePanel").transform.Find("Player").GetComponent<AI>().isPlayerAutoPlay;

        //轮转
        currentAuthority += 1;

        //重置
        if (currentAuthority == CharacterType.Desk)
        {
            currentAuthority = CharacterType.Player;
        }

        if (currentAuthority == CharacterType.ComputerOne || currentAuthority == CharacterType.ComputerTwo)
        {
            AI(biggest == currentAuthority);
        }
        else if (currentAuthority == CharacterType.Player)
        {
            //根据是否托管进行相关处理
            if (isPlayerAuto)
            {
                //托管出牌
                PlayerAI(biggest == currentAuthority);
            }
            else
            {
                activeButton(biggest != currentAuthority);
            }

        }

    }

    
}
