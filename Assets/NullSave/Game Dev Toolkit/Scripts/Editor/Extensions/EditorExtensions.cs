using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    public class EditorExtensions
    {

        #region Constants

        private const string ROOT_DIR = "Game Dev Toolkit";

        #endregion

        #region Public Methods

        /// <summary>
        /// Get install directory for NullSave tools
        /// </summary>
        /// <returns></returns>
        public static string InstallDirectory()
        {
            string path = Path.Combine(Application.dataPath, ROOT_DIR);
            if (Directory.Exists(path)) return path;

            path = SearchForDirectory(ROOT_DIR, Application.dataPath);
            if (path == null) path = Application.dataPath;

            return path;
        }

        #endregion

        #region Private Methods

        private static string SearchForDirectory(string search, string startFrom)
        {
            string result = null;

            foreach (string dir in Directory.GetDirectories(startFrom))
            {
                if (dir.EndsWith(search))
                {
                    return dir;
                }
                else
                {
                    result = SearchForDirectory(search, dir);
                    if (result != null) return result;
                }
            }

            return result;
        }

        #endregion

    }
}