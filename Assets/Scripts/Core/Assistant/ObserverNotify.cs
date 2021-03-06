﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
	namespace Assistant
	{
		public delegate void NotifyDelegate<P>(P param);
		public class ObserverNotify<K,P>
		{
			private Dictionary<K, NotifyDelegate<P>> m_DicObserver;

			public ObserverNotify(){
				this.m_DicObserver = new Dictionary<K, NotifyDelegate<P>> ();
			}

			public void Notify(K key, P param){
				if (this.m_DicObserver.ContainsKey (key)) {
					this.m_DicObserver [key].Invoke (param);
				} 
			}

			public bool ContainsObserver(K key)
			{
				return this.m_DicObserver.ContainsKey (key);
			}

			public void AddObserver(K key,NotifyDelegate<P> handle){
				if (this.m_DicObserver.ContainsKey (key)) {
					this.m_DicObserver [key] += handle;
				} else {
					this.m_DicObserver [key] = handle;
				}
			}

			public void RemoveNotify(K key, NotifyDelegate<P> handle = null){
				if (this.m_DicObserver.ContainsKey (key)) {
					if (handle == null) {
						this.m_DicObserver.Remove (key);
					} else {
						this.m_DicObserver [key] -= handle;
						if (this.m_DicObserver [key].GetInvocationList ().Length == 0)
							this.m_DicObserver.Remove (key);
					}
				}
			}

			public void ClearObserver(){
				if (this.m_DicObserver != null) {
					this.m_DicObserver.Clear ();
					this.m_DicObserver = null;
				}
			}
		}
	}
}