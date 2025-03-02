
using UnityEngine.Serialization;

public class PlayerSkillSO : SpecDataSO
{
    public string skillDescription;
    public Enums.ELEMENT element;
    public int playerRequireLevel;
    public int skillLevel;
    public float attack;
    [FormerlySerializedAs("coolTime")] public float cooldown;
}
