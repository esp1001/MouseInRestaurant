public class AddOneMouseEventMessage : EventMessage {

    public AddOneMouseEventMessage()
    {
        LevelInPlayManager.Instance.MouseTappedCount++;
    }
}
