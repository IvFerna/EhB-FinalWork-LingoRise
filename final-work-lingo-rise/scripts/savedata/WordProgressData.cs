public class WordProgressData
{
    public int TotalExposureCount =>
     HeardCount + SeenCount + InteractionCount;

    public int HeardCount { get; set; } = 0;

    public int SeenCount { get; set; } = 0;

    public int InteractionCount { get; set; } = 0;

    public bool IsUnlocked { get; set; } = false;
}