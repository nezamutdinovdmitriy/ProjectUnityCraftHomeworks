using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class DestinationPoint : MonoBehaviour, IComponentSavable
    {
        ///Variable
        [field: SerializeField]
        public Vector3 Value { get; set; }
        
        public void Accept(IComponentVisitor visitor) => visitor.Visit(this);
    }
}