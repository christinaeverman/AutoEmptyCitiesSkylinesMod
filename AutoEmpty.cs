using System.Collections.Generic;
using ICities;
using ColossalFramework;

namespace AutomaticEmpty
{
   public class AutoEmpty
   {
      public class Watch : ThreadingExtensionBase
      {
         private BuildingManager _buildingManager;
         FastList<ushort> landfillBuildings;
         FastList<ushort> cemeteryBuildings;
         
      }
   }
}