namespace Framework
{
    namespace Code.Widget
    {
        public class ScrollbarEx : UnityEngine.UI.Scrollbar, IWidget
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