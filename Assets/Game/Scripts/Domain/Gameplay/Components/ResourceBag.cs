using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    public struct ResourceBagData
    {
        public ResourceType Type;
        public int Current;
    }
    
    //Can be extended
    public sealed class ResourceBag : MonoBehaviour, IComponentSavable
    {
        ///Variable
        [field: SerializeField]
        public ResourceType Type { get; set; }
        
        ///Variable
        [field: SerializeField]
        public int Current { get; set; }
        
        ///Const
        [field: SerializeField]
        public int Capacity { get; set; }
        
        public void Accept(IComponentVisitor visitor) => visitor.Visit(this);
    }
}