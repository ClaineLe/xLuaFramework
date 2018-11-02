using System.Collections.Generic;
using UnityEngine;
using Framework.Game;

namespace Framework.Core
{
    namespace Assistant
    {
        public class View : LuaCompatible<View>, Widget.IWidget, ILuaCompatible
        {
			
            public string m_RefName;
            public string RefName
            {
                get { return m_RefName; }
                set {
                    if (m_RefName != value)
                        m_RefName = value;
                }
            }
            
			private GameObject _gameObject;
			private Transform _transform;
			private RectTransform _rectTransform;

            public Dictionary<string,Presender> _subViews{ get; private set;}
			public Dictionary<string, Widget.IWidget> _widgets{ get; private set;}

            protected override void onCreate()
            {
                this._subViews = new Dictionary<string, Presender>();
                this._widgets = new Dictionary<string, Widget.IWidget>();
            }
            protected override string _luaPath
            {
                get
                {
                    return string.Format(ResPathConst.FORMAT_VIEW_NAME, Name, Name);
                }
            }
            public void SetupViewGo(GameObject viewGo){
                this._gameObject = viewGo;
                this._transform = viewGo.transform;
                this._rectTransform = this._transform as RectTransform;
                this.InitWidgets();
                base.InitLuaTable(this);
            }

            public void InitWidgets() {
                Widget.IWidget[] widgets = _transform.GetComponentsInChildren<Widget.IWidget>();
                for (int i = 0; i < widgets.Length; i++) {
                    Widget.IWidget widget = widgets[i];
					if (string.IsNullOrEmpty(widget.RefName.Trim()))
                        continue;
					if (widgets [i] is View) {
						View view = widgets [i] as View;
                        this._subViews[widget.RefName] = Presender.Create(view.Name);
                        this._subViews[widget.RefName].SetupView(view);
                    } else {
						this._widgets [widget.RefName] = widget;
                    }
                }
            }

			public void SetParent(Transform parent){
				this._transform.SetParent(parent);
			}
        }
    }
}