using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Checkers.Common
{
    public static class FileNameHelper
    {
        public static string GetExecutingDirectory()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            return new FileInfo(location).Directory.FullName + @"\";
        }
    }
}
