namespace Game.Frame.Timer
{
    public interface ITime
    {
        float DeltaMS { get; }

        float RealDeltaMS { get; }

        float StartDurationMS { get; }

        float TimeScale { get; }

        int FrameCount { get; }
    }
}