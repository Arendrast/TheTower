using System;
using System.Collections;
using UnityEngine;

namespace General
{
    public class Timer
    {
        public event Action<float> OnUpdate;
        public Action OnTimeIsOver;
        public Action OnInterruptedIncomplete;

        private float _time;
        private float _remainingTime;

        private MonoBehaviour _context;
        private IEnumerator _countDown;

        public Timer(MonoBehaviour context) => _context = context;

        public void Set(float time)
        {
            _time = time;
            _remainingTime = time;
        }

        public void StartCountingTime(bool isScaled)
        {
            _countDown = CountDown(isScaled);
            _context.StartCoroutine(_countDown);
        }

        public void StopCountingTime()
        {
            if (_countDown != null)
                _context.StopCoroutine(_countDown);
        
            if (_remainingTime >= 0)
                OnInterruptedIncomplete?.Invoke();
        }

        private IEnumerator CountDown(bool isScaled)
        {
            while (_remainingTime >= 0)
            {
                _remainingTime -= isScaled ? Time.deltaTime : Time.unscaledDeltaTime;
                OnUpdate?.Invoke(_remainingTime);
            
                yield return null;
            }
        
            OnTimeIsOver?.Invoke();
        }
    }
}
