public struct MemoryUnitState
{
    public MemoryPage[] Pages { get; set; }
    public int PageFaults { get; set; }
    public int MemorySize { get { return Pages.Length; } }
}
