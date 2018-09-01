namespace Framework
{
    namespace Code.Widget
    {
        using UnityEngine;

        public class EmptyNode : MonoBehaviour, IWidget
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
        }
    }
}