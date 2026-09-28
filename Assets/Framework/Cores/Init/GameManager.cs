using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Dada.Foundations;
using UnityEngine;

namespace Dada.Cores
{
    public enum GameState
    {
        Init,
        Loading,
        Playing,
        Paused,
    }

    public class GameManager : MonoSingleton<GameManager>
    {
        private readonly Dictionary<Type, object> _services = new();
        private GameState _currentState = GameState.Init;

        public GameState CurrentState => _currentState;

        public event Action<GameState, GameState> OnStateChanged;

        public void SetState(GameState newState)
        {
            if (_currentState == newState) return;

            var previous = _currentState;
            _currentState = newState;
            OnStateChanged?.Invoke(previous, newState);
        }

        public void RegisterService<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public T GetService<T>()
        {
            return _services.TryGetValue(typeof(T), out var service)
                ? (T)service
                : default;
        }

        public async UniTaskVoid StartGame(string firstScene)
        {
            SetState(GameState.Loading);
            await SceneLoader.LoadSceneAsync(firstScene);
            if (CurrentState == GameState.Loading)
                SetState(GameState.Playing);
        }
    }
}
