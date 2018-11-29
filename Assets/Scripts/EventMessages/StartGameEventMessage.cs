public class StartGameEventMessage : EventMessage {

    public readonly int StartLevelIdx;

    public StartGameEventMessage(int startLevelIdx)
    {
        this.StartLevelIdx = startLevelIdx;
    }
}
