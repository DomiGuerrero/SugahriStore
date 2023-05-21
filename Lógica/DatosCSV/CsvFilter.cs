using Prototipo_1_SugahriStore.Modelos;
using System.Globalization;
using System.Net.NetworkInformation;
using static System.Net.WebRequestMethods;

namespace Prototipo_1_SugahriStore.Lógica.DatosCSV;

//Generaremos la lista de productos del CSV extraido de la tienda 
public static class CsvManagement
{
    private static readonly string rutaPedidos = Path.Combine(AppContext.BaseDirectory, "Resources", "DatosEjemplo\\PedidosEjemplo.csv");
    private static readonly string rutaUsuarios = Path.Combine(AppContext.BaseDirectory, "Resources", "Users\\Usuarios.csv");
    private static readonly string rutaProductos = Path.Combine(AppContext.BaseDirectory, "Resources", "Productos\\Productos.csv");
    private static readonly string rutaPlaceholder = "https://i.etsystatic.com/isla/69d1eb/46127016/isla_500x500.46127016_qr9ms8u7.jpg?version=0";

    public static List<Producto> DeserializarProductos()
    {
        List<Producto> productos = new ();

        using (StreamReader reader = new(rutaProductos))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] fields = line.Split(',');

                string nombre = fields[0];
                int inventario = int.TryParse(fields[1], out int inv) ? inv : 0;
                decimal coste = decimal.TryParse(fields[2], NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal cst) ? cst : 0;
                string imageUrl = !string.IsNullOrEmpty(fields[3]) ? fields[3] : rutaPlaceholder;

                Producto producto = new(nombre, inventario, coste, imageUrl);

                productos.Add(producto);
            }
        }

        return productos;
    }
    public static void SerializarProductos(List<Producto> productos, string rutaArchivo)
    {
        using StreamWriter writer = new(rutaArchivo + ".csv");
        writer.WriteLine("Handle,Variant Inventory Qty,Variant Price,Variant Image");

        foreach (Producto producto in productos)
        {
            string nombre = producto.Nombre.Replace(",", "");
            int inventario = producto.Inventario;
            decimal coste = producto.Coste;
            string imageUrl = producto.ImageUrl;

            string line = $"{nombre},{inventario},{coste},{imageUrl}";
            writer.WriteLine(line);
        }
    }




    public static List<Usuario> DeserializarUsuarios()
    {
        List<Usuario> usuarios = new();
        using (var reader = new StreamReader(rutaUsuarios))
        {
            string[] headers = reader.ReadLine().Split(','); // Leemos la cabecera del archivo
            int nombreIndex = Array.IndexOf(headers, "Nombre");
            int contraseñaIndex = Array.IndexOf(headers, "Contraseña");
            int rolesIndex = Array.IndexOf(headers, "Roles");

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split(',');
                Rol rol = new()
                {
                    Nombre = fields[rolesIndex]
                };
                Usuario usuario = new()
                {
                    Nombre = fields[nombreIndex],
                    Contraseña = fields[contraseñaIndex],
                    Rol = rol
                };
                usuarios.Add(usuario);
            }
        }
        return usuarios;
    }

    public static List<Pedido> DeserializarPedidos()
    {
        List<Pedido> pedidos = new();
        Dictionary<string, Pedido> dictPedidos = new();

        using (var reader = new StreamReader(rutaPedidos))
        {
            string[] headers = reader.ReadLine().Split(','); // Leemos la cabecera del archivo
            int nombreIndex = Array.IndexOf(headers, "Name");
            int emailIndex = Array.IndexOf(headers, "Email");
            int estadoFinancieroIndex = Array.IndexOf(headers, "Financial Status");
            int estadoEnvioIndex = Array.IndexOf(headers, "Fulfillment Status");
            int monedaIndex = Array.IndexOf(headers, "Currency");
            int totalIndex = Array.IndexOf(headers, "Total");
            int lineaNombreIndex = Array.IndexOf(headers, "Lineitem name");
            int lineaPrecioIndex = Array.IndexOf(headers, "Lineitem price");
            int lineaCantidadIndex = Array.IndexOf(headers, "Lineitem quantity");

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split(',');
                string nombre = fields[nombreIndex];
                if (!dictPedidos.ContainsKey(nombre))
                {
                    Pedido pedido = new()
                    {
                        Nombre = nombre,
                        Email = fields[emailIndex],
                        Estado = fields[estadoFinancieroIndex],
                        EstadoDeEnvio = fields[estadoEnvioIndex],
                        Divisa = fields[monedaIndex],
                        Total = decimal.Parse(fields[totalIndex], new CultureInfo("en-US")),
                        LineasPedido = new List<LineaPedido>()
                    };
                    dictPedidos.Add(nombre, pedido);
                    pedidos.Add(pedido);
                }
                LineaPedido lineaPedido = new()
                {
                    Nombre = fields[lineaNombreIndex],
                    Precio = decimal.Parse(fields[lineaPrecioIndex], new CultureInfo("en-US")),
                    Cantidad = int.Parse(fields[lineaCantidadIndex])
                };
                dictPedidos[nombre].LineasPedido.Add(lineaPedido);
            }
        }
        return pedidos;
    }


}










