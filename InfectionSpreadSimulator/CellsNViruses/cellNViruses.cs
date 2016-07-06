using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellsNVirus
{
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
        }
        public string Name
        {
            get { return name; }
        }
        public int Health 
        {
           get {return health;}
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
    
    public class Edge
    {
        private int infectedChance;
        private string edgeName;

        public Edge(string edgeName,int infectedChance)
        {
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
    }
}
