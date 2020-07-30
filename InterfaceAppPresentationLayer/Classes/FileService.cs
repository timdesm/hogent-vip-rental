using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InterfaceAppPresentationLayer.Classes
{
    class FileService
    {

        public static string GetFileAsString(String path)
        {
            string file;
            using(StreamReader r = new StreamReader(path))
            {
                file = r.ReadToEnd();
            }
            return file;
        }

    }
}
