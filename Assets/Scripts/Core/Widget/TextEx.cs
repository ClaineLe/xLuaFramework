﻿
namespace Framework
{
	namespace Core.Widget
    {
        public class TextEx : UnityEngine.UI.Text, IWidget
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