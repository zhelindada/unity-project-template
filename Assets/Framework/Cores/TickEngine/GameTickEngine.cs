using System;
using System.Collections.Generic;
using UnityEngine;
using Dada.Foundations;

namespace Dada.Cores
{
    public class GameTickEngine : MonoSingleton<GameTickEngine>
    {
        public const string ChannelEveryTick = "EveryTick";

        public float globalSpeed = 1f;
        public bool IsPaused { get; private set; } = true;

        private readonly List<string> _channelKeys = new();
        private readonly Dictionary<string, float> _intervals = new();
        private readonly Dictionary<string, float> _speeds = new();
        private readonly Dictionary<string, float> _accumulators = new();
        private readonly Dictionary<string, int> _tickCounts = new();

        private readonly Dictionary<string, List<ITickReceiver>> _receiversByChannel = new();
        private readonly Dictionary<string, List<ITickReceiver>> _pendingAddByChannel = new();
        private readonly Dictionary<string, List<ITickReceiver>> _pendingRemoveByChannel = new();
        private readonly List<ITickReceiver> _pendingRemoveAll = new();
        private readonly HashSet<ITickReceiver> _firedThisTick = new();

        public event Action<float> OnTicked;
        public event Action<bool> OnPauseChanged;

        protected override void OnAwake()
        {
            base.OnAwake();
            EnsureChannel(ChannelEveryTick, 0f, 1f);
        }

        private void Update()
        {
            if (IsPaused) return;

            float dt = Time.deltaTime * globalSpeed;
            ProcessPendingLists();
            _firedThisTick.Clear();

            for (int i = 0; i < _channelKeys.Count; i++)
            {
                string channel = _channelKeys[i];
                float interval = _intervals[channel];

                if (interval <= 0f)
                {
                    _tickCounts[channel]++;
                    FireChannel(channel, dt);
                    continue;
                }

                _accumulators[channel] += dt * _speeds[channel];
                while (_accumulators[channel] >= interval)
                {
                    _accumulators[channel] -= interval;
                    _tickCounts[channel]++;
                    FireChannel(channel, dt);
                }
            }

            OnTicked?.Invoke(dt);
        }

        private void FireChannel(string channel, float dt)
        {
            if (!_receiversByChannel.TryGetValue(channel, out var receivers))
                return;

            foreach (var receiver in receivers)
            {
                if (_firedThisTick.Add(receiver))
                    receiver.OnTick(dt);
            }
        }

        private void EnsureChannel(string channel, float defaultInterval = 2f, float defaultSpeed = 1f)
        {
            if (_intervals.ContainsKey(channel)) return;

            _channelKeys.Add(channel);
            _intervals[channel] = defaultInterval;
            _speeds[channel] = defaultSpeed;
            _accumulators[channel] = 0f;
            _tickCounts[channel] = 0;
        }

        public void SetChannelInterval(string channel, float seconds)
        {
            EnsureChannel(channel, seconds);
            _intervals[channel] = Mathf.Max(0f, seconds);
        }

        public void SetChannelSpeed(string channel, float speed)
        {
            EnsureChannel(channel, 2f);
            _speeds[channel] = Mathf.Max(0f, speed);
        }

        public float GetChannelInterval(string channel) => _intervals.GetValueOrDefault(channel, 2f);
        public float GetChannelSpeed(string channel) => _speeds.GetValueOrDefault(channel, 1f);
        public int GetTickCount(string channel) => _tickCounts.GetValueOrDefault(channel, 0);

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

        public void Register(ITickReceiver receiver)
        {
            Register(receiver, ChannelEveryTick);
        }

        public void Register(ITickReceiver receiver, string channel)
        {
            EnsureChannel(channel);
            if (!_pendingAddByChannel.TryGetValue(channel, out var list))
            {
                list = new List<ITickReceiver>();
                _pendingAddByChannel[channel] = list;
            }
            list.Add(receiver);
        }

        public void Unregister(ITickReceiver receiver)
        {
            _pendingRemoveAll.Add(receiver);
        }

        public void Unregister(ITickReceiver receiver, string channel)
        {
            if (!_pendingRemoveByChannel.TryGetValue(channel, out var list))
            {
                list = new List<ITickReceiver>();
                _pendingRemoveByChannel[channel] = list;
            }
            list.Add(receiver);
        }

        private void ProcessPendingLists()
        {
            foreach (var channel in _pendingAddByChannel.Keys)
            {
                var pending = _pendingAddByChannel[channel];
                if (pending.Count == 0) continue;

                if (!_receiversByChannel.TryGetValue(channel, out var active))
                {
                    active = new List<ITickReceiver>();
                    _receiversByChannel[channel] = active;
                }

                foreach (var receiver in pending)
                {
                    if (!active.Contains(receiver))
                        active.Add(receiver);
                }
                pending.Clear();
            }

            foreach (var channel in _pendingRemoveByChannel.Keys)
            {
                var pending = _pendingRemoveByChannel[channel];
                if (pending.Count == 0) continue;

                if (_receiversByChannel.TryGetValue(channel, out var active))
                {
                    foreach (var receiver in pending)
                        active.Remove(receiver);
                }
                pending.Clear();
            }

            foreach (var receiver in _pendingRemoveAll)
            {
                foreach (var list in _receiversByChannel.Values)
                    list.Remove(receiver);
            }
            _pendingRemoveAll.Clear();
        }
    }
}