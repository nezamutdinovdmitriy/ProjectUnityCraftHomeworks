using System;
using System.Collections.Generic;
using Modules.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SampleGame.AI
{
    public class FSMCommandNode : BehaviourNode
    {
        [Serializable]
        private struct CommandNodeMapping
        {
            [HideLabel, HorizontalGroup]
            public CommandType Command;
            
            [HideLabel, HorizontalGroup]
            public BehaviourNode Node;
        }

        [Space]
        [SerializeField]
        private Blackboard _blackboard;
        
        [Space]
        [SerializeField, HideInPlayMode]
        private CommandNodeMapping[] _allNodes = Array.Empty<CommandNodeMapping>();

        [ShowInInspector, HideInEditorMode]
        private readonly Dictionary<CommandType, BehaviourNode> _nodeMapping = new();

        [ShowInInspector, HideInEditorMode]
        private BehaviourNode _currentNode;

        [Space]
        [ShowInInspector, HideInEditorMode]
        private ICommandData _currentCommand;

        private void Start() => BuildNodeMapping();
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if(_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out _currentCommand)
               && _nodeMapping.TryGetValue(_currentCommand.Type, out _currentNode)
               && _currentNode != null)
                return _currentNode.Run(deltaTime);

            return BehaviourResult.Failure;
        }

        protected override void OnAbort()
        {
            base.OnAbort();
        
            if (_currentNode != null && _currentNode.IsRunning)
                _currentNode.Abort();
        
            _currentNode = null;

            Vector3 currentPosition = _blackboard.GetValue(BlackboardAPI.Character).transform.position;
            _currentCommand = new DefaultCommandData(new CommandPoint(currentPosition));
        }

        private void BuildNodeMapping()
        {
            _nodeMapping.Clear();

            foreach (CommandNodeMapping mapping in _allNodes)
                if (_nodeMapping.TryAdd(mapping.Command, mapping.Node) == false)
                    throw new InvalidOperationException($"Duplicate CommandType: {mapping.Command}");
        }
    }
}