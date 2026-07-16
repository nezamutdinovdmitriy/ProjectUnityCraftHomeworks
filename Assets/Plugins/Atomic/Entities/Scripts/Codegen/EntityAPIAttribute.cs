// using System;
//
// namespace Atomic.Entities
// {
//     [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
//     public sealed class EntityAPIAttribute : Attribute
//     {
//         /// <summary>
//         /// The namespace for the generated API class.
//         /// Optional - if not specified, uses the namespace of the source class.
//         /// </summary>
//         public string? Namespace { get; set; }
//
//         /// <summary>
//         /// The name of the generated static class.
//         /// Optional - if not specified, uses the name of the source class.
//         /// </summary>
//         public string? ClassName { get; set; }
//
//         /// <summary>
//         /// The output directory for the generated file (relative to project root).
//         /// Optional - if not specified, generates in the same directory as the source file.
//         /// </summary>
//         public string? Directory { get; set; }
//
//         /// <summary>
//         /// The entity type that extension methods will target (default: typeof(IEntity)).
//         /// </summary>
//         /// <example>
//         /// EntityType = typeof(IEntity)
//         /// EntityType = typeof(IMyCustomEntity)
//         /// </example>
//         public Type? EntityType { get; set; }
//
//         /// <summary>
//         /// Enable aggressive inlining for generated methods (default: true).
//         /// </summary>
//         public bool AggressiveInlining { get; set; } = true;
//
//         /// <summary>
//         /// Enable unsafe access with ref returns for value types (default: true).
//         /// </summary>
//         public bool UnsafeAccess { get; set; } = true;
//
//         /// <summary>
//         /// Namespaces to exclude from auto-detected imports.
//         /// Use this to prevent certain using directives from being copied to the generated file.
//         /// </summary>
//         public string[]? ExcludeImports { get; set; }
//
//         /// <summary>
//         /// Target .csproj file to add generated file to (relative to project root).
//         /// Optional - if not specified, auto-detects all projects containing this source file.
//         /// </summary>
//         public string? TargetProject { get; set; }
//     }
// }
