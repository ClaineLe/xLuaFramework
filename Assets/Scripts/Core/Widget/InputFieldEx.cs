﻿using Framework.Core.Assistant;

namespace Framework
{
    namespace Core.Widget
    {
        public class InputFieldEx : UnityEngine.UI.InputField, IWidget
		{	private View m_ParentView;
			public View ParentView {
				get {
					return m_ParentView;
				}
				set {

					if (m_ParentView != value)
						m_ParentView = value;
				}
			}
            public string m_RefName;
            public string RefName
            {
                get
                {
                    return m_RefName;
                }
                set
                {
                    if (m_RefName != value)
                        m_RefName = value;
                }
            }
        }
    }
}