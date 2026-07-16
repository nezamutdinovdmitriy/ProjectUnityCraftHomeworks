// using System;
//
// namespace Atomic.Entities
// {
//     /// <summary>
//     /// Links a behaviour class to an EntityAPI definition.
//     /// The behaviour will be included in the generated API with extension methods.
//     /// </summary>
//     /// <example>
//     /// [LinkTo(typeof(MyEntityAPI))]
//     /// public class JumpBehaviour : IEntityInit, IEntityTick { }
//     /// </example>
//     [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
//     public sealed class LinkToAttribute : Attribute
//     {
//         /// <summary>
//         /// The EntityAPI type this behaviour is linked to.
//         /// </summary>
//         public Type ApiType { get; }
//
//         public LinkToAttribute(Type apiType)
//         {
//             ApiType = apiType;
//         }
//     }
// }