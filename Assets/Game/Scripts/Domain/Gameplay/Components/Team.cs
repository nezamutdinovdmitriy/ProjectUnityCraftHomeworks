using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Team : MonoBehaviour, IComponentSavable
    {
        ///Variable
        [field: SerializeField]
        public TeamType Type { get; set; }
        
        public void Accept(IComponentVisitor visitor) => visitor.Visit(this);
    }
}