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
			private View m_ParentView;
			public View ParentView {
				get {
					return m_ParentView;
				}
				set {

					if (m_ParentView != value)
						m_ParentView = value;
				}
			}

			public bool IsSubView{
				get{
					return m_ParentView != null;
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
				this.initSubViews();
                this.initWidgets();
                base.InitLuaTable(this);
                return this;
            }

            public View SetupViewGo(Widget.EmptyNode subView){
                this.m_RefName = subView.RefName;
                return this.SetupViewGo(subView.gameObject);
            }

			private void initSubViews() {
				Widget.SubView[] subViews = _transform.GetComponentsInChildren<Widget.SubView>();
                for (int i = 0; i < subViews.Length; i++){
					Widget.SubView subView = subViews[i];
                    if (string.IsNullOrEmpty(subView.RefName.Trim()))
                        continue;
                    if (subView.gameObject.Equals(this._gameObject))
                        continue;
					View view = View.Create(subView.ViewScript).SetupViewGo(subView);
					view.m_ParentView = this;
                    this._subPresenders.Add(Presender.Create(view.m_Name).SetupView(view));
                }
            }

            private void initWidgets() {
				_widgets = new List<Widget.IWidget>( _transform.GetComponentsInChildren<Widget.IWidget>());
				_widgets.RemoveAll (a => a.ParentView != null);
				_widgets.RemoveAll (a => a is Widget.SubView);
				for (int i = 0; i < _widgets.Count; i++) {
					_widgets [i].ParentView = this;
				}
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