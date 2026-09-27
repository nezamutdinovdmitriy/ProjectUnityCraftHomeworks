using System;
using System.Collections.Generic;
using Modules.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SampleGame.AI
{
    public class FSMCommandNode : BehaviourNode, IBehaviourNodeComposite
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

        private bool _initialized;

        public IEnumerable<BehaviourNode> Nodes
        {
            get
            {
                foreach (CommandNodeMapping mapping in _allNodes)
                    if (mapping.Node != null)
                        yield return mapping.Node;
            }
        }

        protected override void OnStart()
        {
            if (_initialized)
                return;

            BuildNodeMapping();
            _initialized = true;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData command) == false)
                return BehaviourResult.Failure;

            if (ReferenceEquals(_currentCommand, command) == false)
                SwitchCommand(command);

            if (_currentNode == null)
                return BehaviourResult.Failure;

            return _currentNode.Run(deltaTime);
        }

        protected override void OnAbort()
            => StopCurrentNode();

        protected override void OnStop(BehaviourResult result)
        {
            StopCurrentNode();
            CleanupCurrentCommand();
        }

        private void BuildNodeMapping()
        {
            _nodeMapping.Clear();

            foreach (CommandNodeMapping mapping in _allNodes)
                if (_nodeMapping.TryAdd(mapping.Command, mapping.Node) == false)
                    throw new InvalidOperationException($"Duplicate CommandType: {mapping.Command}");
        }

        private void SwitchCommand(ICommandData command)
        {
            StopCurrentNode();
            CleanupCurrentCommand();

            _currentCommand = command;
            _currentCommand.Unpack(_blackboard);

            if (_nodeMapping.TryGetValue(_currentCommand.Type, out _currentNode) == false)
            {
                throw new InvalidOperationException(
                    $"No BehaviourNode mapped for CommandType: {_currentCommand.Type}");
            }
        }

        private void StopCurrentNode()
        {
            if (_currentNode != null && _currentNode.IsRunning)
                _currentNode.Abort();

            _currentNode = null;
        }

        private void CleanupCurrentCommand()
        {
            _currentCommand?.Cleanup(_blackboard);
            _currentCommand = null;
        }
    }
}