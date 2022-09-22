using System.Text.Json.Serialization;

namespace Caracal.SpringBoot.Admin.Api.Model; 

public sealed class WeatherForecast {
  [JsonPropertyName("coord")] 
  public Coordinates Coordinates { get; set; } = null!;
  
  [JsonPropertyName("weather")]
  public List<WeatherData> DataPoints { get; set; } = new();
  
  [JsonPropertyName("base")]
  public string Base { get; set; }
  
  [JsonPropertyName("main")]
  public object Main { get; set; }
}

public sealed class WeatherData {
  [JsonPropertyName("id")]
  public long Id { get; set; }

  [JsonPropertyName("main")] 
  public string Title { get; set; } = null!;
  
  [JsonPropertyName("description")] 
  public string Description { get; set; } = null!;
  
  [JsonPropertyName("icon")] 
  public string Icon { get; set; } = null!;
}

public sealed class Coordinates {
  [JsonPropertyName("lat")]
  public float Latitude { get; set; }

  [JsonPropertyName("lon")]
  public float Longitude { get; set; }
}

/*
{"coord":{"lon":-40,"lat":30},"weather":[{"id":804,"main":"Clouds","description":"overcast clouds","icon":"04n"}],"base":"stations","main":{"temp":299.38,"feels_like":299.38,"temp_min":299.38,"temp_max":299.38,"pressure":1021,"humidity":70,"sea_level":1021,"grnd_level":1021},"visibility":10000,"wind":{"speed":5.48,"deg":103,"gust":5.92},"clouds":{"all":100},"dt":1663827426,"sys":{"sunrise":1663835296,"sunset":1663879038},"timezone":-10800,"id":0,"name":"","cod":200}

public class Clouds
    {
        public int all { get; set; }
    }

    public class Coord
    {
        public int lon { get; set; }
        public int lat { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }
    }

    public class Root
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    public class Sys
    {
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
        public double gust { get; set; }
    }



*/