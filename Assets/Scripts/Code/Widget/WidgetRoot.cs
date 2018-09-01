using System.Collections.Generic;
using UnityEngine;
namespace Framework
{
    namespace Code.Widget
    {
        public class WidgetRoot : MonoBehaviour, IWidget
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
            public IWidget[] GetWidgets() {
                IWidget[] widgetArr = transform.GetComponentsInChildren<IWidget>();
                List<IWidget> widgetList = new List<IWidget>();
                for (int i = 0; i < widgetArr.Length; i++) {
                    if (string.IsNullOrEmpty(widgetArr[i].RefName.Trim()))
                        continue;
                    widgetList.Add(widgetArr[i]);
                }
                return widgetList.ToArray();
            }
        }
    }
}