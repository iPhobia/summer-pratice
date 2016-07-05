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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleTerrainMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.SetPositionByKeywords("Odessa, Ukraine");
            gmap.DragButton = MouseButtons.Left; //перетаскивание карты ЛКМ


            GMapOverlay polyOverlay = new GMapOverlay("polygons");
            IList<GMap.NET.PointLatLng> points = new List<GMap.NET.PointLatLng>();
            /*using (StreamReader sr = new StreamReader(odessaCordDB, System.Text.Encoding.Default))
            {
                int j = 0;
                while ((stringDatabase = sr.ReadLine()) != null)
                {
                    string[] tempStrings = new string[2];
                    tempStrings = stringDatabase.Split(',');
                    points.Add(new GMap.NET.PointLatLng(Convert.ToDouble(tempStrings[0]), Convert.ToDouble(tempStrings[1])));
                    j++;
                }
            }*/
            points.Add(new GMap.NET.PointLatLng(46.488322, 30.733475));
            points.Add(new GMap.NET.PointLatLng(46.483417, 30.748951));
            points.Add(new GMap.NET.PointLatLng(46.472660, 30.746891));
            points.Add(new GMap.NET.PointLatLng(46.473251, 30.731099));
            var serializer = new JavaScriptSerializer();
            string textValue = serializer.Serialize(points);
            var key = InitializeRegistryKey("pointsList");
            key.SetValue(key.Name, textValue);
            string value = key.GetValue(key.Name).ToString();
            List<GMap.NET.PointLatLng> list = serializer.Deserialize<List<GMap.NET.PointLatLng>>(value);
            MessageBox.Show(list[0].Lat.ToString() + ", " + list[0].Lng.ToString());
            //GMapPolygon polygon = new GMapPolygon(new List<GMap.NET.PointLatLng>(points), "mypolygon");
            //polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            //polygon.Stroke = new Pen(Color.Red, 1);
            //polyOverlay.Polygons.Add(polygon);
            //gmap.Overlays.Add(polyOverlay);
        }

        private RegistryKey InitializeRegistryKey(string key)
        {
            Microsoft.Win32.RegistryKey _key;
            _key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(key);
            return _key;
        }
    }
}
