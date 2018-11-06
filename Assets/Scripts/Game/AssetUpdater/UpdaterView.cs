using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Widget;

namespace Framework
{
    namespace Game
    {
        public class UpdaterView
        {
            private const string prefab_view_name = "UpdaterView";

            private GameObject _gameObject;
            public IWidget[] _widgets;
            private Transform _transform;

            private UpdaterView() { }
            public static UpdaterView Create() {
                UpdaterView view = new UpdaterView();
                view._gameObject = GameObject.Instantiate(Resources.Load<GameObject>(prefab_view_name));
                view._transform = view._gameObject.transform;
                view._widgets = view._transform.GetComponentsInChildren<IWidget>();
                return view;
            }

            public void Release() {
                if (this._gameObject)
                    GameObject.Destroy(this._gameObject);
            }
        }
    }
}