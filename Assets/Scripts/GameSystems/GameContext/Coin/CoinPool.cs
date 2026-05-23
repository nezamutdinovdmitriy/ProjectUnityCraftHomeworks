using UnityEngine;
using Zenject;

namespace GameSystems.Coin
{
    public class CoinPool : MonoMemoryPool<Vector2Int, Modules.Coin>
    {
        protected override void Reinitialize(Vector2Int position, Modules.Coin coin)
        {
            coin.Position = position;
            coin.Generate();
        }
    }
}