using System.Collections;
using System.Collections.Generic;
using System.IO;
using Framework.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework.Core
{
    namespace Assistant
    {
        public class DownloadHandleContinue : DownloadHandlerScript
        {
            private FileStream m_Stream;

            public string m_IsError { get; protected set; }
            private string _tmpExtension;
            private string _saveFullPath;

            private long _localSize;
            private long _totalSize;
            private string tmpFileFullPath{
                get{
                    return Path.ChangeExtension(_saveFullPath, _tmpExtension);
                }
            }

            public long GetLocalSize()
            {
                return _localSize;
            }

            public DownloadHandleContinue(string saveFullPath, int preCacheSize = 200, string tmpExtension = ".temp"):base(new byte[1024 * preCacheSize]){
                _saveFullPath = saveFullPath;
                _tmpExtension = tmpExtension;

                m_Stream = new FileStream(tmpFileFullPath, FileMode.Append, FileAccess.Write);    //文件流操作的是临时文件，结尾添加.temp扩展名
                _localSize = m_Stream.Length;
                m_Stream.Position = _localSize; 
            }

            protected override float GetProgress()
            {
                if (_localSize > 0 && _totalSize > 0)
                    return (float)(_localSize / (double)_totalSize);
                else
                    return 0;
            }
            protected override void CompleteContent()
            {
                ReleaseStream();
                if (File.Exists(tmpFileFullPath)){
                    if (File.Exists(_saveFullPath))
                        File.Delete(_saveFullPath);
                    File.Move(tmpFileFullPath, _saveFullPath);
                }else{
                    m_IsError = "Found out DownLoadFile. SaveFullPath:" + tmpFileFullPath;
                }
            }

            protected override void ReceiveContentLength(int contentLength)
            {
                this._totalSize = _localSize + contentLength;
            }

            protected override bool ReceiveData(byte[] data, int dataLength)
            {
                if (dataLength > 0)
                {
                    m_Stream.Write(data, 0, dataLength);
                    _localSize += dataLength;
                    return true;
                }
                return false;
            }

            public void ReleaseStream(){
                if(m_Stream != null)
                {
                    m_Stream.Close();
                    m_Stream.Dispose();
                    m_Stream = null;
                }
            }
        }
    }
}