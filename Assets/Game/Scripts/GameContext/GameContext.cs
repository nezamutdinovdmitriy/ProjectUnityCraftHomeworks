using Atomic.Entities;
using System.Collections.Generic;

/**
 * Created by Entity Domain Generator.
 */

namespace Game.GameContexst
{
    /// <summary>
    /// Abstract base class for singleton entities.
    /// Supports both default constructor and factory-based creation.
    /// </summary>
    public sealed class GameContext : EntitySingleton<GameContext>, IGameContext
    {
        public GameContext()
        {
        }

        /// <summary>
        /// Creates a new entity with the specified name, tags, values, behaviours, and optional settings.
        /// </summary>
        public GameContext(
            string name,
            IEnumerable<string> tags,
            IEnumerable<KeyValuePair<string, object>> values,
            IEnumerable<IEntityBehaviour> behaviours,
            Settings? settings = null
        ) : base(name, tags, values, behaviours, settings)
        {
        }

        /// <summary>
        /// Creates a new entity with the specified name, tags, values, behaviours, and optional settings.
        /// </summary>
        public GameContext(
            string name,
            IEnumerable<int> tags,
            IEnumerable<KeyValuePair<int, object>> values,
            IEnumerable<IEntityBehaviour> behaviours,
            Settings? settings = null
        ) : base(name, tags, values, behaviours, settings)
        {
        }

        /// <summary>
        /// Creates a new entity with the specified name and initial capacities for tags, values, and behaviours.
        /// </summary>
        public GameContext(
            string name = null,
            int tagCapacity = 0,
            int valueCapacity = 0,
            int behaviourCapacity = 0,
            Settings? settings = null
        ) : base(name, tagCapacity, valueCapacity, behaviourCapacity, settings)
        {
        }
    }
}
