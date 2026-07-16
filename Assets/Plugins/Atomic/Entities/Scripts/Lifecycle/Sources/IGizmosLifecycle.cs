#if UNITY_EDITOR
using System;

namespace Atomic.Entities
{
    public interface IGizmosLifecycle
    {
        event Action OnGizmosDraw;
    }
}
#endif