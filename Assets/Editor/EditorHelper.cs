using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Framework.Code.Widget;


namespace Framework.Editor
{
    using AssetBundle;
    namespace Common
    {
        public class EditorHelper
        {
            [MenuItem("FrameworkTools/AssetBundleWnd", false, -1)]
            public static void ShowAssetBundleWnd()
            {
                AssetBundleWnd wnd = EditorWindow.GetWindowWithRect<AssetBundleWnd>(new Rect(600, 800, 600, 435), true);
                wnd.Show();
            }
        }
    }
}