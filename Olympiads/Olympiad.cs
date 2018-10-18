using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Olympiads
{
    class Olympiad
    {
        public string Name { get; } = "";
        public List<Profile> Profiles { get; } = new List<Profile>();
        public string ProfileDisplay
        {
            get
            {
                string profiles = "";
                foreach (var p in Profiles)
                {
                    profiles += p.Name + "\n";
                }
                return profiles;
            }
        }
        public string Link { get; } = "";
        public int Year { get; } = 0;
        public double Level { get; private set; } = 0;
        public double Weight { get; private set; } = 0;
        public bool Empty { get; } = true;
        public bool NotificationOn { get; private set; } = false;
        public DateTime NotificationDate { get; private set; } = new DateTime();

        public Olympiad() { }

        [JsonConstructor]
        public Olympiad(string name, string link, int year)
        {
            Name = name;
            Link = link;
            Year = year;
            Level = 0;
            Weight = 0;
            Empty = true;
        }

        public Olympiad(string name, string link, int year, List<Profile> profiles) : this(name, link, year)
        {
            Profiles = profiles;
            Level = CalcLevel();
            Weight = CalcWeight();
        }

        public void AddProfile(Profile profile)
        {
            Profiles.Add(profile);
            Level = CalcLevel();
            Weight = CalcWeight();
        }

        public void AddProfiles(List<Profile> profiles)
        {
            Profiles.AddRange(profiles);
            CalcLevel();
        }

        private double CalcLevel()
        {
            double level = 0;
            foreach(var p in Profiles)
            {
                level += p.Level;
            }

            return level / Profiles.Count;
        }

        private double CalcWeight()
        {
            double weight = 0;

            foreach (var p in Profiles)
            {
                weight += p.Weight;
            }

            var usefulProfiles = Profiles.Where(x => x.Weight > 0).Count();
            var usefulProfilesPercent = (double)usefulProfiles / Profiles.Count();

            weight *= usefulProfilesPercent;

            return weight;
        }

        private void SetNotififcation(DateTime date)
        {
            NotificationOn = true;
            NotificationDate = date;
        }
    }
}
