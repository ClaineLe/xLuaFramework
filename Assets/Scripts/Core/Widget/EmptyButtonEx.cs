using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace Framework
{
    namespace Core.Widget
    {
        public class EmptyButtonEx : MaskableGraphic, IPointerClickHandler, IWidget
        {
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