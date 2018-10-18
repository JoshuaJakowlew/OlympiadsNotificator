using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Windows.UI.Popups;

namespace Olympiads
{
    class Parser
    {
        private List<Olympiad> olympiads = new List<Olympiad>();
        private bool olympiadsCached = false;

        public List<Olympiad> Olympiads {
            get
            {
                if (olympiadsCached) return olympiads;

                LoadOlympiads();
                FilterOlympiads();
                olympiadsCached = true;

                return olympiads;
            }
            private set
            {
                olympiads = value;
            }
        }

        public Parser()
        {   
        }

        private void LoadOlympiads()
        {
            var web = new HtmlWeb();

            for (int year = 2013; year <= 2018; ++year)
            {

                string url = @"http://rsr-olymp.ru/archive/" + year;
                var doc = web.Load(url);

                ParseOlympiads(doc, year);

            }
        }

        private void ParseOlympiads(HtmlDocument doc, int year)
        {
            // Desine sperare qui hic intras

            var nodes = doc.DocumentNode.SelectNodes("//table[@class='mainTableInfo']/tbody/tr");

            // TODO: save all current olympiad's profiles to a list
            // and then use Olympiad.AddProfiles instead of Olympiad.AddProfile due to perfomance

            foreach (var node in nodes)
            {
                var cols = node.ChildNodes;
                int profileNameIndex = 1;
                int profileSubjectsIndex = 3;
                int profileLevelIndex = 5;

                // Current row is primary
                if (cols.Count == 11)
                {
                    var name = cols[3].InnerText.Trim();

                    string link = null;
                    try
                    {
                        link = cols[3].FirstChild.Attributes["href"].Value.Trim();
                    }
                    catch
                    {
                        link = "";
                    }

                    profileNameIndex = 5;
                    profileSubjectsIndex = 7;
                    profileLevelIndex = 9;

                    var olympiad = new Olympiad(name, link, year);
                    olympiads.Add(olympiad);
                }

                var profile = new Profile(
                    new Tuple<string, string, int> (
                        cols[profileNameIndex].InnerText,
                        cols[profileSubjectsIndex].InnerText,
                        int.Parse(cols[profileLevelIndex].InnerText)
                    ));
                olympiads.Last().AddProfile(profile);
            }
        }

        private void FilterOlympiads()
        {
            olympiads = olympiads
                .Where(x => x.Weight > 0)
                .OrderByDescending(x => x.Year)
                .GroupBy(x => new string(x.Link.ToLower().Where(p => char.IsLetter(p)).ToArray()))
                .Select(x => x.ElementAt(0))
                .OrderByDescending(x => x.Year)
                .GroupBy(x => new string (x.Name.ToLower().Where(p=>char.IsLetter(p)).ToArray()))
                .Select(x => x.ElementAt(0))
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Level)
                .ThenByDescending(x => x.Weight)
                .ToList();
        }  
    }
}
