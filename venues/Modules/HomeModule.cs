using Nancy;
using System.Collections.Generic;
using System.Data.SqlClient;
using BandTracker.Objects;
using System;

namespace BandTracker
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => View["index.cshtml"];

      Get["/bands"] = _ =>
      {
        List<Band> allBands = Band.GetAll();
        return View["bands.cshtml", allBands];
      };

      Get["/bands/add"] = _ => View["band_new.cshtml"];

      Post["/bands/add"] = _ =>
      {
        Band newBand = new Band(Request.Form["band-name"]);
        newBand.Save();
        List<Band> allBands = Band.GetAll();
        return View["bands.cshtml", allBands];
      };

      Get["/venues"] = _ =>
      {
        List<Venue> allVenues = Venue.GetAll();
        return View["venues.cshtml", allVenues];
      };

      Get["/venues/add"] = _ => View["venue_new.cshtml"];

      Post["/venues/add"] = _ =>
      {
        Venue newVenue = new Venue(Request.Form["venue-name"]);
        newVenue.Save();
        List<Venue> allVenues = Venue.GetAll();
        return View["venues.cshtml", allVenues];
      };
    }
  }
}
