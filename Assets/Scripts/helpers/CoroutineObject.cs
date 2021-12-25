using System;
using System.Collections;
using UnityEngine;

namespace helpers
{
    public sealed class CoroutineObject
    {
        public MonoBehaviour Owner { get; private set; }
        public Coroutine Coroutine { get; private set; }
        public Func<IEnumerator> Routine { get; private set; }

        public bool IsProcessing => Coroutine != null;

        public CoroutineObject(MonoBehaviour owner, Func<IEnumerator> routine)
        {
            Owner = owner;
            Routine = routine;
        }

        private IEnumerator Process()
        {
            yield return Routine.Invoke();
            Coroutine = null;
        }

        public void Start()
        {
            Stop();
            Coroutine = Owner.StartCoroutine(Process());
        }

        public void Stop()
        {
            if (IsProcessing)
            {
                Owner.StopCoroutine(Coroutine);
                Coroutine = null;
            }
        }
    }
}