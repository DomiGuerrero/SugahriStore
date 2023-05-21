using System.Net;
using SugahriStore.Lógica;

namespace SugahriStore.Modelos;
public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Inventario { get; set; }
    public decimal Coste { get; set; }
    public string ImageUrl { get; set; }

    public Producto(string nombre, int inventario, decimal coste, string imageUrl)
    {
        Nombre = nombre;
        Inventario = inventario;
        Coste = coste;
        ImageUrl = imageUrl;
    }
}
