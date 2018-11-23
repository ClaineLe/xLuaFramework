using System.Collections.Generic;
using UnityEngine;
using Framework.Game;
using System.Linq;

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
			//private RectTransform _rectTransform;

            public List<Presender> _subPresenders{ get; private set;}
            public List<Widget.IWidget> _widgets{ get; private set;}
            protected override void onCreate()
            {
                this._subPresenders = new List<Presender>();
                this._widgets = new List<Widget.IWidget>();
            }
            protected override string _luaPath
            {
                get
                {
                    return string.Format(PathConst.FORMAT_VIEW_NAME, m_Name, m_Name);
                }
            }
            public View SetupViewGo(GameObject viewGo){
                this._gameObject = viewGo;
                this._transform = this._gameObject.transform;
                //this._rectTransform = this._transform as RectTransform;
                this.initSubPresender();
                this.initWidgets();
                base.InitLuaTable(this);
                return this;
            }

            public View SetupViewGo(Widget.EmptyNode subView){
                this.m_RefName = subView.RefName;
                return this.SetupViewGo(subView.gameObject);
            }

            private void initSubPresender() {
                Widget.EmptyNode[] subViews = _transform.GetComponentsInChildren<Widget.EmptyNode>();
                for (int i = 0; i < subViews.Length; i++){
                    Widget.EmptyNode subView = subViews[i];
                    if (string.IsNullOrEmpty(subView.RefName.Trim()))
                        continue;
                    if (subView.gameObject.Equals(this._gameObject))
                        continue;
                    View view = View.Create(subView.ViewName).SetupViewGo(subView);
                    this._subPresenders.Add(Presender.Create(view.m_Name).SetupView(view));
                }
            }

            private void initWidgets() {
                List<Widget.IWidget> widgets = new List<Widget.IWidget>( _transform.GetComponentsInChildren<Widget.IWidget>());
                for (int i = 0; i < _subPresenders.Count; i++) {
                    widgets = this._subPresenders[i].m_View.clearRepeatWidget(widgets);
                }
                this._widgets = widgets;
            }

            public List<Widget.IWidget> clearRepeatWidget(List<Widget.IWidget> scrWidgetList) {
                return scrWidgetList.Except(this._widgets).ToList();
            }


            public void SetParent(Transform parent){
				this._transform.SetParent(parent);
			}

            protected override void onRelease()
            {
            }
        }
    }
}