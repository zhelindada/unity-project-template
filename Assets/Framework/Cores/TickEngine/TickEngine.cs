using System;
using System.Collections.Generic;
using UnityEngine;
using Dada.Foundations;

namespace Dada.Cores.TickEngine
{
    public enum TickSpeed
    {
        x1 = 1,
        x4 = 4,
        x10 = 10,
        x50 = 50
    }

    public class TickEngine : MonoSingleton<TickEngine>
    {
        [SerializeField] private TickSpeed _defaultSpeed = TickSpeed.x1;
        [SerializeField] private bool _autoStart = false;

        public int TotalTicks { get; private set; }
        public TickSpeed CurrentSpeed { get; private set; }
        public bool IsPaused { get; private set; } = true;

        private float _accumulator;
        private readonly List<ITickListener> _listeners = new();
        private readonly List<ITickListener> _pendingAdd = new();
        private readonly List<ITickListener> _pendingRemove = new();

        public event Action<int> OnTicked;
        public event Action<TickSpeed> OnSpeedChanged;
        public event Action<bool> OnPauseChanged;

        protected override void OnAwake()
        {
            base.OnAwake();
            CurrentSpeed = _defaultSpeed;
        }

        private void Update()
        {
            if (IsPaused) return;

            float tickInterval = SpeedToInterval(CurrentSpeed);
            _accumulator += Time.deltaTime;

            while (_accumulator >= tickInterval)
            {
                _accumulator -= tickInterval;
                AdvanceOneTick();
            }
        }

        public void AdvanceOneTick()
        {
            TotalTicks++;
            ProcessPendingLists();
            foreach (var listener in _listeners)
                listener.OnTick(TotalTicks);
            OnTicked?.Invoke(TotalTicks);
        }

        public void Play()
        {
            if (!IsPaused) return;
            IsPaused = false;
            OnPauseChanged?.Invoke(false);
        }

        public void Pause()
        {
            if (IsPaused) return;
            IsPaused = true;
            OnPauseChanged?.Invoke(true);
        }

        public void TogglePause()
        {
            if (IsPaused) Play(); else Pause();
        }

        public void SetSpeed(TickSpeed speed)
        {
            if (CurrentSpeed == speed) return;
            CurrentSpeed = speed;
            _accumulator = 0f;
            OnSpeedChanged?.Invoke(speed);
        }

        public void Register(ITickListener listener)
        {
            _pendingAdd.Add(listener);
        }

        public void Unregister(ITickListener listener)
        {
            _pendingRemove.Add(listener);
        }

        private void ProcessPendingLists()
        {
            foreach (var l in _pendingAdd)
                if (!_listeners.Contains(l))
                    _listeners.Add(l);
            _pendingAdd.Clear();

            foreach (var l in _pendingRemove)
                _listeners.Remove(l);
            _pendingRemove.Clear();
        }

        public static float SpeedToInterval(TickSpeed speed) => speed switch
        {
            TickSpeed.x1 => 2.0f,
            TickSpeed.x4 => 0.5f,
            TickSpeed.x10 => 0.2f,
            TickSpeed.x50 => 0.04f,
            _ => 2.0f
        };

        public int GameDay => TotalTicks / 144;
        public int GameHour => (TotalTicks % 144) / 6;
        public int TickInHour => TotalTicks % 6;
        public bool IsHourBoundary => TotalTicks % 6 == 0;
        public bool IsDailyReset => TotalTicks % 144 == 0;
    }
}
