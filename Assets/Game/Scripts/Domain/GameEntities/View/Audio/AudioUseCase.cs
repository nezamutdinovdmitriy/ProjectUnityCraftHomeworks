using Game.Presentation.Audio;
using Modules.AudioEvents;
using Unity.Collections;
using UnityEngine;

namespace Game.UseCases
{
    public static class AudioUseCase
    {
        public static void Play(
            AudioSystem audio,
            in FixedString128Bytes name,
            Vector3 position,
            AudioEventKeyCache keyCache)
        {
            if (keyCache.TryGet(in name, out AudioEventKey key))
                audio.PlayEvent(key, position);
        }
    }
}