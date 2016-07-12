using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.WindowsForms;
using System.IO;
using Microsoft.Win32;
using System.Web.Script.Serialization;
using CellsNVirus;

namespace InfectionSpreadSimulator
{
    public partial class Form1 : Form
    {
        GMapOverlay polyOverlay = new GMapOverlay("polygons");
        GMapPolygon polygon;
        List<Cell> districts = new List<Cell>();
        List<Edge> edgeBtwDistricts = new List<Edge>();
        Random rnd = new Random();
        int districtrand;
        Virus virusInfection = new Virus();

        //List<GMap.NET.PointLatLng> polygons = new List<GMap.NET.PointLatLng>;
        public Form1()
        {
            InitializeComponent();
        }
        Edge kievprim = new Edge("kievNPrimEdge", 70);
        Edge kievmalin = new Edge("kievNMalinEdge", 60);
        Edge malinprim = new Edge("malinNPrimEdge", 50);
        Edge malinsuvor = new Edge("malinNSuvorEdge", 40);
        Edge suvorprim = new Edge("suvorNPrimEdge", 30);
        Edge suvorposkot = new Edge("suvorNPosKotEdge", 20);
        private void Form1_Load(object sender, EventArgs e)
        {
            if (FirstStartOnThisComputer() == true)
            {
                //no program's entry has been detected in registry, so create a new one
                Registry.CurrentUser.CreateSubKey(Statics.ProgramsRegistryKeyName);
                polygonOdessaFormer();
            }
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleTerrainMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.SetPositionByKeywords("Odessa, Ukraine");
            gmap.DragButton = MouseButtons.Left; //перетаскивание карты ЛКМ

            if (!FirstStartOnThisComputer())
            {
                //if coordinates stored in registry are corrupted, then reupload them into registry again
                if (!OdessaSchemeCheck())
                {
                    polygonOdessaFormer();
                    MessageBox.Show("Program has detected, that polygons' coordinates were modified. Reset to default state succesfully completed.");
                }
            }

            districts.Add(new Cell("kievskiy",6));
            districts.Add(new Cell("primorskiy",7));
            districts.Add(new Cell("malinovskiy",8));
            districts.Add(new Cell("suvorovskiy",9));
            districts.Add(new Cell("kotovskiyPosyolok",10));

           
            edgeBtwDistricts.Add(kievprim);
            edgeBtwDistricts.Add(kievmalin);
            edgeBtwDistricts.Add(malinprim);
            edgeBtwDistricts.Add(malinsuvor);
            edgeBtwDistricts.Add(suvorprim);
            edgeBtwDistricts.Add(suvorposkot);

            //draw polygons
            inputDataInPolygonDrawer();
        }

