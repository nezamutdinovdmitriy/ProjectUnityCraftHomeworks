// using UnityEngine;
//
// namespace Game
// {
//     public class TransformComponent
//     {
//         private readonly Transform _transform;
//
//         public TransformComponent(Transform transform) => _transform = transform;
//
//         public Vector3 Position
//         {
//             get => _transform.position;
//             set => _transform.position = value;
//         }
//
//         public Quaternion Rotation
//         {
//             get => _transform.rotation;
//             set => _transform.rotation = value;
//         }
//
//         public Vector3 Right => _transform.right;
//
//         public Transform Parent
//         {
//             get => _transform.parent;
//             set => _transform.parent = value;
//         }
//
//         public Vector3 EulerAngles
//         {
//             get => _transform.eulerAngles;
//             set => _transform.eulerAngles = value;
//         }
//
//         public void Translate(Vector3 translation, Space relativeTo) 
//             => _transform.Translate(translation, relativeTo);
//     }
// }