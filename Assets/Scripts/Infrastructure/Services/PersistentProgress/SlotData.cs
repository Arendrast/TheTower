using System;

namespace Infrastructure.Services.PersistentProgress
{
    [Serializable]
    public class SlotData
    {
        public int Amount;
        public string ItemName;
    }
}