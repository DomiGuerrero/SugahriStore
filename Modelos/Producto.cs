
namespace SugahriStore.Modelos;
public class Producto
{
    public int Id { get; set; } // Identificador único del producto
    public string Nombre { get; set; } // Nombre del producto
    public int Inventario { get; set; } // Cantidad disponible en inventario del producto
    public decimal Coste { get; set; } // Precio del producto
    public string ImageUrl { get; set; } // URL de la imagen del producto

    public Producto(string nombre, int inventario, decimal coste, string imageUrl)
    {
        Nombre = nombre; // Asignar el nombre proporcionado al producto

        Inventario = inventario; // Asignar la cantidad de inventario proporcionada al producto

        Coste = coste; // Asignar el costo proporcionado al producto

        ImageUrl = imageUrl; // Asignar la URL de la imagen proporcionada al producto
    }

}
