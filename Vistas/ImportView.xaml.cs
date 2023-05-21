namespace SugahriStore.Vistas;

public partial class ImportView : ContentPage
{
	public ImportView(MainPage mainPage)
	{
		InitializeComponent();
	}

    private async void OnImportFileClicked(object sender, EventArgs e)
    {
        try
        {
            FileResult result = await FilePicker.PickAsync();

            if (result != null)
            {
                Stream stream = await result.OpenReadAsync();
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Leer archivo y hacer algo con los datos
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar excepción
        }
    }

    private async void OnExportFileClicked(object sender, EventArgs e)
    {
        try
        {
            FileResult result = await FilePicker.PickAsync();

            if (result != null)
            {
                string filename = filenameEntry.Text;
                if (string.IsNullOrEmpty(filename))
                {
                    filename = "file";
                }

                string extension = Path.GetExtension(result.FullPath);
                if (string.IsNullOrEmpty(extension))
                {
                    extension = ".csv"; // Cambiar por la extensión que necesites
                }

                string filepath = Path.Combine(result.FullPath, $"{filename}{extension}");

                using (StreamWriter writer = new StreamWriter(filepath))
                {
                    // Escribir datos en el archivo
                }

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filepath)
                });
            }
        }
        catch (Exception ex)
        {
            // Manejar excepción
        }
    }
}
