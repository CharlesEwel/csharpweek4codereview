using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTracker.Objects
{
  public class Venue
  {
    private int _id;
    private string _name;

    public Venue(string name, int id = 0)
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
   public override bool Equals(System.Object otherVenue)
    {
      if(!(otherVenue is Venue)) return false;
      else
      {
        Venue newVenue = (Venue) otherVenue;
        bool idEquality = this.GetId() == newVenue.GetId();
        bool nameEquality = this.GetName() == newVenue.GetName();
        return(nameEquality && idEquality);
      }
    }
    public static List<Venue> GetAll()
    {
      List<Venue> allVenues = new List<Venue>{};
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM venues;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int venueId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Venue newVenue = new Venue(name, venueId);
        allVenues.Add(newVenue);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allVenues;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO venues (name) OUTPUT INSERTED.id VALUES (@venueName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@venueName";
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
    public static Venue Find(int venueId)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM venues WHERE id = @venueId;", conn);

      SqlParameter venueIdParameter = new SqlParameter();
      venueIdParameter.ParameterName = "@venueId";
      venueIdParameter.Value = venueId.ToString();

      cmd.Parameters.Add(venueIdParameter);

      int foundVenueId = 0;
      string foundVenueName = null;
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        foundVenueId = rdr.GetInt32(0);
        foundVenueName = rdr.GetString(1);
      }
      Venue newVenue = new Venue(foundVenueName, foundVenueId);
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return newVenue;
    }
    public void Update(string newName)
    {
      _name = newName;

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE venues SET name=@NewName WHERE id=@VenueId;", conn);

      SqlParameter venueIdParameter = new SqlParameter();
      venueIdParameter.ParameterName = "@VenueId";
      venueIdParameter.Value = this.GetId();
      cmd.Parameters.Add(venueIdParameter);

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

      SqlCommand cmd = new SqlCommand("DELETE FROM venues WHERE id = @VenueId;", conn);

      SqlParameter venueIdParameter = new SqlParameter();
      venueIdParameter.ParameterName = "@VenueId";
      venueIdParameter.Value = this.GetId();

      cmd.Parameters.Add(venueIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null) conn.Close();
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM venues;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
