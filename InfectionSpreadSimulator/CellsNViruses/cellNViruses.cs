using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellsNViruses
{
    public class Cell
    {
        private int health = 50;
        private bool isInfected = false;
        public int Health 
        {
           get{return health;}
        }
        public bool IsInfected
        {
        get {return isInfected;}
        set {isInfected = value;}
        }


    }
}
