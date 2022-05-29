public struct MemoryPage
{
    public int processId;
    public int index;

    public static MemoryPage NullPage => new MemoryPage
    {
        processId = -1
    };

    public MemoryPage(Process process)
    {
        processId = process.id;
        index = process.index;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return processId == ((MemoryPage)obj).processId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(MemoryPage page1, MemoryPage page2)
    {
        return page1.Equals(page2);
    }

    public static bool operator !=(MemoryPage page1, MemoryPage page2)
    {
        return !page1.Equals(page2);
    }
}
