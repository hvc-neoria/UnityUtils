using System;
using System.Collections.Generic;

namespace HvcNeoria.Unity.Utils
{
    /// <summary>
    /// ステートマシン
    /// </summary>
    /// <typeparam name="TState">状態。enumなど。</typeparam>
    /// <typeparam name="TTrigger">状態遷移するトリガー。enumなど。TStateと同じ型でも構わない。</typeparam>
    public class StateMachine<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        /// <summary>
        /// 現在の状態
        /// </summary>
        /// <value>TState</value>
        public TState Current { get; private set; }

        /// <summary>
        /// 状態コレクション
        /// </summary>
        /// <typeparam name="TState">TState</typeparam>
        /// <typeparam name="Container">コンテナー</typeparam>
        /// <returns></returns>
        Dictionary<TState, Container> States = new Dictionary<TState, Container>();

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="initialState">初期状態</param>
        public StateMachine(TState initialState = default(TState))
        {
            foreach (TState state in Enum.GetValues(typeof(TState)))
            {
                States.Add(state, new Container());
            }
            Current = initialState;
        }

        /// <summary>
        /// 遷移情報を追加する。
        /// </summary>
        /// <param name="from">遷移元</param>
        /// <param name="to">遷移先</param>
        /// <param name="trigger">遷移のトリガー</param>
        public void AddTransition(TState from, TState to, TTrigger trigger)
        {
            States[from].transitions.Add(trigger, to);
        }

        /// <summary>
        /// 状態遷移時に実行する処理を、状態別タイミング別に登録する。
        /// </summary>
        /// <param name="state">状態</param>
        /// <param name="timing">タイミング</param>
        /// <param name="action">実行する処理</param>
        public void Subscribe(TState state, Timing timing, Action action)
        {
            switch (timing)
            {
                case Timing.Enter:
                    States[state].onEnter += action;
                    break;
                case Timing.Update:
                    States[state].onUpdate += action;
                    break;
                case Timing.Exit:
                    States[state].onExit += action;
                    break;
            }
        }

        public void Unsubscribe(TState state, Timing timing, Action action)
        {
            switch (timing)
            {
                case Timing.Enter:
                    States[state].onEnter -= action;
                    break;
                case Timing.Update:
                    States[state].onUpdate -= action;
                    break;
                case Timing.Exit:
                    States[state].onExit -= action;
                    break;
            }
        }

        /// <summary>
        /// トリガーを実行する。
        /// それに伴い、状態が遷移する。
        /// </summary>
        /// <param name="trigger">トリガー</param>
        public void Execute(TTrigger trigger)
        {
            States[Current].Exit();
            if (!States[Current].transitions.ContainsKey(trigger))
            {
                throw new ArgumentException($"遷移情報がありません。Current:{Current}, Trigger:{trigger}");
            }
            Current = States[Current].transitions[trigger];
            States[Current].Enter();
        }

        /// <summary>
        /// Updateで呼び出すこと。OnUpdateに登録した処理が実行される。
        /// </summary>
        public void OnUpdate()
        {
            States[Current].Update();
        }

        /// <summary>
        /// コンテナー
        /// </summary>
        class Container
        {
            public Dictionary<TTrigger, TState> transitions = new Dictionary<TTrigger, TState>();
            public event Action onEnter = delegate { };
            public event Action onUpdate = delegate { };
            public event Action onExit = delegate { };

            public void Enter() => onEnter();
            public void Update() => onUpdate();
            public void Exit() => onExit();
        }
    }

    /// <summary>
    /// タイミング
    /// </summary>
    public enum Timing
    {
        Enter,
        Update,
        Exit,
    }
}
