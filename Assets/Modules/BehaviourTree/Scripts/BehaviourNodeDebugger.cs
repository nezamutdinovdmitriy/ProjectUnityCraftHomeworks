using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Modules.AI
{
    [AddComponentMenu("AI/BehaviourTree/BehaviourNode «Debugger»")]
    public sealed class BehaviourNodeDebugger : MonoBehaviour
    {
        private sealed class NodeInfo
        {
            public int Depth;

            public Action StartedHandler;
            public Action StoppedHandler;
            public Action AbortedHandler;

            public float StartedTime;
        }

        [Space]
        [SerializeField]
        private BehaviourNode _root;

        private readonly Dictionary<BehaviourNode, NodeInfo> _infos = new();

        private void Awake()
        {
            if (_root == null)
            {
                Debug.LogError("[BT] Root is null", this);
                return;
            }

            TraverseAndSubscribe(_root, 0);

            Debug.Log(
                $"<color=#6BCBFF>[BT]</color> Debugger attached to <b>{_root.name}</b>",
                _root);
        }

        private void OnDestroy()
        {
            foreach ((BehaviourNode node, NodeInfo info) in _infos)
            {
                node.OnStarted -= info.StartedHandler;
                node.OnStopped -= info.StoppedHandler;
                node.OnAborted -= info.AbortedHandler;
            }

            _infos.Clear();

            Debug.Log(
                "<color=#6BCBFF>[BT]</color> Debugger disposed",
                this);
        }

        private void TraverseAndSubscribe(BehaviourNode node, int depth)
        {
            if (node == null)
                return;

            if (_infos.ContainsKey(node))
                return;

            NodeInfo info = new()
            {
                Depth = depth
            };

            info.StartedHandler = () => OnNodeStarted(node);
            info.StoppedHandler = () => OnNodeStopped(node);
            info.AbortedHandler = () => OnNodeAborted(node);

            _infos[node] = info;

            node.OnStarted += info.StartedHandler;
            node.OnStopped += info.StoppedHandler;
            node.OnAborted += info.AbortedHandler;

            if (node is IBehaviourNodeDecorator wrapper)
            {
                TraverseAndSubscribe(wrapper.Child, depth + 1);
            }

            if (node is IBehaviourNodeComposite composite)
            {
                foreach (BehaviourNode child in composite.Nodes)
                {
                    TraverseAndSubscribe(child, depth + 1);
                }
            }
        }

        private void OnNodeStarted(BehaviourNode node)
        {
            NodeInfo info = _infos[node];

            info.StartedTime = Time.realtimeSinceStartup;

            Debug.Log(
                $"{BuildPrefix(info.Depth)}" +
                $"<color=#00FFAA>▶ START</color> " +
                $"{BuildNodeLabel(node)}",
                node);
        }

        private void OnNodeStopped(BehaviourNode node)
        {
            NodeInfo info = _infos[node];

            float duration = Time.realtimeSinceStartup - info.StartedTime;

            Debug.Log(
                $"{BuildPrefix(info.Depth)}" +
                $"{BuildResultLabel(node.Result)} " +
                $"{BuildNodeLabel(node)} " +
                $"<color=#888888>({duration:0.000}s)</color>",
                node);
        }

        private void OnNodeAborted(BehaviourNode node)
        {
            NodeInfo info = _infos[node];

            float duration = Time.realtimeSinceStartup - info.StartedTime;

            Debug.LogWarning(
                $"{BuildPrefix(info.Depth)}" +
                $"<color=#FFAA00>■ ABORT</color> " +
                $"{BuildNodeLabel(node)} " +
                $"<color=#888888>({duration:0.000}s)</color>",
                node);
        }

        private static string BuildNodeLabel(BehaviourNode node)
        {
            return node switch
            {
                IBehaviourNodeComposite =>
                    $"<b><color=#6BCBFF>{node.name}</color></b>",

                IBehaviourNodeDecorator =>
                    $"<b><color=#C792EA>{node.name}</color></b>",

                _ =>
                    $"<color=#DDDDDD>{node.name}</color>"
            };
        }

        private static string BuildResultLabel(BehaviourResult result)
        {
            return result switch
            {
                BehaviourResult.Success =>
                    "<color=#55FF55>✔ SUCCESS</color>",

                BehaviourResult.Failure =>
                    "<color=#FF5555>✘ FAILURE</color>",

                BehaviourResult.Aborted =>
                    "<color=#FFAA00>■ ABORTED</color>",

                BehaviourResult.Running =>
                    "<color=#AAAAAA>… RUNNING</color>",

                _ =>
                    $"<color=#FFFFFF>{result}</color>"
            };
        }

        private static string BuildPrefix(int depth)
        {
            if (depth <= 0)
                return string.Empty;

            StringBuilder sb = new();

            for (int i = 0; i < depth; i++)
            {
                sb.Append("│   ");
            }

            return sb.ToString();
        }
    }
}