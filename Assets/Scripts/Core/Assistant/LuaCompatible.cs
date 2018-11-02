using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    namespace Assistant
    {
        public interface ILuaCompatible {
        }
        public abstract class LuaCompatible<T> : ILuaCompatible where T : LuaCompatible<T>, ILuaCompatible, new()
        {
            protected abstract string _luaPath { get; }

            private string _name;
            protected string Name {
                get {
                    return this._name;
                }
            }

            private XLua.LuaTable _luaTable;
            public XLua.LuaTable m_LuaTable
            {
                get {
                    return _luaTable;
                }
            }

            public static T Create(string luaName)
            {
                T t = new T();
                t._name = luaName;
                t.onCreate();
                return t;
            }

            protected abstract void onCreate();

            protected void InitLuaTable(params object[] arg) {
                XLua.LuaTable luaTmp = Framework.Game.Manager.LuaMgr.TblRequire(_luaPath);
                this._luaTable = luaTmp.Get<XLua.LuaFunction>("Create").Call(luaTmp, arg[0])[0] as XLua.LuaTable;
            }

            public void Release() {
                onRelease();
            }

            protected virtual void onRelease() {

            }
        }
    }
}