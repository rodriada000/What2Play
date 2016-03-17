using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameDecider.Models
{

    public class Genre
    {
        public string name { get; set; }
    }

    public class Theme
    {
        public string name { get; set; }
    }

    public class ReleaseDate
    {
        public string platform_name { get; set; }
        public string release_date { get; set; }
    }

    public class Company
    {
        public int id { get; set; }
        public bool developer { get; set; }
        public bool publisher { get; set; }
        public string name { get; set; }
    }

    public class Cover
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string id { get; set; }
    }

    public class Screenshot
    {
        public string url { get; set; }
        public string title { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string id { get; set; }
    }

    public class Video
    {
        public string title { get; set; }
        public string uid { get; set; }
    }

    public class IgdbGameDetails
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string release_date { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string summary { get; set; }
        public List<Genre> genres { get; set; }
        public List<Theme> themes { get; set; }
        public double rating { get; set; }
        public List<ReleaseDate> release_dates { get; set; }
        public List<Company> companies { get; set; }
        public Cover cover { get; set; }
        public List<Screenshot> screenshots { get; set; }
        public List<Video> videos { get; set; }
    }

    public class RootObject
    {
        public IgdbGameDetails game { get; set; }
    }

}