using ICities;
using ColossalFramework;
using UnityEngine;
using System;
using ColossalFramework.Plugins;

namespace AutomaticEmpty
{
   public class AutoEmptyMod : IUserMod
   {
      public string Name
      {
         get { return "Automatic Empty"; }
      }

      public string Description
      {
         get
         {
            return
               "This mod automatically empties the landfill and cemetery buildings " +
               "when they are full and restarts filling once completely empty.";
         }
      }
   }

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
         try
         {
            _landfillBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.Garbage);

            foreach (ushort buildingID in _landfillBuildings)
            {
               BuildingAI landfillAI = _buildingManager.m_buildings.m_buffer[buildingID].Info.m_buildingAI;

               if (landfillAI is LandfillSiteAI && landfillAI.CanBeEmptied())
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
         }
         catch (Exception e)
         {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, "Check Landfill Error.");
         }

         try
         {
            _cemeteryBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.HealthCare);

            foreach (ushort buildingID in _cemeteryBuildings)
            {
               BuildingAI cemeteryAI = _buildingManager.m_buildings.m_buffer[buildingID].Info.m_buildingAI;

               if (cemeteryAI is CemeteryAI && cemeteryAI.CanBeEmptied())
               {
                  CemeteryAI cemeterySiteAI = cemeteryAI as CemeteryAI;

                  if (cemeterySiteAI.IsFull(buildingID, ref _buildingManager.m_buildings.m_buffer[buildingID]))
                  {
                     cemeteryAI.SetEmptying(buildingID, ref _buildingManager.m_buildings.m_buffer[buildingID], true);
                  }

                  if (cemeterySiteAI.CanBeRelocated(buildingID,
                     ref _buildingManager.m_buildings.m_buffer[buildingID]))
                  {
                     cemeteryAI.SetEmptying(buildingID, ref _buildingManager.m_buildings.m_buffer[buildingID], false);
                  }
               }
            }
         }
         catch (Exception e)
         {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, "Check Cemetery Error.");
         }
      }
   }
}