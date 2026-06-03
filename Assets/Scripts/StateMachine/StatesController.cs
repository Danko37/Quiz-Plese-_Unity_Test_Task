using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace StateMachine
{
    /// <summary>
    /// Класс управляет состояниями приложения
    /// </summary>
    public class StatesController<TEnum> : IStatesController<TEnum>
    {
        private readonly IReadOnlyDictionary<TEnum, IState> _states;
        private IState _currentState;

        public StatesController(IReadOnlyDictionary<TEnum, IState> states)
        {
            _states = states ?? throw new ArgumentNullException(nameof(states));
        }

        public async UniTask EnterStateAsync(TEnum code, CancellationToken ct)
        {
            if (!_states.TryGetValue(code, out var newState))
                throw new InvalidOperationException($"State '{code}' is not registered.");

            //сначала завершаем текущее состояние
            if (_currentState != null)
                await _currentState.ExitAsync(ct);

            _currentState = newState;
            //входим в новое состояние
            await _currentState.EnterAsync(ct);
        }
    }
}
