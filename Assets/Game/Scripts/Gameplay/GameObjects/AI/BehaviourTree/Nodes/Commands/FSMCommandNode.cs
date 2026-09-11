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

        [SerializeField]
        private BehaviourNode _defaultNode;
        
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

        private bool _init;
        
        protected override void OnStart()
        {
            if (_init == false)
            {
                BuildNodeMapping();
                _init = true;
            }
        }

        protected override void OnStop(BehaviourResult result) 
            => StopCurrentNode();

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.HasValue(BlackboardAPI.CurrentCommand) == false)
            {
                CommandPoint commandPoint = new CommandPoint(_blackboard.GetValue(BlackboardAPI.Character).transform.position);
                _currentCommand = new DefaultCommandData(commandPoint);
                _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, _currentCommand);
                
                SwitchCurrentNode(_defaultNode);
                return _currentNode != null ? _currentNode.Run(deltaTime) : BehaviourResult.Failure;
            }
            
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData newCommand))
            {
                _currentCommand = newCommand;

                if (_nodeMapping.TryGetValue(_currentCommand.Type, out BehaviourNode targetNode))
                {
                    SwitchCurrentNode(targetNode);
                    return _currentNode.Run(deltaTime);
                }

                if (_currentCommand is { Type: CommandType.Default })
                {
                    SwitchCurrentNode(_defaultNode);
                    return _currentNode != null ? _currentNode.Run(deltaTime) : BehaviourResult.Failure;
                }
            }
            
            SwitchCurrentNode(null);
            return BehaviourResult.Failure;
        }

        protected override void OnAbort() => StopCurrentNode();

        private void SwitchCurrentNode(BehaviourNode newNode)
        {
            if (_currentNode == newNode)
                return;

            StopCurrentNode();
            _currentNode = newNode;
        }
        
        private void StopCurrentNode()
        {
            if (_currentNode != null && _currentNode.IsRunning)
                _currentNode.Abort();
            
            _currentNode = null;
        }
        
        private void BuildNodeMapping()
        {
            _nodeMapping.Clear();

            foreach (CommandNodeMapping mapping in _allNodes)
            {
                if (_nodeMapping.TryAdd(mapping.Command, mapping.Node) == false)
                    throw new InvalidOperationException($"Duplicate CommandType: {mapping.Command}");
            }
        }
    }
}