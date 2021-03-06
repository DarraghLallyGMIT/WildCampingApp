﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//Geo location API
using Xamarin.Essentials;
using System.IO;

namespace WildCampingApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationPage : ContentPage
	{
        //Constant - File names
        private const string OUTPUT_FILE_LAT = "lastLatitude.txt";
        private const string OUTPUT_FILE_LONG = "lastLongitude.txt";

        //Global Variables
        string saveLat;
        string saveLong;

        public LocationPage ()
		{
			InitializeComponent ();
            //Load Saved File
            loadFile();
            //Calling location
            currentLocation();
            
		}

        //Load Saved File
        private void loadFile() {
            try {
                //Find default save to folder, save as string
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                //Point to file
                string fileLat = Path.Combine(path, OUTPUT_FILE_LAT);
                string fileLong = Path.Combine(path, OUTPUT_FILE_LONG);
                //Read from file
                string inputLastLat = File.ReadAllText(fileLat);
                string inputLastLong = File.ReadAllText(fileLong);
                //Display last
                lblLocationDisplayLast_Lat.Text = inputLastLat;
                lblLocationDisplayLast_Long.Text = inputLastLong;
            }
            catch
            {

            }
        }

        //Save Last Known Location
        private void saveLocation()
        {
            //Find default save to folder, save as string
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //Point to files
            string fileLat = Path.Combine(path, OUTPUT_FILE_LAT);
            string fileLong = Path.Combine(path, OUTPUT_FILE_LONG);
            //Create outputs
            string outputLat = saveLat;
            string outputLong = saveLong;
            //Write to files
            File.WriteAllText(fileLat, outputLat);
            File.WriteAllText(fileLong, outputLong);
        }

        //Method for getting current location - code shell from zamarin forms blog
        public async void currentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {

                    //**Converting GPS Lat to DMS Lat              
                    // Getting Degrees and Converting minutes
                    int latDegrees = (int)location.Latitude;
                    double latDecOne = location.Latitude - latDegrees;
                    double latMinutesDec = latDecOne * 60;
                    //Converting seconds and rounding seconds to 2 places
                    int latMinutes = (int)latMinutesDec;
                    double latDecTwo = latMinutesDec - latMinutes;
                    double latSeconds = latDecTwo * 60;
                    double latSecondsRound = Math.Round(latSeconds, 2, MidpointRounding.AwayFromZero);
                    //Display compass direction
                    String compassHeadingLat;
                    if (location.Latitude > 0)
                    {
                        compassHeadingLat = "North";
                    }
                    else {
                        compassHeadingLat = "South";
                    }
                    //Display to label
                    String outputLat = compassHeadingLat + "\n" + Math.Abs(latDegrees) +  " degree's\n" + Math.Abs((int)latMinutes)
                        + " minute's\n" + Math.Abs(latSecondsRound) + " second's";
                    lblLocationDisplay_Lat.Text = outputLat;

                    //Save output to global variable for saving
                    saveLat = outputLat;

                    //**Converting GPS Long to DMS Long              
                    // Getting Degrees and Converting minutes
                    int lonDegrees = (int)location.Longitude;
                    double lonDecOne = location.Longitude - lonDegrees;
                    double lonMinutesDec = lonDecOne * 60;
                    //Converting seconds and rounding seconds to 2 places
                    int lonMinutes = (int)lonMinutesDec;
                    double lonDecTwo = lonMinutesDec - lonMinutes;
                    double lonSeconds = lonDecTwo * 60;
                    double lonSecondsRound = Math.Round(lonSeconds, 2, MidpointRounding.AwayFromZero);
                    //Display compass direction
                    String compassHeadingLon;
                    if (location.Longitude > 0)
                    {
                        compassHeadingLon = "East";
                    }
                    else
                    {
                        compassHeadingLon = "West";
                    }
                    //Display to label
                    String outputLon = compassHeadingLon + "\n" + Math.Abs(lonDegrees) + " degree's\n" + Math.Abs((int)lonMinutes) 
                        + " minute's\n" + Math.Abs(lonSecondsRound) + " second's";
                    lblLocationDisplay_Long.Text = outputLon;
                    //Save output to global variable for saving
                    saveLong = outputLon;
                }
                //Save location
                saveLocation();
                
            }//try
            catch (FeatureNotSupportedException fnsEx)
            {
                lblLocationDisplay_Lat.Text = "Unable to get Location";
                lblLocationDisplay_Long.Text = "Feature Not Supported";
            }
            catch (PermissionException pEx)
            {
                lblLocationDisplay_Lat.Text = "Unable to get Location";
                lblLocationDisplay_Long.Text = "Please Enable Location Settings";
            }
            catch (Exception ex)
            {
                lblLocationDisplay_Lat.Text = "Unable to get Location";
            }

        }
    }
}