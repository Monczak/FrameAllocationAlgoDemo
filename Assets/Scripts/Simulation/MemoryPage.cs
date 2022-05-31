public struct MemoryPage
{
    public int processId;
    public int pageId;
    public int index;

    public Process process;

    public static MemoryPage NullPage => new MemoryPage
    {
        processId = -1,
        pageId = -1,
        process = null
    };

    public MemoryPage(Process process, int id)
    {
        processId = process.id;
        pageId = id;
        index = process.index;
        this.process = process;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return processId == ((MemoryPage)obj).processId && pageId == ((MemoryPage)obj).pageId;
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
