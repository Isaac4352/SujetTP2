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

namespace DetectionLangue.ViewModels
{
    public class DetectionLangueViewModel : BaseViewModel
    {
        private Detecteur _detecteur;

        public Detection[] detections { get; set; }

        public Detection selectedDetection { get; set; }

        int count;

        private readonly static string URL_BASE_API = " https://ws.detectlanguage.com/0.2/detect";
        public string Token = "b77c059e169d80f07e529ee6e53df079";

        private JsonSerializerSettings _jsonSettings;

        public RelayCommand CmdProchainePhoto { get; private set; }

        public RelayCommand CmdGoToConfiguration { get; private set; }
        public RelayCommand CmdAnnulerToken { get; private set; }
        public RelayCommand CmdSauvegarderToken { get; private set; }
        public RelayCommand CmdDetecteurDetect { get; private set; }

        ConfigurationView configuration { get; set; }
        Window window;
        //public string Token { get; private set; }   
        public string TmpToken { get; set; }

        public string Langue { get; set; }
        private bool _enExecution;

        //BoolToEstFiable convertBool;


        private string _confiance;

        public string Confiance
        {
            get { return _confiance; }
            set {
                _confiance = value;
                OnPropertyChanged();
            }
        }

        private string _estFiable;
        public string EstFiable { 
            get { return _estFiable; }
            set { 
                _estFiable = value;
                OnPropertyChanged(); 
            }
        }

        public Detecteur Detecteur
		{
			get { return _detecteur; }
			set { _detecteur = value; }
		}


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
            Detecteur.SetHttpRequestHeader("x-api-key", Token);
            detections = new Detection[10];
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

                
                Console.WriteLine("inside prochaine photo");
                Detecteur.SetHttpRequestHeader("Authorization", "Bearer " + Token);
                string json = await Detecteur.RequeteGetAsync($"?q={DetectTexte}");

             //   detections.

       
                var data = JsonConvert.DeserializeObject<JObject>(json, _jsonSettings);
                detections = new Detection[10];
                count = 0;
                foreach (var tmpDetection in data["data"]["detections"])
                {
                    if (tmpDetection["isReliable"] != null)
                    {
                        detections[count].isReliable = tmpDetection["isReliable"].ToString();
                        detections[count].confidence = tmpDetection["confidence"].ToString();
                        detections[count].language = tmpDetection["language"].ToString();
                    }
                    count++;
                }
                var detection = data["data"]["detections"][0];
                EstFiable = detection["isReliable"].ToString();
                Confiance = detection["confidence"].ToString();
                //Console.WriteLine(data.detections);
               // data.detections = JsonConvert.PopulateObject(json, data);F


                /*        if (_indexActuel < _photos.Count)
                        {
                            string json = await _client.RequeteGetAsync($"/images/{_photos[_indexActuel].id}");
                            Photo photo = JsonConvert.DeserializeObject<Photo>(json, _jsonSettings) ?? new Photo();

                            UrlPhoto = photo.url;
                            Races = "";
                            if (photo.breeds != null) Races = string.Join(",", photo.breeds);
                            _indexActuel++;
                            NbPhotoAffichees++;
                        }*/
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
            window.Show();
        }

        private void annulerToken(object? obj)
        {
            if(window != null)
            {
                window.Close();
                window.ShowDialog();
               // window.Visibility = Visibility.Collapsed;

            }  
        }

        private void sauvegarderToken(object? obj)
        {

            if (window != null)
            {
                Token = TmpToken;
                window.Visibility = Visibility.Hidden;
                window.Close();
            }

        }

        private void Detecter(object? obj)
        {
          
        }
    }
}
