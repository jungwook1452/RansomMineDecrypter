/*
 * FileLog.cs
 * 
 * Program Log write
 * 
 * Copyright (c) jungwook(hjaas5397@naver.com). All rights reserved.
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace RansomMineDecrypter
{
    class FileLog
    {
        private string _LogFile_Name = "";

        public FileLog(String LogFileName)
        {
            _LogFile_Name = LogFileName;
        }

        public void Write(string Text)
        {
            A_Write(Text);
        }

        public void WriteLine(string Text)
        {
            A_WriteLine(Text);
        }

        private void A_Write(string str)
        {
            using (StreamWriter write = new StreamWriter(Path.Combine(Application.StartupPath, _LogFile_Name), true))
            {
                write.Write(str);
            }
        }

        private void A_WriteLine(string str)
        {
            using (StreamWriter write = new StreamWriter(Path.Combine(Application.StartupPath, _LogFile_Name), true))
            {
                write.WriteLine(str);
            }
        }
    }
}
