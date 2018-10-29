﻿using System.Collections.Generic;
using UnityEngine;
using Framework.Game;


namespace Framework
{
    namespace Core.Widget
    {
        public class View : IWidget
        {

			private string m_Name;
			public string Name{
				get{ 
					return m_Name;
				}
			}

            public string m_RefName;
            public string RefName
            {
                get { return m_RefName; }
                set {
                    if (m_RefName != value)
                        m_RefName = value;
                }
            }

			protected virtual bool m_IsLuaView{
				get{
					return true;	
				}
			}

			private XLua.LuaTable m_LuaView;
			public XLua.LuaTable LuaView{
				get{ 
					return m_LuaView;
				}
			}

			private GameObject _gameObject;
			private Transform _transform;
			private RectTransform _rectTransform;


			public Dictionary<string,Presender> _subViews{ get; private set;}
			public Dictionary<string,IWidget> _widgets{ get; private set;}

			protected virtual void OnInit (){
			}
			protected virtual void OnRelease(){
			}
			private View(){}
			public static View Create(GameObject viewNode){
				View view = new View ();
				view.m_Name = viewNode.name;
				view._gameObject = viewNode;
				view._transform = viewNode.transform;
				view._rectTransform = view._transform as RectTransform;
				view._subViews = new Dictionary<string, Presender> ();
				view._widgets = new Dictionary<string, IWidget> ();
				view.Init ();
				return view;
			}

			private void Init(){
				this.InitWidgets ();
				if (this.m_IsLuaView) {
					this.InitLuaView ();
				}
				OnInit ();
			}

			public void Release(){
				OnRelease ();
			}

            public void InitWidgets() {
				IWidget[] widgets = _transform.GetComponentsInChildren<IWidget>();
                for (int i = 0; i < widgets.Length; i++) {
					IWidget widget = widgets[i];
					if (string.IsNullOrEmpty(widget.RefName.Trim()))
                        continue;
					if (widgets [i] is View) {
						View view = widgets [i] as View;
						this._subViews [widget.RefName] = Presender.Create (view._gameObject);
					} else {
						this._widgets [widget.RefName] = widget;
					}
                }
            }

			public void InitLuaView(){
				string luaPath = string.Format (ResPathConst.FORMAT_VIEW_NAME,this.Name,this.Name);
				XLua.LuaTable luaTmp = Framework.Game.Manager.LuaMgr.TblRequire (luaPath);
				this.m_LuaView = luaTmp.Get<XLua.LuaFunction> ("Create").Call (luaTmp, this)[0] as XLua.LuaTable;
			}

			public void SetParent(Transform parent){
				this._transform.SetParent(parent);
			}
        }
    }
}