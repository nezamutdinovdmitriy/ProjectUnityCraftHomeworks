using System.Collections.Generic;
using Modules.AudioEvents;
using Unity.Collections;

namespace Game.Presentation.Audio
{
    public sealed class AudioEventKeyCache
    {
        private readonly Dictionary<FixedString32Bytes, AudioEventKey> _keys = new();

        public bool TryGet(in FixedString32Bytes name, out AudioEventKey key)
        {
            if (name.Length == 0)
            {
                key = default;
                return false;
            }

            if (_keys.TryGetValue(name, out key))
                return true;

            key = new AudioEventKey(name.ToString());
            _keys.Add(name, key);
            return true;
        }
    }
}