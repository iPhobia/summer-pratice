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

namespace InfectionSpreadSimulator
{
    public partial class Form1 : Form
    {
        static string path = @"C:\Users\NightKin\Source\Repos\summer-pratice\InfectionSpreadSimulator";
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


            GMapOverlay polyOverlay = new GMapOverlay("polygons");
            IList<GMap.NET.PointLatLng> points = new List<GMap.NET.PointLatLng>();
            using (StreamReader sr = new StreamReader(odessaCordDB, System.Text.Encoding.Default))
            {
                int j = 0;
                while ((stringDatabase = sr.ReadLine()) != null)
                {
                    string[] tempStrings = new string[2];
                    tempStrings = stringDatabase.Split(',');
                    points.Add(new GMap.NET.PointLatLng(Convert.ToDouble(tempStrings[0]), Convert.ToDouble(tempStrings[1])));
                    j++;
                }
            }
            GMapPolygon polygon = new GMapPolygon(new List<GMap.NET.PointLatLng>(points), "mypolygon");
            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            polygon.Stroke = new Pen(Color.Red, 1);
            polyOverlay.Polygons.Add(polygon);
            gmap.Overlays.Add(polyOverlay);
        }
    }
}
