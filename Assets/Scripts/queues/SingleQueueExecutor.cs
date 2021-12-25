using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace helpers
{
    public class SingleQueueExecutor
    {
        private static SingleQueueExecutor instance;
        private List<IEnumerator> _procs;
        private readonly MonoBehaviour _context;

        private Coroutine crt;

        private Coroutine _coroutineOne;

        public SingleQueueExecutor(MonoBehaviour context)
        {
            _procs = new List<IEnumerator>();
            _context = context;
        }
 
        /*public static QueueExecutor GetInstance()
        {
            return instance ?? (instance = new QueueExecutor());
        }*/

        public void Add(IEnumerator coroutine)
        {
            _procs.Add(coroutine);
        }

        public List<IEnumerator> GetCoroutines()
        {
            return _procs;
        }

        public void StartOneForced(IEnumerator coroutine)
        {
            if (_coroutineOne != null)
                _context.StopAllCoroutines();
            _coroutineOne = _context.StartCoroutine(coroutine);;
        }

        private IEnumerator Execute(Action callback)
        {
            var cors = new Coroutine[_procs.Count];
            for (var i = 0; i < _procs.Count; i++)
            {
                cors[i] = _context.StartCoroutine(_procs[i]);
            }
            
            foreach (var cor in cors)
            {
                yield return cor;
            }
            // empty list
            _procs = new List<IEnumerator>();
            // call back function
            callback();
        }

        public void StartForced(Action callback)
        {
            if (crt != null) 
                _context.StopAllCoroutines();
            crt = _context.StartCoroutine(Execute(callback));
        }

    }
}