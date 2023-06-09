using CommunityToolkit.Maui.Storage;
using SugahriStore.Datos;
using SugahriStore.Lógica.DatosCSV;
using SugahriStore.Modelos;

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

                // Verificar la extensión del archivo seleccionado
                string fileExtension = Path.GetExtension(filePath);
                if (fileExtension != ".csv")
                {
                    // Mostrar un mensaje de error si la extensión no es .csv
                    await DisplayAlert("Error", "Seleccione un archivo con extensión .csv", "Aceptar");
                    return;
                }

                // Mostrar la ruta del archivo importado en el label
                importFilePathLabel.Text = filePath;
            }
        }

        private async void OnExportFileClicked(object sender, EventArgs e)
        {
            // Solicitar al usuario que seleccione una carpeta para exportar el archivo
            var folder = await FolderPicker.PickAsync(default);

            // Verificar si se seleccionó una carpeta válida
            if (folder != null && folder.Folder != null)
            {
                // Mostrar la ruta de la carpeta seleccionada en una etiqueta
                exportFilePathLabel.Text = folder.Folder.Path;
            }
            else
            {
                // Mostrar una alerta de error si no se seleccionó ninguna carpeta
                await DisplayAlert("Error", "No se seleccionó ninguna carpeta.", "Aceptar");
            }
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

                if (pedidos != null && pedidos.Count > 0)
                {
                    this.PedidosRepositorio.InsertarPedidos(pedidos);

                    // Mostrar un mensaje de éxito
                    DisplayAlert("Éxito", "Archivo importado correctamente", "Aceptar");
                }
                else
                {
                    // Mostrar un mensaje de error si la lista de pedidos está vacía
                    DisplayAlert("Error", "El archivo es incorrecto o está vacío", "Aceptar");
                }
            }
            else
            {
                // Mostrar un mensaje de error si no se ha seleccionado un archivo para importar
                DisplayAlert("Error", "Por favor, selecciona un archivo para importar", "Aceptar");
            }
        }

    }
}
