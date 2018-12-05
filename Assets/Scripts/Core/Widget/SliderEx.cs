using Framework.Core.Assistant;

namespace Framework
{
    namespace Core.Widget
    {
        public class SliderEx : UnityEngine.UI.Slider, IWidget
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