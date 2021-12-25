using models;
using UnityEngine;

namespace states.controllers
{
    public interface IStateManager
    {
        TransformableAbstract GetTransformable();
        MonoBehaviour AttachedTo();

    }
}