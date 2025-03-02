public class PlayerPropertyManager : Singleton<PlayerPropertyManager>
{
    public int m_Gold { get; private set; }
    public int m_Cash { get; private set; }

    public void AddGold(int addGold)
    {
        m_Gold += addGold;
        UIManager.Instance.Gold = m_Gold;
    }

    public void AddCash(int addCash)
    {
        m_Cash += addCash;
        UIManager.Instance.Cash = m_Cash;
    }

    public void Init()
    {
        //load 데이터로 가져오기
        m_Gold = 0;
        m_Cash = 0;
    }
}