        private void virus_Tick(object sender, EventArgs e)
        {
            int kievNPrimInfectChance = rnd.Next(0, 150);
            int kievNMalinInfectChance = rnd.Next(0, 150);
            int malinNPrimInfectChance = rnd.Next(0, 150);
            int malinNSuvorInfectChance = rnd.Next(0, 150);
            int suvorNPrimInfectChance = rnd.Next(0, 150);
            int suvorNPosKotInfectChance = rnd.Next(0, 150);

            //in case of kievskiy district infected
            if (districts[0].IsInfected)
            {
                //primorskiy district infect
                if (kievNPrimInfectChance < edgeBtwDistricts[0].InfectedChance)
                {
                    districts[1].IsInfected = true;
                }
                //malinovskiy district infect
                if (kievNMalinInfectChance < edgeBtwDistricts[1].InfectedChance)
                {
                    districts[2].IsInfected = true;
                }
            }

            //in case of primorskiy district infected
            if (districts[1].IsInfected)
            {
                //kievskiy district infect
                if (kievNPrimInfectChance < edgeBtwDistricts[0].InfectedChance)
                {
                    districts[0].IsInfected = true;
                }
                //malinovskiy district infect
                if (malinNPrimInfectChance < edgeBtwDistricts[2].InfectedChance)
                {
                    districts[2].IsInfected = true;
                }
                //suvorovskiy district infect
                if (suvorNPrimInfectChance < edgeBtwDistricts[4].InfectedChance)
                {
                    districts[3].IsInfected = true;
                }
            }

            //in case of malinovskiy district infected
            if (districts[2].IsInfected)
            {
                //kievskiy district infect
                if (kievNMalinInfectChance < edgeBtwDistricts[0].InfectedChance)
                {
                    districts[0].IsInfected = true;
                }
                //primorskiy district infect
                if (malinNPrimInfectChance < edgeBtwDistricts[2].InfectedChance)
                {
                    districts[1].IsInfected = true;
                }
                //suvorovskiy district infect
                if (malinNSuvorInfectChance < edgeBtwDistricts[3].InfectedChance)
                {
                    districts[3].IsInfected = true;
                }
            }

            //in case of suvorovskiy district infected
            if (districts[3].IsInfected)
            {
                //malinovskiy district infect
                if (malinNSuvorInfectChance < edgeBtwDistricts[3].InfectedChance)
                {
                    districts[2].IsInfected = true;
                }
                //primorskiy district infect
                if (suvorNPrimInfectChance < edgeBtwDistricts[4].InfectedChance)
                {
                    districts[1].IsInfected = true;
                }
                //posyolok Kotovskiy district infect
                if (suvorNPosKotInfectChance < edgeBtwDistricts[5].InfectedChance)
                {
                    districts[4].IsInfected = true;
                }
            }

            //in case of posyolok Kotovskiy district infected
            if (districts[4].IsInfected)
            {
                //primorskiy district infect
                if (suvorNPosKotInfectChance < edgeBtwDistricts[5].InfectedChance)
                {
                    districts[3].IsInfected = true;
                }
            }
        }

        private bool FirstStartOnThisComputer()
        {
            if (Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void polygonOdessaFormer()
        {
            IList<GMap.NET.PointLatLng> points = new List<GMap.NET.PointLatLng>();
            var serializer = new JavaScriptSerializer();
            string textValue;

            //kievskiyDistrict
            points.Add(new GMap.NET.PointLatLng(46.320427, 30.677651));
            points.Add(new GMap.NET.PointLatLng(46.351084, 30.665794));
            points.Add(new GMap.NET.PointLatLng(46.393555, 30.689039));
            points.Add(new GMap.NET.PointLatLng(46.429958, 30.695675));
            points.Add(new GMap.NET.PointLatLng(46.436918, 30.744867));
            points.Add(new GMap.NET.PointLatLng(46.412058, 30.760014));
            points.Add(new GMap.NET.PointLatLng(46.376738, 30.748329));
            textValue = serializer.Serialize(points);
            //create s string parameter in Win registy for Kievskiy District
            Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName, true).SetValue(Statics.KievskiyDistrict, textValue, RegistryValueKind.String);
            points.Clear();

            //primorskiyDistrict
            points.Add(new GMap.NET.PointLatLng(46.477375, 30.697186));
            points.Add(new GMap.NET.PointLatLng(46.473977, 30.722220));
            points.Add(new GMap.NET.PointLatLng(46.412058, 30.760014));
            points.Add(new GMap.NET.PointLatLng(46.448168, 30.773919));
            points.Add(new GMap.NET.PointLatLng(46.478912, 30.767224));
            points.Add(new GMap.NET.PointLatLng(46.505384, 30.729287));
            textValue = serializer.Serialize(points);
            //create s string parameter in Win registy for Primorskiy District
            Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName, true).SetValue(Statics.PrimorskiyDistrict, textValue, RegistryValueKind.String);
            points.Clear();

            points.Add(new GMap.NET.PointLatLng(46.436918, 30.744867));
            points.Add(new GMap.NET.PointLatLng(46.423040, 30.646808));
            points.Add(new GMap.NET.PointLatLng(46.501816, 30.613063));
            points.Add(new GMap.NET.PointLatLng(46.508243, 30.628287));
            points.Add(new GMap.NET.PointLatLng(46.477375, 30.697186));
            points.Add(new GMap.NET.PointLatLng(46.473977, 30.722220));
            textValue = serializer.Serialize(points);
            //create s string parameter in Win registy for Malinovskiy District
            Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName, true).SetValue(Statics.MalinovkiyDistrict, textValue, RegistryValueKind.String);
            points.Clear();

