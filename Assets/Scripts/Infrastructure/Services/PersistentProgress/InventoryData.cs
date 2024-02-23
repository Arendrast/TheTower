using System;
using System.Collections.Generic;

namespace Infrastructure.Services.PersistentProgress
{
    [Serializable]
    public class InventoryData
    {
        public List<SlotData> Slots = new List<SlotData>();
    }
}