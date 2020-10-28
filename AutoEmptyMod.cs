using System.Collections;
using ICities;
using System;
using System.IO;
using ColossalFramework;
using UnityEngine;
using System.Collections.Generic;

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
}