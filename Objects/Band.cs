using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTracker.Objects
{
  public class Band
  {
    private int _id;
    private string _name;

    public Band(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public int GetId()
   {
       return _id;
   }
   public string GetName()
   {
       return _name;
   }
   public override bool Equals(System.Object otherBand)
    {
      if(!(otherBand is Band)) return false;
      else
      {
        Band newBand = (Band) otherBand;
        bool idEquality = this.GetId() == newBand.GetId();
        bool nameEquality = this.GetName() == newBand.GetName();
        return(nameEquality && idEquality);
      }
    }
    public static List<Band> GetAll()
    {
      List<Band> allBands = new List<Band>{};
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Band newBand = new Band(name, bandId);
        allBands.Add(newBand);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allBands;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@bandName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@bandName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }
    public static Band Find(int bandId)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @bandId;", conn);

      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@bandId";
      bandIdParameter.Value = bandId.ToString();

      cmd.Parameters.Add(bandIdParameter);

      int foundBandId = 0;
      string foundBandName = null;
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        foundBandId = rdr.GetInt32(0);
        foundBandName = rdr.GetString(1);
      }
      Band newBand = new Band(foundBandName, foundBandId);
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return newBand;
    }
    public void Update(string newName)
    {
      _name = newName;

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE bands SET name=@NewName WHERE id=@BandId;", conn);

      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bandIdParameter);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@NewName";
      nameParameter.Value = newName;
      cmd.Parameters.Add(nameParameter);

      cmd.ExecuteNonQuery();

      if(conn!=null) conn.Close();
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id = @BandId; DELETE FROM shows WHERE band_id = @BandId;", conn);

      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = this.GetId();

      cmd.Parameters.Add(bandIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null) conn.Close();
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM bands; DELETE FROM shows;", conn);
      cmd.ExecuteNonQuery();
    }
    public void AddShow(int venueId, DateTime date)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO shows (venue_id, band_id, date) VALUES (@VenueId, @BandId, @Date);", conn);

      SqlParameter venueIdParameter = new SqlParameter();
      venueIdParameter.ParameterName = "@VenueId";
      venueIdParameter.Value = venueId;
      cmd.Parameters.Add(venueIdParameter);

      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bandIdParameter);

      SqlParameter dateParameter = new SqlParameter();
      dateParameter.ParameterName = "@Date";
      dateParameter.Value = date;
      cmd.Parameters.Add(dateParameter);

      cmd.ExecuteNonQuery();

      if(conn!=null) conn.Close();
    }
    public void DeleteShow(int venueId, DateTime date)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM shows WHERE venue_id=@VenueId AND band_id=@BandId AND date=@Date;", conn);

      SqlParameter venueIdParameter = new SqlParameter();
      venueIdParameter.ParameterName = "@VenueId";
      venueIdParameter.Value = venueId;
      cmd.Parameters.Add(venueIdParameter);

      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bandIdParameter);

      SqlParameter dateParameter = new SqlParameter();
      dateParameter.ParameterName = "@Date";
      dateParameter.Value = date;
      cmd.Parameters.Add(dateParameter);


      cmd.ExecuteNonQuery();

      if(conn!=null) conn.Close();
    }
    public List<Venue> GetVenues()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands JOIN shows ON (shows.band_id = bands.id) JOIN venues ON (shows.venue_id = venues.id) WHERE band_id = @BandId ORDER BY shows.date DESC;", conn);

      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bandIdParameter);

      rdr=cmd.ExecuteReader();

      List<Venue> foundVenues = new List<Venue>{};

      while(rdr.Read())
      {
        int foundId = rdr.GetInt32(0);
        string foundName = rdr.GetString(1);
        Venue foundVenue = new Venue(foundName, foundId);
        foundVenues.Add(foundVenue);
      }

      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();

      return foundVenues;
    }
    public List<DateTime> GetShowDates()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT date FROM shows WHERE band_id = @BandId ORDER BY date DESC;", conn);

      SqlParameter bandIdParameter = new SqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bandIdParameter);

      DateTime foundDate = new DateTime(2011, 11, 11);
      rdr = cmd.ExecuteReader();

      List<DateTime> showDates = new List<DateTime>{};
      while(rdr.Read())
      {
        foundDate = rdr.GetDateTime(0);
        showDates.Add(foundDate);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return showDates;
    }
    public List<Venue> GetEligibleVenues()
    {
      List<Venue> eligibleVenues = Venue.GetAll();
      return eligibleVenues;
    }
  }
}