            points.Add(new GMap.NET.PointLatLng(46.492279, 30.664001));
            points.Add(new GMap.NET.PointLatLng(46.477375, 30.697186));
            points.Add(new GMap.NET.PointLatLng(46.505384, 30.729287));
            points.Add(new GMap.NET.PointLatLng(46.545008, 30.750693));
            points.Add(new GMap.NET.PointLatLng(46.557131, 30.780836));
            points.Add(new GMap.NET.PointLatLng(46.566255, 30.759605));
            points.Add(new GMap.NET.PointLatLng(46.559798, 30.702037));
            points.Add(new GMap.NET.PointLatLng(46.549333, 30.691316));
            points.Add(new GMap.NET.PointLatLng(46.542091, 30.664011));
            points.Add(new GMap.NET.PointLatLng(46.501918, 30.675035));
            textValue = serializer.Serialize(points);
            //create s string parameter in Win registy for Suvorovskiy District
            Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName, true).SetValue(Statics.SuvorovskiyDistrict, textValue, RegistryValueKind.String);
            points.Clear();

            points.Add(new GMap.NET.PointLatLng(46.557131, 30.780836));
            points.Add(new GMap.NET.PointLatLng(46.566255, 30.759605));
            points.Add(new GMap.NET.PointLatLng(46.578149, 30.759793));
            points.Add(new GMap.NET.PointLatLng(46.596786, 30.786406));
            points.Add(new GMap.NET.PointLatLng(46.593099, 30.807680));
            points.Add(new GMap.NET.PointLatLng(46.570210, 30.801500));
            textValue = serializer.Serialize(points);
            //create s string parameter in Win registy for Posyolok Kotovskiy
            Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName, true).SetValue(Statics.PosyolokKotovskiy, textValue, RegistryValueKind.String);
            points.Clear();
        }

        private bool OdessaSchemeCheck()
        {
            if (Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.KievskiyDistrict) == null)
            {
                return false;
            }
            else if (Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.PrimorskiyDistrict) == null)
            {
                return false;
            }
            else if (Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.MalinovkiyDistrict) == null)
            {
                return false;
            }
            else if (Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.SuvorovskiyDistrict) == null)
            {
                return false;
            }
            else if (Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.PosyolokKotovskiy) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void drawPolygons(string registryValue, string regionName, Cell cell)
        {
            //universal method for drawing polygons. Grabs the values from registry, deserialize them and draw on map
            var serializer = new JavaScriptSerializer();
            List<GMap.NET.PointLatLng> list = serializer.Deserialize<List<GMap.NET.PointLatLng>>(registryValue);
            polygon = new GMapPolygon(list, regionName);
            if (cell.Health == cell.MaxHealth)
                polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Green));
            else
            if (cell.Health < cell.MaxHealth && cell.Health > cell.MaxHealth - cell.MaxHealth * 0.2)
                polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.LightGreen));
            else
            if (cell.Health <= cell.MaxHealth - cell.MaxHealth * 0.2 && cell.Health > cell.MaxHealth - cell.MaxHealth * 0.4)
                polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Yellow));
            else
            if (cell.Health <= cell.MaxHealth - cell.MaxHealth * 0.4 && cell.Health > cell.MaxHealth - cell.MaxHealth * 0.6)
                polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Orange));
            else
            if (cell.Health <= cell.MaxHealth - cell.MaxHealth * 0.6 && cell.Health > cell.MaxHealth - cell.MaxHealth * 0.8)
                polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.OrangeRed));
            else
            if (cell.Health <= cell.MaxHealth - cell.MaxHealth * 0.8 && cell.Health >= 0)
                polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            polygon.Stroke = new Pen(Color.Green, 1);
            polyOverlay.Polygons.Add(polygon);
            gmap.Overlays.Add(polyOverlay);
        }

        private void inputDataInPolygonDrawer()
        {
            string value;
            removePolygons();
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.KievskiyDistrict).ToString();
            drawPolygons(value, Statics.KievskiyDistrict, districts[0]);
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.PrimorskiyDistrict).ToString();
            drawPolygons(value, Statics.PrimorskiyDistrict, districts[1]);
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.MalinovkiyDistrict).ToString();
            drawPolygons(value, Statics.MalinovkiyDistrict, districts[2]);
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.SuvorovskiyDistrict).ToString();
            drawPolygons(value, Statics.SuvorovskiyDistrict, districts[3]);
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.PosyolokKotovskiy).ToString();
            drawPolygons(value, Statics.PosyolokKotovskiy, districts[4]);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //polygonOdessaFormer();
            //Application.Restart();
            //gmap.PolygonsEnabled = false;

            inputDataInPolygonDrawer();
        }

        private void removePolygons()
        {
            polyOverlay.Polygons.Clear();
            gmap.Overlays.Clear();
        }

        private void update_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                districtrand = rnd.Next(1, 101);
                if (districts[i].IsInfected)
                {
                    if (districts[i].Health > 0)
                    {
                        districts[i].Health = districts[i].Health - districts[i].HealthDecrease;
                    }
                    if (districtrand <= districts[i].Health / 3)
                    {
                        districts[i].IsInfected = false;
                        //districts[i].Health = districts[i].Health + 1;
                    }
                }
                else
                {
                    if (districts[i].Health < 100)
                    {
                        districts[i].Health = districts[i].Health + districts[i].HealthDecrease;
                    }
                }
                textBox1.Text = Convert.ToString(districts[0].IsInfected) + " " + Convert.ToString(districts[0].Health);
                textBox2.Text = Convert.ToString(districts[1].IsInfected) + " " + Convert.ToString(districts[1].Health);
                textBox3.Text = Convert.ToString(districts[2].IsInfected) + " " + Convert.ToString(districts[2].Health);
                textBox4.Text = Convert.ToString(districts[3].IsInfected) + " " + Convert.ToString(districts[3].Health);
                textBox5.Text = Convert.ToString(districts[4].IsInfected) + " " + Convert.ToString(districts[4].Health);

                if (districts[i].Health > 100)
                    districts[i].Health = 100;
                if (districts[i].Health < 0)
                    districts[i].Health = 0;
            }
            inputDataInPolygonDrawer();

            if (!districts[0].IsInfected && !districts[1].IsInfected && !districts[2].IsInfected && !districts[3].IsInfected && !districts[4].IsInfected)
            {
                virusInfection.IsActive = false;
                virus.Enabled = false;
                button2.BackColor = Color.LightGreen;
                button2.Text = "Activate Virus";
            }
        }

        private void rndStart_Click(object sender, EventArgs e)
        {
            if (virusInfection.IsActive == false)
            {
                virusInfection.IsActive = true;
                int startRegionRnd = rnd.Next(0, 5);
                districts[startRegionRnd].IsInfected = true;
                //Enables the virus's timer
                virus.Enabled = true;
                button2.BackColor = Color.Red;
                button2.Text = "Deactivate Virus";
            }
            else
            {
                virus.Enabled = false;
                virusInfection.IsActive = false;
                button2.BackColor = Color.LightGreen;
                button2.Text = "Activate Virus";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {   if (comboBox1.Text.Equals("Kievskiy Primorskiy"))
            kievprim.InfectedChance = Convert.ToInt32(textBox11.Text);
            if (comboBox1.Text.Equals("Kievskiy Malinovskiy"))
                kievmalin.InfectedChance = Convert.ToInt32(textBox11.Text);
            if (comboBox1.Text.Equals("Malinovskiy Primorskiy"))
                malinprim.InfectedChance = Convert.ToInt32(textBox11.Text);
            if (comboBox1.Text.Equals("Malinovskiy Suvorovskiy"))
                malinsuvor.InfectedChance = Convert.ToInt32(textBox11.Text);
            if (comboBox1.Text.Equals("Suvorovskiy Primorskiy"))
                suvorprim.InfectedChance = Convert.ToInt32(textBox11.Text);
            if (comboBox1.Text.Equals("Suvorovskiy Poskot"))
                suvorposkot.InfectedChance = Convert.ToInt32(textBox11.Text);
        }
    }
}
