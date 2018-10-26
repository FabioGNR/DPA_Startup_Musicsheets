using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor.Saving
{
    public static class AbstractSaver
    {
        private readonly static Dictionary<string, ICompositionSaver> _savers = new Dictionary<string, ICompositionSaver>();

        static AbstractSaver()
        {
            _savers.Add(".mid", new MidiSaver());
            _savers.Add(".ly", new LilypondSaver());
        }

        public static void SaveToFile(Composition composition, string filename)
        {
            string extension = Path.GetExtension(filename);
            if (_savers.ContainsKey(extension))
            {
                EnsureFileReady(filename);
                var saver = _savers[extension];
                saver.Save(composition, filename);
            }
            else
            {
                throw new NotSupportedException($"Can't save in {extension} extension");
            }
        }

        public static string GetFileTypeFilter()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var pair in _savers)
            {
                string extension = pair.Key;
                string description = pair.Value.Description;
                builder.Append(description);
                builder.Append("|*");
                builder.Append(extension);
                builder.Append('|');
            }
            builder.Length--; // this clears the last pipe
            return builder.ToString();
        }

        /// <summary>
        /// this method
        /// </summary>
        /// <param name="filename"></param>
        private static void EnsureFileReady(string filename)
        {
            string directoryName = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }
    }
}
