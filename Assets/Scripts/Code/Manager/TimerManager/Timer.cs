using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework
{
	namespace Code.Manager
	{
		public class Timer
		{
			private float _interval;
			private float _times;
			private bool _autoRemove; 
			private Action _onStart;
			private Action<float> _onTick;
			private Action<int> _onPreAction;
			private Action _onFinish;

			private float _curInterval;
			private int _curTimes;

			private float _progress;
			public float Progress{
				get{
					return _progress;
				}
			}

			public Timer( float interval,  int times,  Action<float> onTick,  Action onStart,  Action<int> onPreAction, Action onFinish, bool awakeStart, bool autoRemove){
				this._interval = interval;
				this._times = times;
				this._autoRemove = autoRemove;
				this._onStart = onStart;
				this._onTick = onTick;
				this._onPreAction = onPreAction;
				this._onFinish = onFinish;

				if (awakeStart){
					Start ();
				}
			}

			public bool IsAutoRemove{
				get{ 
					return _autoRemove;
				}
			}

			private bool m_isPause;
			public bool IsPause{
				get{ 
					return m_isPause;
				}
			}

			private bool m_isStart;
			public bool IsStart{
				get{ 
					return m_isStart;
				}
			}

			private bool _canTick{
				get{ 
					return !IsPause && IsStart;
				}
			}

			private void ReSet(){
				m_isPause = false;
				m_isStart = false;
				_curInterval = 0;
				_curTimes = 0;
			}

			public void Start ()
			{
				this.m_isStart = true;
				if(_onStart != null)
					_onStart ();
			}

			public void Pause ()
			{
				this.m_isPause = true;
			}

			public void Recovery ()
			{
				this.m_isPause = false;
			}

			public void Finish ()
			{
				ReSet ();
				if (_onFinish != null)
					_onFinish ();

				if (_autoRemove) {
					Framework.Game.Manager.TimerMgr.RemoveTimer (this);
				}
			}


			public void Tick ()
			{
				if (_canTick) {
					_curInterval += Time.deltaTime;
					_progress = Mathf.Clamp (_curInterval / _interval, 0, 1);
					if (_onTick != null)
						_onTick (_progress);
					
					if (_curInterval >= _interval) {
						if (_times == 0 || ++_curTimes <= _times) {
							_curInterval = 0;
							_onPreAction (_curTimes); 
						}
						if (_times != 0 && _times == _curTimes) {
							Finish ();
						}
					}
				}
			}
		}
	}
}