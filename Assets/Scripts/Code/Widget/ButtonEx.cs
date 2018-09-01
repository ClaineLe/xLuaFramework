namespace Framework
{
    namespace Code.Widget
    {
        public class ButtonEx : UnityEngine.UI.Button, IWidget{
            public string m_RefName;
            public string RefName
            {
                get{
                    return m_RefName;
                }
                set{
                    if (m_RefName != value)
                        m_RefName = value;
                }
            }
            public ImageEx v_Image;
            public TextEx v_label;

        }
    }
}