using UnityEngine;

namespace Assets.Scripts.Abstractions
{
    public interface IObjectTweetner
    {
        void MoveTo(Transform transform, Vector3 targetPosition);
    }
}
