using System;
using Modules.AI;
using SampleGame.AI;
using UnityEngine;

namespace SampleGame
{
    public class CommandMarkerPresenter : MonoBehaviour
    {
        [SerializeField]
        private CommandMarkerView _view;

        [SerializeField]
        private Blackboard _blackboard;

        private void OnEnable()
        {
            _blackboard.OnTagAdded += OnTagAdded;
        }

        private void OnTagAdded(int key)
        {
            ICommandData commandData = _blackboard.GetValue(BlackboardAPI.CurrentCommand);
            
            if (key == BlackboardAPI.MoveCommandTag)
            {
                if (commandData is not MoveCommandData moveCommandData)
                    return;

                if (moveCommandData.Point.Position.HasValue)
                {
                    Vector3 position = moveCommandData.Point.Position.Value;
                    _view.ShowMoveMarker(position);
                }

                if (moveCommandData.Point.Target != null)
                {
                    Transform targetTransform = moveCommandData.Point.Target.transform;
                    _view.ShowMoveMarker(targetTransform);
                }
                
                _blackboard.DelTag(BlackboardAPI.MoveCommandTag);
            }
            
            // для других комманд сделать такую же обработку?
        }
    }
}