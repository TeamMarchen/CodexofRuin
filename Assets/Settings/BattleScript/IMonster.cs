using System.Collections.Generic;

public interface IMonster : IStatus
{
    public List<string> dropItemID { get; set; }
    public int exp { get; set; }
}