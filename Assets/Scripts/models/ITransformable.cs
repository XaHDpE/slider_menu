using UnityEngine;

namespace models
{
    public interface ITransformable
    {
        void Rotate(float xAngle, float yAngle, float zAngle);
        void Scale(Vector3 finalScale, float time);
        void Move(Vector3 finalPosition, float time);
        void MoveScale(Vector3 finalPosition, Vector3 finalScale, float time);
        void MoveRotateScale(Vector3 finalPosition, Quaternion finalRotation, Vector3 finalScale, float time);
        
    }
}