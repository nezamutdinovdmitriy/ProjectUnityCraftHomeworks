namespace Modules.AudioEvents
{
    /// <summary>
    /// Generates sequential integer IDs for audio events.
    /// </summary>
    public sealed class SequentialAudioIdGenerator : IAudioIdGenerator
    {
        private const int INITIAL_ID = 1;

        private readonly int _initialId;
        private int _nextId;

        public SequentialAudioIdGenerator(int nextId = INITIAL_ID)
        {
            _initialId = nextId;
            _nextId = nextId;
        }

        public int GenerateId(string name) => _nextId++;

        public void Reset() => _nextId = _initialId;
    }
}