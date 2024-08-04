using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Principal.Server.Controller.ViewModels
{
    public class ServiceInstallationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _selectedRegion;
        private string _userName;
        private int _rotationDays;

        public string SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                if (_selectedRegion != value)
                {
                    _selectedRegion = value;
                    OnPropertyChanged(nameof(SelectedRegion));
                }
            }
        }

        public ObservableCollection<string> Regions { get; } = new ObservableCollection<string>
        {
            "us-east-1",
            "us-east-2",
            "us-west-1"
        };

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public int RotationDays
        {
            get => _rotationDays;
            set
            {
                _rotationDays = value;
                OnPropertyChanged(nameof(RotationDays));
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case nameof(SelectedRegion):
                        if (string.IsNullOrWhiteSpace(SelectedRegion))
                            result = "La región es requerida.";
                        break;
                    case nameof(UserName):
                        if (string.IsNullOrWhiteSpace(UserName))
                            result = "El usuario es requerido.";
                        break;
                }
                return result;
            }
        }

        public void SaveToJson(string filePath)
        {
            if (string.IsNullOrWhiteSpace(SelectedRegion) || string.IsNullOrWhiteSpace(UserName))
            {
                return;
            }

            if (RotationDays is < 30 or > 60)
            {
                return;
            }

            var settings = new
            {
                OwinServer = new
                {
                    Url = "http://localhost:8080"
                },
                Aws = new
                {
                    UserMonitor = UserName,
                    RegionName = SelectedRegion,
                    RotationDays,
                    DeactivationDays = 50
                }
            };

            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            var json = File.ReadAllText(filePath);
            var settings = JObject.Parse(json);

            SelectedRegion = settings["Aws"]?["RegionName"]?.ToString();
            UserName = settings["Aws"]?["UserMonitor"]?.ToString();
            RotationDays = settings["Aws"]?["RotationDays"]?.ToObject<int>() ?? 30;
        }

        public string Error => null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
