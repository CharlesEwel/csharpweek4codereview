using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using BandTracker.Objects;


namespace BandTracker.Tests
{
  public class VenueTest : IDisposable
  {
    public VenueTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Venue.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Venue.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Save_SavesCorrectObjectToDatabase()
    {
      //Arrange
      Venue newVenue = new Venue("Chad's");

      //Act
      newVenue.Save();
      Venue savedVenue = Venue.GetAll()[0];

      //Assert
      Assert.Equal(newVenue, savedVenue);
    }
    [Fact]
    public void Test_Find_ReturnsASpecificVenueObject()
    {
      //Arrange
      Venue newVenue = new Venue("Chad's");
      newVenue.Save();

      //Act
      Venue foundVenue = Venue.Find(newVenue.GetId());

      //Assert
      Assert.Equal(newVenue, foundVenue);
    }
    [Fact]
    public void Test_DeleteOne_DeletesASpecificVenueObject()
    {
      //Arrange
      Venue firstVenue = new Venue("Chad's");
      firstVenue.Save();
      Venue secondVenue = new Venue("Todd's");
      secondVenue.Save();

      //Act
      secondVenue.Delete();
      List<Venue> expectedVenue = new List<Venue> {firstVenue};
      List<Venue> testVenue= Venue.GetAll();

      //Assert
      Assert.Equal(expectedVenue, testVenue);
    }
  }
}
