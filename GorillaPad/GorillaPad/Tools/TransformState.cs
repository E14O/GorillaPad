using UnityEngine;

namespace GorillaPad.Tools
{
    public readonly struct TransformState
    {
        public Vector3 Pos { get; }
        public Quaternion Rot { get; }
        public Vector3 Scale { get; }

        public TransformState(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Pos = position;
            Rot = rotation;
            Scale = scale;
        }
    }
}
