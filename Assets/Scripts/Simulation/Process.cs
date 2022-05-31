public class Process
{
    public int id;
    public int index;

    public int size;

    public override bool Equals(object obj)
    {
        return obj is Process p && p.id == id;
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}
