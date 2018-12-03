using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Framework.Core.Assistant;


namespace Framework
{
    namespace Core.Widget
    {
        public class EmptyButtonEx : MaskableGraphic, IPointerClickHandler, IWidget
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
            public UnityEvent onClick { get; set; }

            protected EmptyButtonEx()
            {
                onClick = new UnityEvent();
                useLegacyMeshGeneration = false;
            }

            protected override void OnPopulateMesh(VertexHelper toFill)
            {
                toFill.Clear();
            }

            public void OnPointerClick(PointerEventData eventData)
            {
                onClick.Invoke();
            }
          
        }
    }
}