using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace Core.Widget
    {
        public interface IWidget
        {
            string RefName
            {
                get;
                set;
            }
        }
    }
}