using Modules.Entities;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class TargetObject : MonoBehaviour, IComponentSavable
    {
        ///Variable
        [field: SerializeField]
        public Entity Value { get; set; }
        
        public void Accept(IComponentVisitor visitor) => visitor.Visit(this);
    }
}