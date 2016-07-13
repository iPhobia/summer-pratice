using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;
using Microsoft.Win32;

namespace InfSprClasses
{
    [Serializable()]
    public class Cell
    {
        private int maxHealth = 100;
        private int health;
        private string name;
        private bool isInfected = false;
        private int healthDecrease;

        public Cell(string name, int healthDecrease)
        {
            this.name = name;
            health = maxHealth;
            this.healthDecrease = healthDecrease;
        }

        public int HealthDecrease
        {
            get { return healthDecrease; }
            set { healthDecrease = value; }
        }
        public string Name
        {
            get { return name; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }
        public bool IsInfected
        {
            get { return isInfected; }
            set { isInfected = value; }
        }
    }

    [Serializable()]
    public class Edge
    {
        private int infectedChance;
        private string edgeName;

        public Edge(string edgeName, int infectedChance)
        {
            this.edgeName = edgeName;
            this.infectedChance = infectedChance;
        }

        public string EdgeName
        {
            get { return edgeName; }
        }
        public int InfectedChance
        {
            get { return infectedChance; }
            set { infectedChance = value; }
        }
    }

    [Serializable()]
    public class Virus
    {
        private bool isActive = false;

        public Virus()
        {

        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public void InfectRegion(Cell cellToInfect)
        {
            cellToInfect.IsInfected = true;
        }
    }

    public class SaveSerializer
    {
        private static string saveBinaryFilesPath;
        public static void Serialize(List<Cell> districts, List<Edge> edges, Virus virus)
        {
            string saveDate = Convert.ToString(DateTime.Now.Year) + "_" + Convert.ToString(DateTime.Now.Month) + "_" + Convert.ToString(DateTime.Now.Day) + "_" + Convert.ToString(DateTime.Now.Hour) + "_" + Convert.ToString(DateTime.Now.Minute) + "_" + Convert.ToString(DateTime.Now.Second);
            saveBinaryFilesPath = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName, true).GetValue(Statics.NameOfStringParameterInRegistryForFolderWithSaves).ToString();
            if (!Directory.Exists(saveBinaryFilesPath + @"\" + saveDate + "_InfSprSim_save"))
                Directory.CreateDirectory(saveBinaryFilesPath + @"\" + saveDate + "_InfSprSim_save");
            FileStream fileStream = File.Create(saveBinaryFilesPath + @"\" + saveDate + "_InfSprSim_save" + @"\" + "InfSprSim.cll");
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(fileStream, districts);
            fileStream.Close();

            fileStream = File.Create(saveBinaryFilesPath + @"\" + saveDate + "_InfSprSim_save" + @"\" + "InfSprSim.edg");
            serializer.Serialize(fileStream, edges);
            fileStream.Close();

            fileStream = File.Create(saveBinaryFilesPath + @"\" + saveDate + "_InfSprSim_save" + @"\" + "InfSprSim.vir");
            serializer.Serialize(fileStream, virus);
            fileStream.Close();
        }

    }
    public class Statics
    {
        private static string programsRegistryKeyName = "InfSprSimPointsList";
        private static string kievskiyDistrict = "KievskiyDistrict";
        private static string primorskiyDistrict = "PrimorskiyDistrict";
        private static string malinovkiyDistrict = "MalinovkiyDistrict";
        private static string suvorovskiyDistrict = "SuvorovskiyDistrict";
        private static string posyolokKotovskiy = "PosyolokKotovskiy";

        private static string nameOfStringParameterInRegistryForFolderWithSaves = "1_savesLocation";
        private static RegistryKey programKey = Registry.CurrentUser;
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
        public static string MalinovkiyDistrict
        {
            get { return malinovkiyDistrict; }
        }
        public static string SuvorovskiyDistrict
        {
            get { return suvorovskiyDistrict; }
        }
        public static string PosyolokKotovskiy
        {
            get { return posyolokKotovskiy; }
        }
        public static string NameOfStringParameterInRegistryForFolderWithSaves
        {
            get { return nameOfStringParameterInRegistryForFolderWithSaves; }
        }
        public static RegistryKey ProgramKey
        {
            get { return programKey; }
        }
    }
}

