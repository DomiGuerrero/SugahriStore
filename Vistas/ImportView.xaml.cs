using CommunityToolkit.Maui.Storage;
using SugahriStore.Datos;
using SugahriStore.Lógica.DatosCSV;
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
            // Lógica para seleccionar archivo de importación
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
            var folder = await FolderPicker.PickAsync(default);

            exportFilePathLabel.Text = $"{folder.Folder.Path}";
        }


        private void OnExportClicked(object sender, EventArgs e)
        {
            // Validar si se ha seleccionado una ubicación de exportación
            if (!string.IsNullOrEmpty(exportFilePathLabel.Text))
            {
                // Obtener el nombre del archivo desde el Entry
                string fileName = filenameEntry.Text;

                // Validar si se ha proporcionado un nombre de archivo
                if (!string.IsNullOrEmpty(fileName))
                {
                    // Cambiar la extensión del archivo a ".csv"
                    string filePath = Path.ChangeExtension(Path.Combine(exportFilePathLabel.Text, fileName), "csv");

                    List<Pedido> pedidos = this.PedidosRepositorio.ObtenerPedidos();

                    // Realizar la lógica de exportación, por ejemplo, guardar los datos en el archivo especificado
                    CsvManagement.SerializarPedidos(pedidos, filePath);

                    // Mostrar un mensaje de éxito
                    DisplayAlert("Éxito", "Archivo exportado correctamente", "Aceptar");

                }
                else
                {
                    // Mostrar un mensaje de error si no se ha proporcionado un nombre de archivo
                    DisplayAlert("Error", "Por favor, ingresa un nombre de archivo", "Aceptar");
                }
            }
            else
            {
                // Mostrar un mensaje de error si no se ha seleccionado una ubicación de exportación
                DisplayAlert("Error", "Por favor, selecciona una ubicación de exportación", "Aceptar");
            }
        }

        private void OnImportClicked(object sender, EventArgs e)
        {
            // Validar si se ha seleccionado un archivo para importar
            if (!string.IsNullOrEmpty(importFilePathLabel.Text))
            {
                // Obtener la ruta del archivo importado
                string filePath = importFilePathLabel.Text;

                List<Pedido> pedidos = CsvManagement.DeserializarPedidos(filePath);
                this.PedidosRepositorio.InsertarPedidos(pedidos);

                 // Mostrar un mensaje de éxito
                 DisplayAlert("Éxito", "Archivo importado correctamente", "Aceptar");
            }
            else
            {
                // Mostrar un mensaje de error si no se ha seleccionado un archivo para importar
                DisplayAlert("Error", "Por favor, selecciona un archivo para importar", "Aceptar");
            }
        }


    }
}
