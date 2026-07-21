using UnityEngine;

namespace Game.EntityContext.Core.Fire
{
    public static class FireUseCase
    {
        public static void Fire(this IEntityContext entity)
        {
            Debug.Log("FIRED");
        }
    }
}