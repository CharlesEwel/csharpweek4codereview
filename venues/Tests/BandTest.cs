using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using BandTracker.Objects;


namespace BandTracker.Tests
{
  public class BandTest : IDisposable
  {
    public BandTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Band.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Band.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Save_SavesCorrectObjectToDatabase()
    {
      //Arrange
      Band newBand = new Band("Ween");

      //Act
      newBand.Save();
      Band savedBand = Band.GetAll()[0];

      //Assert
      Assert.Equal(newBand, savedBand);
    }
    [Fact]
    public void Test_Find_ReturnsASpecificBandObject()
    {
      //Arrange
      Band newBand = new Band("Ween");
      newBand.Save();

      //Act
      Band foundBand = Band.Find(newBand.GetId());

      //Assert
      Assert.Equal(newBand, foundBand);
    }
    [Fact]
    public void Test_DeleteOne_DeletesASpecificBandObject()
    {
      //Arrange
      Band firstBand = new Band("Ween");
      firstBand.Save();
      Band secondBand = new Band("Dexy's Midnight Runners");
      secondBand.Save();

      //Act
      secondBand.Delete();
      List<Band> expectedBand = new List<Band> {firstBand};
      List<Band> testBand= Band.GetAll();

      //Assert
      Assert.Equal(expectedBand, testBand);
    }
  }
}
