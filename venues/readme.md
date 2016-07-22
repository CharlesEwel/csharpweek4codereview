# Csharp Week 4 Code Review

#### By Charlie Ewel, 07/15/2016

## Description

This program will deal with relationships between bands and venues which is a many to many relationship. It will allow users to see what bands a given venue has hosted and what venues a given band has played.

## Setup/Installation Requirements

This program can only be accessed on a PC with Windows 10, and with git and atom installed.

* Clone this repository
* Enter into the cloned directory in PowerShell
* Type following command into the Windows PowerShell > sqlcmd -S "(localdb)\mssqllocaldb" -i database_backups\band_tracker.sql
* Type following command into the Windows PowerShell > sqlcmd -S "(localdb)\mssqllocaldb" -i database_backups\band_tracker_test.sql
* Type following command into the Windows PowerShell > dnu restore
* Type following command into PowerShell > dnx kestrel
* Open Chrome and type in the following address: "localhost:5004"

## Known Bugs

No known bugs.

## Specifications

The program should ... | Example Input | Example Output | Why'd we choose this?
----- | ----- | ----- | ------
Allow user to add bands and venues| User fills out a form with band/venue information | That band/venue is added to it's respective list | This is the basic functionality for save, get all, and find for our band classes.
Allow user to edit and delete individual venues |Change venue's name from "Joe's" to "Charlie's"| The venue now listed as Joe's will become known as Charlie's | Allows us to test CRUD for venues
Allow user to add show| User enters a band and venue name in a 'add show' form | That band will be added to the list of bands who have played that venue, and that venue will be added to the list of venues played by that band | This allows us to implement the many to many relationship between bands and venues

## Support and Contact Details

Contact Epicodus for support in running this program.

## Technologies Used

* HTML
* C#

## License

*This software is licensed under the Microsoft ASP.NET license.*

Copyright (c) 2016 Charles Ewel
