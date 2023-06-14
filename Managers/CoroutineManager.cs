using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeIdle.Scripts.Managers
{
    public class CoroutineManager : MonoBehaviour
    {
        public Coroutine SetTimeout(float duration, Action  callback)
        {
            return StartCoroutine(SetTimer(duration, false, callback));
        }
        
        public Coroutine SetImmediate(float duration, bool repeat, Action callback)
        {
            return StartCoroutine(SetTimer(duration, repeat, callback));
        }
        
        private IEnumerator SetTimer(float duration, bool repeat, Action callback)
        {
            do
            {
                yield return new WaitForSeconds(duration);
                if (callback != null)
                    callback();
            } while (repeat);
        }
        
        public Coroutine StartAsync(IEnumerator coroutine)
        {
            if(coroutine != null) return StartCoroutine(coroutine);
            return null;
        }
        
        public void Stop(Coroutine coroutine)
        {
            if(coroutine != null) StopCoroutine(coroutine);
        }
        
        public void StopCoroutines(List<Coroutine> coroutines)
        {
            foreach (var coroutine in coroutines)
            {
                if(coroutine != null) Stop(coroutine);
            }
        }
        
        public void StopAll()
        {
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            StopAll();
        }
    }
}