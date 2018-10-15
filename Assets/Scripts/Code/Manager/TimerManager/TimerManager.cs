using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework
{
	namespace Code.Manager
	{
		public partial class ManagerName
		{
			public const string Timer = "TimerManager";
		}
		public class TimerManager : BaseManager<TimerManager>, IManager
		{
			private List<Timer> _timerList;
			public int TimerCnt{
				get{ 
					if (_timerList == null)
						return 0;
					return _timerList.Count;
				}
			}
			public void Init ()
			{
				this._timerList = new List<Timer> ();
			}

			public void Release ()
			{
			}

			public void Tick ()
			{
				if (_timerList.Count > 0) {
					for (int i = 0; i < _timerList.Count;i++ ) {
						_timerList [i].Tick ();
					}
				}
			}

			public Timer AddTimer (float interval, int times, Action<float> onTick, Action onStart = null, Action<int> onPreAction = null, Action onFinish = null, bool awakeStart = true, bool autoRemove = true)
			{
				Timer timer = new Timer (interval,times,onTick,onStart,onPreAction,onFinish,awakeStart,autoRemove);
				_timerList.Add (timer);
				return timer;
			}

			public void RemoveTimer(Timer timer){
				_timerList.Remove (timer);
			}
		}
	}
}