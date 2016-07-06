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
            if (Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName) == null)
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
            Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName, true).SetValue(Statics.KievskiyDistrict, textValue, RegistryValueKind.String);
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
            Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName, true).SetValue(Statics.PrimorskiyDistrict, textValue, RegistryValueKind.String);
            points.Clear();
        }

        private bool OdessaSchemeCheck()
        {
            if (Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.KievskiyDistrict) == null)
            {
                return false;
            }
            else if (Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Statics.ProgramsRegistryKeyName).GetValue(Statics.PrimorskiyDistrict) == null)
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            polygonOdessaFormer();
            Application.Restart();
        }
    }
}
