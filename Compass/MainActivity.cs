using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using System;
using Android.Runtime;
using System.Threading.Tasks;

namespace Compass
{
    [Activity(Label = "Compass", MainLauncher = true)]
    public class MainActivity : Activity, ILocationListener
    {
        private Button _getCoordinatesButton;
        private TextView _textLatitude;
        private TextView _textLongitude;
        private TextView _textError;
        private TextView _textDistance;
        private ImageView _compassImage;

        private LocationManager _locationManager;
        private string _locationProvider;
        Location _currentLocation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            _getCoordinatesButton = base.FindViewById<Button>(Resource.Id.buttonGetCoordinates);
            _textLatitude = base.FindViewById<TextView>(Resource.Id.textLatitude);
            _textLongitude = base.FindViewById<TextView>(Resource.Id.textLongitude);
            _textError = base.FindViewById<TextView>(Resource.Id.textError);
            _textDistance = base.FindViewById<TextView>(Resource.Id.textDistance);
            _compassImage = base.FindViewById<ImageView>(Resource.Id.imageCompassPointer);

            _getCoordinatesButton.Click += (sender, ev) =>
            {
                if (_currentLocation == null)
                {
                    _textError.Text = "Não foi possível obter sua localização atual";
                    return;
                }

                this.DisplayLocation(_currentLocation);
            };

            InitializeLocationManager();
        }

        void DisplayLocation(Location location)
        {
            if (location == null)
            {
                _textError.Text = "Null Location";
            }
            else
            {
                _textLatitude.Text = "Latitude: " + location.Latitude;
                _textLongitude.Text = "Longitude: " + location.Longitude;
                _textError.Text = String.Empty;

                var relationPosition = new RelationPosition(-46.646722, -23.535515, location.Latitude, location.Longitude);
                _textDistance.Text = relationPosition.ToString();
                _compassImage.Rotation = (float)relationPosition.InclinationX;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                _textError.Text = "Unable to determine your location";
            }
            else
            {
                DisplayLocation(_currentLocation);
            }
        }
        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }

        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);

            Criteria criteriaForLocationService = new Criteria()
            {
                Accuracy = Accuracy.Fine
            };

            var acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = String.Empty;
            }
        }

    }
}

