using DetectionLangue.Models;
using DetectionLangue.ViewModels.Commands;
using DetectionLangue.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using System.Printing;
using System.Windows.Media.Animation;
using DetectionLangue.ViewModels.Converters;
using System.Windows.Controls;
using System.DirectoryServices.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace DetectionLangue.ViewModels
{
    public class DetectionLangueViewModel : BaseViewModel
    {
        private Detecteur _detecteur;
        public Detecteur Detecteur
        {
            get { return _detecteur; }
            set { _detecteur = value; }
        }

        private ObservableCollection<Detection> _detections;

        public ObservableCollection<Detection> Detections
        {
            get { return _detections; }
            set { _detections = value; OnPropertyChanged(); }
        }

        private Detection _selectedDetection;

        public Detection SelectedDetection {
            get { return _selectedDetection; }
            set {  _selectedDetection = value;
                OnPropertyChanged();
            }
        }

        int count;

        private readonly static string URL_BASE_API = " https://ws.detectlanguage.com/0.2/detect";
        public string Token;

        private JsonSerializerSettings _jsonSettings;

        //Commandes
        public RelayCommand CmdGoToConfiguration { get; private set; }
        public RelayCommand CmdAnnulerToken { get; private set; }
        public RelayCommand CmdSauvegarderToken { get; private set; }
        public RelayCommand CmdDetecteurDetect { get; private set; }

        //pour la page de configuration
        ConfigurationView configuration { get; set; }
        Window window;
        public string TmpToken { get; set; }

        private bool _enExecution;


        private bool _texteExiste;

        public bool TexteExiste
        {
            get { return _texteExiste; }
            set
            {
                _texteExiste = value;
                OnPropertyChanged();
            }
        }

        private string _detectTexte;

        public string DetectTexte
        {
            get { return _detectTexte; }
            set { 
                _detectTexte = value; 
                if(value == null || value == "")
                {
                    TexteExiste = false;
                }
                else
                {
                    TexteExiste = true;
                }
        
            }
        }


        public DetectionLangueViewModel() {
            CmdGoToConfiguration = new RelayCommand(GotoConfiguration, null);
            CmdAnnulerToken = new RelayCommand(annulerToken, null);
            CmdSauvegarderToken = new RelayCommand(sauvegarderToken, null);
            CmdDetecteurDetect = new RelayCommand(DetecteurDetect, null);

            Detecteur = new Detecteur(URL_BASE_API);
            Detections = new ObservableCollection<Detection>();

            TexteExiste = false;

            window = new Window();
            _jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

        }

        public async void DetecteurDetect(object? obj)
        {
            try
            {
                _enExecution = true;

                string json = await Detecteur.RequeteGetAsync($"?q={DetectTexte}");
       
                var data = JsonConvert.DeserializeObject<JObject>(json, _jsonSettings);
                
                //new observablecollection pour faire un reset du combobox
                Detections = new ObservableCollection<Detection>();

                if (data["error"] == null) {
                    foreach (var tmpDetection in data["data"]["detections"])
                    {
          
                        if (tmpDetection["isReliable"] != null)
                        {
                            Detections.Add(new Detection(tmpDetection["language"].ToString(), tmpDetection["isReliable"].ToString(), tmpDetection["confidence"].ToString()));
                        }
                        count++;
                    }
                    SelectedDetection = Detections[0];
                }
                else
                {
                    MessageBox.Show("mauvais token");
                }
            }
            catch (Exception ex) { }
            finally
            {
                _enExecution = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void GotoConfiguration(object? obj)
        {
            window = new Window();
            configuration = new ConfigurationView();
            window.Content = configuration;
            window.DataContext = this;
            window.Show();
        }

        private void annulerToken(object? obj)
        {
            if(window != null)
            {
                window.Close();
            }  
        }

        private void sauvegarderToken(object? obj)
        {

            if (window != null)
            {
                window.Close();
                Token = TmpToken;
                Detecteur.SetHttpRequestHeader("Authorization", "Bearer " + Token);
            }

        }
    }
}
