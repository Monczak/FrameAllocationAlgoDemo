public class BackAndForthPattern : ISequenceGenerationPattern
{
    private int limit;

    public BackAndForthPattern(int limit)
    {
        this.limit = limit;
    }

    public int GetIndex(int i)
    {
        if (i % (limit * 2) < limit)
            return i % (limit * 2);
        return limit * 2 - i % (limit * 2);
    }
}