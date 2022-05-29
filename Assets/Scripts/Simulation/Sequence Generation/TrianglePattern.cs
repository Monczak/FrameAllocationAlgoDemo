public class TrianglePattern : ISequenceGenerationPattern
{
    public int GetIndex(int i)
    {
        int n = 0;
        for (int j = 0; j < i; j++)
        {
            for (int k = 0; k < j; k++)
            {
                if (n == i)
                    return k;
                n++;
            }
        }
        return 0;
    }
}