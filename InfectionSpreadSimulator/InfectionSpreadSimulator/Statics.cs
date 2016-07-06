using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace InfectionSpreadSimulator
{
    public class Statics
    {
        private static string programsRegistryKeyName = "InfSprSimPointsList";
        private static string kievskiyDistrict = "KievskiyDistrict";
        private static string primorskiyDistrict = "PrimorskiyDistrict";
        private static RegistryKey programKey = Microsoft.Win32.Registry.CurrentUser;
        public static string ProgramsRegistryKeyName
        {
            get { return programsRegistryKeyName; }
        }
        public static string KievskiyDistrict
        {
            get { return kievskiyDistrict; }
        }
        public static string PrimorskiyDistrict
        {
            get { return primorskiyDistrict; }
        }
        public static RegistryKey ProgramKey
        {
            get { return programKey; }
        }
    }
}
