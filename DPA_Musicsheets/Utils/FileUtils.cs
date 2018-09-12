using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Utils
{
    public static class FileUtils
    {

        public static FileType ParseFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            switch (extension)
            {
                case ".mid":
                    return FileType.Midi;
                case ".ly":
                    return FileType.LilyPond;
                default:
                    return FileType.Unknown;
            }
        }
    }
}
