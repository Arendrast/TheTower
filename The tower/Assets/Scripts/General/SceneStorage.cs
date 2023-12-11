using System.Collections.Generic;

namespace General
{
    public static class SceneStorage
    {
        public static readonly Dictionary<NamesOfScenes, int> IndexDictionary = new Dictionary<NamesOfScenes,int> {{NamesOfScenes.MainMenu, 0}}; 
        public enum NamesOfScenes
        {
            MainMenu,
        }
    }
}
