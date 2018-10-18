using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Olympiads
{
    class Profile
    {
        public string Name { get; } = "";
        public List<string> Subjects { get; } = new List<string>();
        public int Level { get; } = 0;
        public double Weight { get; private set; } = 0.0;

        public Profile() { }

        [JsonConstructor]
        public Profile(string name, List<string> subjects, int level)
        {
            Name = name;
            Subjects = subjects;
            Level = level;
            Weight = CalcWeight();
        }

        public Profile(Tuple<string, string, int> data)
        {
            Name = data.Item1;
            Subjects = ParseSubjects(data.Item2);
            Level = data.Item3;
            Weight = CalcWeight();
        }

        public void AddSubject(string subject)
        {
            Subjects.Add(subject);
            Weight = CalcWeight();
        }

        public void AddSubjects(List<string> subjects)
        {
            Subjects.AddRange(subjects);
            Weight = CalcWeight();
        }

        private double CalcWeight()
        {
            double weight = 0;

            string lcName = Name.ToLower();
            if (lcName.Contains("информ")) weight += 10;
            if (lcName.Contains("матем")) weight += 10;
            if (lcName.Contains("физика")) weight += 10;

            foreach (var s in Subjects)
            {
                string lcSubject = s.ToLower();
                if (lcSubject.Contains("информ")) weight++;
                if (lcSubject.Contains("матем")) weight++;
                if (lcSubject.Contains("физика")) weight++;
            }

            weight /= Level;

            return weight;
        }

        private List<string> ParseSubjects(string data)
        {
            var subjects = data.Split(',').ToList();
            for (int i = 0; i < subjects.Count; ++i)
            {
                subjects[i] = subjects[i].Trim();
            }

            return subjects;
        }
    }
}
