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

      SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id = @BandId;", conn);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM bands;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
