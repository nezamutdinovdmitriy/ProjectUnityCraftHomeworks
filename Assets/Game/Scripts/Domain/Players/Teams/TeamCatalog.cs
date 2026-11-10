using System;
using System.Collections.Generic;
using Game.Components;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(
        fileName = "TeamCatalog",
        menuName = "SampleGame/Common/New TeamCatalog"
    )]
    public sealed class TeamCatalog : ScriptableObject
    {
        [SerializeField]
        private TeamInfo[] _teams;

        public TeamInfo GetTeam(TeamType teamType)
        {
            for (int i = 0, count = _teams.Length; i < count; i++)
            {
                TeamInfo info = _teams[i];
                if (info.Type == teamType)
                    return info;
            }

            throw new KeyNotFoundException($"Team of type {teamType} is not found!");
        }

        [Serializable]
        public sealed class TeamInfo
        {
            [SerializeField]
            private TeamType type;

            [SerializeField]
            private Material material;

            [SerializeField] 
            private Color color;

            public Material Material
            {
                get { return this.material; }
            }

            public TeamType Type
            {
                get { return type; }
            }

            public Color Color
            {
                get { return color; }
            }
        }
    }
}