public class GenerateLevelEventMessage : EventMessage {

    public readonly int LevelIdx;

    public GenerateLevelEventMessage(int levelIdx)
    {
        LevelIdx = levelIdx;
    }
}
