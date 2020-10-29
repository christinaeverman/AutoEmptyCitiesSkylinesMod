using System;
using System.Collections.Generic;
using ICities;
using ColossalFramework;

namespace AutomaticEmpty
{
   public class AutoEmpty
   {
      public class AutoEmptyThreading : ThreadingExtensionBase
      {
         private BuildingManager _buildingManager;
         private FastList<ushort> _landfillBuildings;
         private FastList<ushort> _cemeteryBuildings;

         public override void OnCreated(IThreading threading)
         {
            _buildingManager = Singleton<BuildingManager>.instance;
         }

         public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
         {
            _landfillBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.Garbage);

            foreach (ushort buildingID in _landfillBuildings)
            {
               BuildingAI landfillAI = _buildingManager.m_buildings.m_buffer[buildingID].Info.m_buildingAI;

               if (landfillAI is LandfillSiteAI && landfillAI.CanBeEmptied() && landfillAI.IsFull(buildingID, ref _buildingManager.m_buildings.m_buffer[buildingID]))
               {
                  LandfillSiteAI landfillSiteAI = landfillAI as LandfillSiteAI;

                  if (landfillSiteAI.IsFull(buildingID, ref _buildingManager.m_buildings.m_buffer[buildingID]))
                  {
                     landfillAI.SetEmptying(buildingID, ref _buildingManager.m_buildings.m_buffer[buildingID], true);
                  }

                  if (landfillSiteAI.CanBeRelocated(buildingID, ref _buildingManager.m_buildings.m_buffer[buildingID]))
                  {
                     landfillSiteAI.SetEmptying(buildingID, ref _buildingManager.m_buildings.m_buffer[buildingID], false);
                  }
               }
            }

            _cemeteryBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.HealthCare);

            foreach (ushort building in _cemeteryBuildings)
            {
               BuildingAI cemeteryAI = _buildingManager.m_buildings.m_buffer[building].Info.m_buildingAI;

               if (cemeteryAI is CemeteryAI && cemeteryAI.CanBeEmptied())
               {
                  cemeteryAI.SetEmptying(building, ref _buildingManager.m_buildings.m_buffer[building], true);
               }
            }
         }
      }

   }
}