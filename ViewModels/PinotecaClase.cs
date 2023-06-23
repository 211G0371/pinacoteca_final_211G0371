using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using pinacoteca_final_211G0371.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace pinacoteca_final_211G0371.ViewModels
{
    public class PinotecaClase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        public string? Error { get; set; }
        public ObservableCollection<PinotecaModels> pinotecaModels { get; set; } = new ObservableCollection<PinotecaModels>();
        public PinotecaModels Pinoteca { get; set; } = new PinotecaModels();
        public ICommand? AgregarCommand { get; set; }
        public ICommand? EditarCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }
        public ICommand? CambiarVistaCommand { get; set; }
        public ICommand? CancelarCommand { get; set; }
        public int Indice { get; set; }
        public string Vista { get; set; } = "ver";

        public PinotecaClase()
        {
            Cargar();

            //Propiedades apuntan a los metodos 
            AgregarCommand = new RelayCommand(Agregar);

            EditarCommand = new RelayCommand(Editar);

            EliminarCommand = new RelayCommand(Eliminar);

            CancelarCommand = new RelayCommand(Cancelar);

            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }
        public void Agregar()
        {
            Error = "";
            if (string.IsNullOrEmpty(Pinoteca.Nombre))
            {
                Error += "Ingrese el nombre\n";
            }
            if (string.IsNullOrEmpty(Pinoteca.Ciudad))
            {
                Error += "Ingrese la ciudad";
            }
            if (string.IsNullOrEmpty(Pinoteca.Dirreccion))
            {
                Error += "Ingrese la direccion";
            }
            int i = 0;
            foreach (PinotecaModels item in pinotecaModels)
            {
                if (item.Nombre == Pinoteca.Nombre)
                {
                    MessageBox.Show("Ingresa otro nombre");
                    return;
                }
                else
                {
                    i++;
                }
            }
            if (Error == "" && pinotecaModels != null)
            {
                pinotecaModels.Add(Pinoteca);
                Guardar();
                Vista = "ver";
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public void Editar()
        {
            if (pinotecaModels != null)
            {
                pinotecaModels[Indice] = Pinoteca;
                Guardar();
                CambiarVista("ver");
            }
        }

        public void Eliminar()
        {
            if (Pinoteca != null && pinotecaModels != null)
            {
                pinotecaModels.Remove(Pinoteca);
                CambiarVista("ver");
                Guardar();
            }
        }
        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(pinotecaModels); //Serializamos 
                                                                    //la observableCollection
            File.WriteAllText("Datos.json", json);
        }
        public void Cargar()
        {
            if (File.Exists("Datos.json"))
            {
                var json = File.ReadAllText("Datos.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<PinotecaModels>>(json);
                if (datos != null)
                {
                    pinotecaModels = (ObservableCollection<PinotecaModels>)datos;
                }
                else
                {
                    pinotecaModels = new ObservableCollection<PinotecaModels>();
                }
            }
        }
        public void CambiarVista(string vistaACambiar)
        {
            Vista = vistaACambiar;
            if (vistaACambiar == "agregar")
            {

                Pinoteca = new PinotecaModels();
            }
            if (vistaACambiar == "editar")
            {
                if (pinotecaModels != null)
                {
                    int indice = pinotecaModels.IndexOf(Pinoteca);
                }
                PinotecaModels copia = new PinotecaModels()
                {
                    Ciudad = Pinoteca.Ciudad,
                    Dirreccion = Pinoteca.Dirreccion,
                    Mt2 = Pinoteca.Mt2,

                };
                Pinoteca = copia;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("vista"));
        }
        public void Cancelar()
        {
            Vista = "ver";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
        }
    }
}
