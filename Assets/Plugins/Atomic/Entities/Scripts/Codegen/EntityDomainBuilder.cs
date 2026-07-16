// namespace Atomic.Entities
// {
//     /// <summary>
//     /// Base class for entity domain configuration using fluent API.
//     /// Inherit from this class and override Configure() to define your entity domain.
//     /// </summary>
//     public abstract class EntityDomainBuilder
//     {
//         /// <summary>
//         /// Entity name. Override this property to set the entity name.
//         /// </summary>
//         public abstract string EntityName { get; }
//
//         /// <summary>
//         /// Target namespace for generated files. Override this property to set the namespace.
//         /// </summary>
//         public abstract string Namespace { get; }
//
//         /// <summary>
//         /// Output directory for generated files. Override this property to set the directory.
//         /// </summary>
//         public abstract string Directory { get; }
//
//         /// <summary>
//         /// Configure the entity domain. Override this method and call the configuration methods.
//         /// </summary>
//         public abstract void Configure();
//
//         // ===== ENTITY MODE METHODS =====
//
//         /// <summary>
//         /// Use standard Entity mode (pure runtime entities).
//         /// </summary>
//         protected void EntityMode() { }
//
//         /// <summary>
//         /// Use EntitySingleton mode (single instance entity).
//         /// </summary>
//         protected void EntitySingletonMode() { }
//
//         /// <summary>
//         /// Use SceneEntity mode (entities tied to scene objects).
//         /// </summary>
//         protected void SceneEntityMode() { }
//
//         /// <summary>
//         /// Use SceneEntitySingleton mode (single scene entity).
//         /// </summary>
//         protected void SceneEntitySingletonMode() { }
//
//         // ===== SCENE ENTITY OPTIONS =====
//
//         /// <summary>
//         /// Generate SceneEntityProxy (MonoBehaviour wrapper for scene interaction).
//         /// Only valid for SceneEntity modes.
//         /// </summary>
//         protected void GenerateProxy() { }
//
//         /// <summary>
//         /// Generate SceneEntityWorld (world container for scene entities).
//         /// Only valid for SceneEntity modes.
//         /// </summary>
//         protected void GenerateWorld() { }
//
//         // ===== INSTALLER METHODS =====
//
//         /// <summary>
//         /// Generate IEntityInstaller interface.
//         /// </summary>
//         protected void IEntityInstaller() { }
//
//         /// <summary>
//         /// Generate ScriptableEntityInstaller (ScriptableObject-based installer).
//         /// </summary>
//         protected void ScriptableEntityInstaller() { }
//
//         /// <summary>
//         /// Generate SceneEntityInstaller (MonoBehaviour-based installer).
//         /// </summary>
//         protected void SceneEntityInstaller() { }
//
//         // ===== ASPECT METHODS =====
//
//         /// <summary>
//         /// Generate ScriptableEntityAspect (ScriptableObject-based aspect).
//         /// </summary>
//         protected void ScriptableEntityAspect() { }
//
//         /// <summary>
//         /// Generate SceneEntityAspect (MonoBehaviour-based aspect).
//         /// </summary>
//         protected void SceneEntityAspect() { }
//
//         // ===== POOL METHODS =====
//
//         /// <summary>
//         /// Generate SceneEntityPool (pool for scene entities).
//         /// Only valid for SceneEntity modes.
//         /// </summary>
//         protected void SceneEntityPool() { }
//
//         /// <summary>
//         /// Generate PrefabEntityPool (pool for prefab-based entities).
//         /// Only valid for SceneEntity modes.
//         /// </summary>
//         protected void PrefabEntityPool() { }
//
//         // ===== FACTORY METHODS =====
//
//         /// <summary>
//         /// Generate ScriptableEntityFactory (ScriptableObject-based factory).
//         /// Only valid for Entity modes (not SceneEntity).
//         /// </summary>
//         protected void ScriptableEntityFactory() { }
//
//         /// <summary>
//         /// Generate SceneEntityFactory (MonoBehaviour-based factory).
//         /// Only valid for Entity modes (not SceneEntity).
//         /// </summary>
//         protected void SceneEntityFactory() { }
//
//         // ===== BAKER METHODS =====
//
//         /// <summary>
//         /// Generate standard baker for Unity DOTS conversion.
//         /// Only valid for Entity modes (not SceneEntity).
//         /// </summary>
//         protected void StandardBaker() { }
//
//         /// <summary>
//         /// Generate optimized baker with RequireComponent attribute.
//         /// Only valid for Entity modes (not SceneEntity).
//         /// </summary>
//         protected void OptimizedBaker() { }
//
//         // ===== VIEW METHODS =====
//
//         /// <summary>
//         /// Generate EntityView (MonoBehaviour view component).
//         /// Only valid for Entity modes (not SceneEntity).
//         /// </summary>
//         protected void EntityView() { }
//
//         /// <summary>
//         /// Generate EntityViewCatalog (ScriptableObject catalog for views).
//         /// Only valid for Entity modes (not SceneEntity).
//         /// </summary>
//         protected void EntityViewCatalog() { }
//
//         /// <summary>
//         /// Generate EntityViewPool (object pool for entity views).
//         /// Only valid for Entity modes (not SceneEntity).
//         /// </summary>
//         protected void EntityViewPool() { }
//
//         /// <summary>
//         /// Generate EntityCollectionView (view for entity collections).
//         /// Only valid for Entity modes (not SceneEntity).
//         /// </summary>
//         protected void EntityCollectionView() { }
//
//         // ===== ADVANCED CONFIGURATION =====
//
//         /// <summary>
//         /// Exclude namespaces from auto-import detection.
//         /// </summary>
//         protected void ExcludeImports(params string[] namespaces) { }
//
//         /// <summary>
//         /// Specify target .csproj file for generated files.
//         /// If not specified, auto-detects based on source file location.
//         /// </summary>
//         protected void TargetProject(string projectPath) { }
//     }
// }