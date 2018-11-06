using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Widget;

namespace Framework
{
    namespace Game
    {
        public class UpdaterPresender
        {
            private UpdaterView m_View;
            private UpdaterPresender() { }
            public static UpdaterPresender Create()
            {
                UpdaterPresender presender = new UpdaterPresender();
                presender.m_View = UpdaterView.Create();
                return presender;
            }

            public void Release()
            {
                if (this.m_View != null)
                {
                    this.m_View.Release();
                    this.m_View = null;
                }
            }
        }
    }
}