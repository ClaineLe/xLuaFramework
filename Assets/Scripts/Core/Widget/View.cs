using System.Collections.Generic;
using UnityEngine;
using Framework.Game;
using Framework.Core.Manager;

namespace Framework.Core
{
    namespace Assistant
    {
        public class View : LuaCompatible<View>, ILuaCompatible
        {
			public bool IsSubView{
				get{
					return this.m_MonoView.ParentView != null;
				}
			}

			private MonoView m_MonoView;

			public string RefName{
				get{
					return m_MonoView.RefName;
				}
			}

			public List<Presender> _subPresenders{ get; private set;}
			public List<Widget.IWidget> _widgets{ get; private set;}

            protected override void onCreate()
            {
            }

            protected override string _luaPath
            {
                get
                {
                    return string.Format(PathConst.FORMAT_VIEW_NAME, m_Name, m_Name);
                }
            }

			public View SetupViewGo(MonoView viewGo){
				this.m_MonoView = viewGo.Init();
				this.initWidgets ();
                base.InitLuaTable(this);
                return this;
            }

            private void initWidgets(){
                _subPresenders = new List<Presender>();
                _widgets = new List<Widget.IWidget>();
                for (int i = 0; i < this.m_MonoView._widgets.Count;i++)
                {
                    if(this.m_MonoView._widgets[i] is MonoView)
                    {
                        MonoView monoView = this.m_MonoView._widgets[i] as MonoView;
                        Presender presender = ViewUtility.CreatePresender(monoView);
                        _subPresenders.Add(presender);
                    }

                    else{
                        _widgets.Add(m_MonoView._widgets[i] as Widget.IWidget);

                    }
                }
            }

            public void SetLayer(ViewLayer viewLayer){
				viewLayer.AddChild (this.m_MonoView);
			}

            protected override void onRelease()
            {
            }
        }
    }
}