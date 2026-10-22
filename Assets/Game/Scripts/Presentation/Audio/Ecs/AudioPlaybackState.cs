using Unity.Entities;

namespace Game.Presentation.Audio.Ecs
{
    public sealed class AudioPlaybackState : IComponentData
    {
        public AudioPlayback Playback;
    }
}