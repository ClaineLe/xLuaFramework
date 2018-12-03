using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Assistant;

namespace Framework
{
    namespace Core.Widget
    {
        public interface IWidget
        {
			View ParentView
			{
				get;
				set;
			}
			
            string RefName
            {
                get;
                set;
            }
        }
    }
}