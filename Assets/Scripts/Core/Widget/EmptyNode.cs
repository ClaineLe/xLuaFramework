using Framework.Core.Assistant;

namespace Framework
{
    namespace Core.Widget
    {
        using UnityEngine;
        using UnityEngine.EventSystems;

        public class EmptyNode : UIBehaviour, IWidget
		{	private MonoView m_ParentView;
			public MonoView ParentView {
				get {
					return m_ParentView;
				}
				set {

					if (m_ParentView != value)
						m_ParentView = value;
				}
			}
			[HideInInspector]
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