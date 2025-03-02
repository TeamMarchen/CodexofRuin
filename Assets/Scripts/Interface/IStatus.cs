public interface IStatus
{
    public int level { get; set; }
    public string name { get; set; }
    public float hp { get; set; }
    public float speed { get; set; }
    public float attackTime { get; set; }
    public float attackRange { get; set; }
    public float baseDamage { get; set; }
}
