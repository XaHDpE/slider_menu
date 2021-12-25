using helpers;
using models.sparepart.iteration2;
using UnityEngine;
using th = helpers.TransformHelper;

namespace models
{
    public abstract class TransformableAbstract : MonoBehaviour, ITransformable
    {
        private SingleQueueExecutor _sqe;

        private void OnEnable()
        {
            _sqe = new SingleQueueExecutor(this);
        }

        public void Scale(Vector3 finalScale, float time)
        {
            var tr = transform;
            _sqe.StartOneForced(th.ChangeScaleByTime(tr, tr.localScale, finalScale, time));
        }

        public void Move(Vector3 finalPosition, float time)
        {
            var tr = transform;
            _sqe.StartOneForced(th.MoveFromToByTime(tr, tr.localScale, finalPosition, time));
        }

        public void MoveScale(Vector3 finalPosition, Vector3 finalScale, float time)
        {
            var tr = transform;
            _sqe.Add(th.MoveFromToByTime(tr, tr.position, finalPosition, time));
            _sqe.Add(th.ChangeScaleByTime(tr, tr.localScale, finalScale, time));
            _sqe.StartForced(() =>
            {
                Debug.Log("MoveScale done.");
            });
        }
        
        public void MoveRotateScale(Vector3 finalPosition, Quaternion finalRotation, Vector3 finalScale, float time)
        {
            var tr = transform;
            _sqe.Add(th.MoveFromToByTime(tr, tr.position, finalPosition, time));
            _sqe.Add(th.RotateByTime(tr, finalRotation, time));
            _sqe.Add(th.ChangeScaleByTime(tr, tr.localScale, finalScale, time));
            _sqe.StartForced(() =>
            {
                Debug.Log("MoveRotateScale done.");
            });
        }

        public void Rotate(float xAngle, float yAngle, float zAngle)
        {
            transform.Rotate(xAngle, yAngle, zAngle);
        }
    }
}