using SugahriStore.Modelos;
using SugahriStore.Repositorios;
using System.Globalization;
using System.Net.NetworkInformation;
using static System.Net.WebRequestMethods;

namespace SugahriStore.Lógica.DatosCSV;

//Generaremos la lista de productos del CSV extraido de la tienda 
public static class CsvManagement
{
    //private static readonly string rutaPedidos = Path.Combine(AppContext.BaseDirectory, "Resources", "DatosEjemplo\\PedidosEjemplo.csv");
    private static readonly string rutaUsuarios = Path.Combine(AppContext.BaseDirectory, "Resources", "Users\\Usuarios.csv");
    private static readonly string rutaProductos = Path.Combine(AppContext.BaseDirectory, "Resources", "Productos\\Productos.csv");
    private static readonly string rutaPlaceholder = "https://i.etsystatic.com/isla/69d1eb/46127016/isla_500x500.46127016_qr9ms8u7.jpg?version=0";
    private static LineaPedidoRepositorio LineaPedidoRepositorio = new LineaPedidoRepositorio();
    public static List<Producto> DeserializarProductos()
    {
        List<Producto> productos = new();

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

    public static void SerializarPedidos(List<Pedido> pedidos, string rutaArchivo)
    {
        using (var writer = new StreamWriter(rutaArchivo))
        {
            // Escribir la cabecera del archivo
            writer.WriteLine("Name,Email,Financial Status,Fulfillment Status,Currency,Total,Lineitem name,Lineitem price,Lineitem quantity");

            foreach (var pedido in pedidos)
            {

                var lineasPedido = LineaPedidoRepositorio.BuscarLineasPedidoPorPedido(pedido.Id);

                if (lineasPedido.Any())
                {
                    foreach (var lineaPedido in lineasPedido)
                    {
                        // Escribir los campos de cada línea de pedido
                        writer.WriteLine($"{pedido.Nombre},{pedido.Email},{pedido.Estado},{pedido.EstadoDeEnvio},{pedido.Divisa},{pedido.Total},{lineaPedido.Nombre},{lineaPedido.Precio},{lineaPedido.Cantidad}");
                    }
                }
                else
                {
                    // Si no hay líneas de pedido, escribir una fila con campos vacíos
                    writer.WriteLine($"{pedido.Nombre},{pedido.Email},{pedido.Estado},{pedido.EstadoDeEnvio},{pedido.Divisa},{pedido.Total},,,");
                }
            }
        }

    }

    public static List<Pedido> DeserializarPedidos(string rutaArchivo)
    {
        List<Pedido> pedidos = new List<Pedido>();

        using (var reader = new StreamReader(rutaArchivo))
        {
            // Leer la cabecera del archivo
            string cabecera = reader.ReadLine();

            if (cabecera == "Name,Email,Financial Status,Fulfillment Status,Currency,Total,Lineitem name,Lineitem price,Lineitem quantity")
            {
                while (!reader.EndOfStream)
                {
                    // Leer cada línea del archivo CSV
                    string linea = reader.ReadLine();
                    string[] campos = linea.Split(',');

                    if (campos.Length == 9)
                    {
                        // Crear un nuevo pedido y asignar los valores de los campos
                        Pedido pedido = new Pedido
                        {
                            Nombre = campos[0],
                            Email = campos[1],
                            Estado = campos[2],
                            EstadoDeEnvio = campos[3],
                            Divisa = campos[4],
                            Total = decimal.Parse(campos[5]),
                            LineasPedido = new List<LineaPedido>()
                        };

                        // Crear una nueva línea de pedido y asignar los valores de los campos
                        LineaPedido lineaPedido = new LineaPedido
                        {
                            Nombre = campos[6],
                            Precio = decimal.Parse(campos[7]),
                            Cantidad = int.Parse(campos[8])
                        };

                        // Verificar si el valor en campos[7] es un número decimal válido
                        if (decimal.TryParse(campos[7], out decimal precio))
                        {
                            lineaPedido.Precio = precio;

                            // Agregar la línea de pedido al pedido correspondiente
                            pedido.LineasPedido.Add(lineaPedido);

                            // Agregar el pedido a la lista de pedidos
                            pedidos.Add(pedido);
                        }
                        else
                        {
                            // El valor en campos[7] no es un número decimal válido, realizar el manejo de error correspondiente
                            Console.WriteLine($"Error: El valor en campos[7] no es un número decimal válido. Valor: {campos[7]}");
                        }
                    }
                }
            }
            else
            {
                // La cabecera del archivo no coincide, realizar el manejo de error correspondiente
                Console.WriteLine("Error: La cabecera del archivo CSV no es válida");
            }
        }

        return pedidos;
    }



}












