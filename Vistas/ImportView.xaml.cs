using SugahriStore.Datos;
using SugahriStore.L�gica.DatosCSV;
using SugahriStore.Modelos;
using System;
using System.IO;

namespace SugahriStore.Vistas
{
    public partial class ImportView : ContentPage
    {
        private PedidosRepositorio PedidosRepositorio = new();
        public ImportView()
        {
            InitializeComponent();
        }

        private async void OnImportFileClicked(object sender, EventArgs e)
        {
            // L�gica para seleccionar archivo de importaci�n
            var fileResult = await FilePicker.PickAsync();

            if (fileResult != null)
            {
                // Obtener la ruta del archivo seleccionado
                string filePath = fileResult.FullPath;

                // Mostrar la ruta del archivo importado en el label
                importFilePathLabel.Text = filePath;
            }
        }

        private async void OnExportFileClicked(object sender, EventArgs e)
        {
            // L�gica para seleccionar ubicaci�n de exportaci�n
            var fileResult = await FilePicker.PickAsync();

            if (fileResult != null)
            {
                // Obtener la ruta de la ubicaci�n de exportaci�n seleccionada
                string filePath = fileResult.FullPath;

                // Mostrar la ruta de exportaci�n en el label
                exportFilePathLabel.Text = filePath;
            }
        }


        private void OnExportClicked(object sender, EventArgs e)
        {
            // Validar si se ha seleccionado una ubicaci�n de exportaci�n
            if (!string.IsNullOrEmpty(exportFilePathLabel.Text))
            {
                // Obtener el nombre del archivo desde el Entry
                string fileName = filenameEntry.Text;

                // Validar si se ha proporcionado un nombre de archivo
                if (!string.IsNullOrEmpty(fileName))
                {
                    // Combinar la ubicaci�n de exportaci�n y el nombre de archivo para obtener la ruta completa del archivo
                    string filePath = Path.Combine(exportFilePathLabel.Text, fileName);

                    List<Pedido> pedidos = this.PedidosRepositorio.ObtenerPedidos();

                    // Realizar la l�gica de exportaci�n, por ejemplo, guardar los datos en el archivo especificado
                    CsvManagement.SerializarPedidos(pedidos, filePath);

                    // Mostrar un mensaje de �xito
                    DisplayAlert("�xito", "Archivo exportado correctamente", "Aceptar");
                }
                else
                {
                    // Mostrar un mensaje de error si no se ha proporcionado un nombre de archivo
                    DisplayAlert("Error", "Por favor, ingresa un nombre de archivo", "Aceptar");
                }
            }
            else
            {
                // Mostrar un mensaje de error si no se ha seleccionado una ubicaci�n de exportaci�n
                DisplayAlert("Error", "Por favor, selecciona una ubicaci�n de exportaci�n", "Aceptar");
            }


        }
        private void OnImportClicked(object sender, EventArgs e)
        {
            // Validar si se ha seleccionado un archivo para importar
            if (!string.IsNullOrEmpty(importFilePathLabel.Text))
            {
                // Obtener la ruta del archivo importado
                string filePath = importFilePathLabel.Text;

                // Realizar la l�gica de importaci�n utilizando la ruta del archivo

                // Mostrar un mensaje de �xito
                DisplayAlert("�xito", "Archivo importado correctamente", "Aceptar");
            }
            else
            {
                // Mostrar un mensaje de error si no se ha seleccionado un archivo para importar
                DisplayAlert("Error", "Por favor, selecciona un archivo para importar", "Aceptar");
            }
        }


    }
}
