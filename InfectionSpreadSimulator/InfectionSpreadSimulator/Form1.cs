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

namespace InfectionSpreadSimulator
{
    public partial class Form1 : Form
    {
        static string path = @"F:\Visual Studio 2013 Projects\Course 2 Practics (InfectionSpreadSimulator)";
        string odessaCordDB = path + @"\txt\OdessaCord.txt";
        string stringDatabase;

        GMapOverlay polyOverlay = new GMapOverlay("polygons");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (FirstStartOnThisComputer() == true)
            {
                //no program's entry has been detected in registry, so create a new one
                Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Statics.ProgramsRegistryKeyName);
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

            //draw polygons
            inputDataInPolygonDrawer();

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

        private void drawPolygons(string registryValue, string regionName)
        {
            //universal method for drawing polygons. Grabs the values from registry, deserialize them and draw on map
            var serializer = new JavaScriptSerializer();
            List<GMap.NET.PointLatLng> list = serializer.Deserialize<List<GMap.NET.PointLatLng>>(registryValue);
            GMapPolygon polygon = new GMapPolygon(list, regionName);
            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.LightGreen));
            polygon.Stroke = new Pen(Color.Green, 1);
            polyOverlay.Polygons.Add(polygon);
            gmap.Overlays.Add(polyOverlay);
        }

        private void inputDataInPolygonDrawer()
        {
            string value; 
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.KievskiyDistrict).ToString();
            drawPolygons(value, Statics.KievskiyDistrict);
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.PrimorskiyDistrict).ToString();
            drawPolygons(value, Statics.PrimorskiyDistrict);
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.MalinovkiyDistrict).ToString();
            drawPolygons(value, Statics.MalinovkiyDistrict);
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.SuvorovskiyDistrict).ToString();
            drawPolygons(value, Statics.SuvorovskiyDistrict);
            value = Statics.ProgramKey.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.PosyolokKotovskiy).ToString();
            drawPolygons(value, Statics.SuvorovskiyDistrict);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            polygonOdessaFormer();
            Application.Restart();
        }
    }
}
