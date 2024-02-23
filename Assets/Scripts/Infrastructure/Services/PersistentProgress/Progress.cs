using System;

namespace Infrastructure.Services.PersistentProgress
{
    [Serializable]
    public class Progress
    {
        public InventoryData InventoryData { get; private set; }
        public string SceneName { get; private set; }

        public Progress(string sceneName)
        {
            InventoryData = new InventoryData();
            SceneName = sceneName;
        }
    }
}