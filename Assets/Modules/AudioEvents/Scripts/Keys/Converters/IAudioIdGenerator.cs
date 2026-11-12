namespace Modules.AudioEvents
{
    /// <summary>
    /// Generates audio event names into integer IDs.
    /// </summary>
    public interface IAudioIdGenerator
    {
        int GenerateId(string name);

        void Reset();
    }
}