namespace Framework
{
    namespace Core.Widget
    {
        public class DropdownEx : UnityEngine.UI.Dropdown, IWidget
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