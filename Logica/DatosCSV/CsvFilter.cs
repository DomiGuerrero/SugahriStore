using CsvHelper;
using CsvHelper.Configuration;
using SugahriStore.Datos;
using SugahriStore.Modelos;
using SugahriStore.Repositorios;
using System.Formats.Asn1;
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
    private static LineaPedidoRepositorio LineaPedidoRepositorio = new();
    private static AuditoriaRepositorio AuditoriaRepositorio = new();
    private static ClienteRepositorio ClienteRepositorio = new();


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
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true,
            MissingFieldFound = null
        };

        using (var writer = new StreamWriter(rutaArchivo))
        using (var csv = new CsvWriter(writer, csvConfig))
        {
            // Escribir la cabecera
            csv.WriteField("Name");
            csv.WriteField("Email");
            csv.WriteField("Financial Status");
            csv.WriteField("Fulfillment Status");
            csv.WriteField("Currency");
            csv.WriteField("Total");
            csv.WriteField("Lineitem name");
            csv.WriteField("Lineitem price");
            csv.WriteField("Lineitem quantity");
            csv.WriteField("Shipping Name");
            csv.WriteField("Shipping Street");
            csv.WriteField("Shipping Country");
            csv.NextRecord();

            foreach (var pedido in pedidos)
            {
                // Obtener el cliente y las líneas de pedido del repositorio
                Cliente cliente = ClienteRepositorio.ObtenerClientePorPedido(pedido);
                List<LineaPedido> lineasPedido = LineaPedidoRepositorio.BuscarLineasPedidoPorPedido(pedido.Id);

                // Escribir los datos del pedido
                csv.WriteField(pedido.Nombre);
                csv.WriteField(pedido.Email);
                csv.WriteField(pedido.Estado);
                csv.WriteField(pedido.EstadoDeEnvio);
                csv.WriteField(pedido.Divisa);
                csv.WriteField(pedido.Total);
                csv.WriteField(string.Empty);
                csv.WriteField(string.Empty);
                csv.WriteField(string.Empty);
                csv.WriteField(cliente.Nombre);
                csv.WriteField(cliente.Direccion);
                csv.WriteField(cliente.Ciudad);
                csv.NextRecord();

                // Escribir las líneas de pedido
                foreach (var lineaPedido in lineasPedido)
                {
                    csv.WriteField(string.Empty);
                    csv.WriteField(string.Empty);
                    csv.WriteField(string.Empty);
                    csv.WriteField(string.Empty);
                    csv.WriteField(string.Empty);
                    csv.WriteField(string.Empty);
                    csv.WriteField(lineaPedido.Nombre);
                    csv.WriteField(lineaPedido.Precio);
                    csv.WriteField(lineaPedido.Cantidad);
                    csv.WriteField(string.Empty);
                    csv.WriteField(string.Empty);
                    csv.WriteField(string.Empty);
                    csv.NextRecord();
                }
            }
        }
    }


    public static List<Pedido> DeserializarPedidos(string rutaArchivo)
    {
        List<Pedido> pedidos = new List<Pedido>();

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true,
            MissingFieldFound = null
        };

        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvReader(reader, csvConfig))
        {
            csv.Read();
            csv.ReadHeader();

            if (csv.HeaderRecord.Contains("Name") && csv.HeaderRecord.Contains("Email") &&
                csv.HeaderRecord.Contains("Financial Status") && csv.HeaderRecord.Contains("Fulfillment Status") &&
                csv.HeaderRecord.Contains("Currency") && csv.HeaderRecord.Contains("Total") &&
                csv.HeaderRecord.Contains("Lineitem name") && csv.HeaderRecord.Contains("Lineitem price") &&
                csv.HeaderRecord.Contains("Lineitem quantity") && csv.HeaderRecord.Contains("Shipping Name") &&
                csv.HeaderRecord.Contains("Shipping Street") && csv.HeaderRecord.Contains("Shipping Country"))
            {
                while (csv.Read())
                {
                    string nombre = csv.GetField<string>("Name");
                    string email = csv.GetField<string>("Email");
                    string estado = csv.GetField<string>("Financial Status");
                    string estadoEnvio = csv.GetField<string>("Fulfillment Status");
                    string divisa = csv.GetField<string>("Currency");
                    string totalStr = csv.GetField<string>("Total");
                    decimal total;

                    if (!string.IsNullOrEmpty(totalStr) && decimal.TryParse(totalStr.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out total))
                    {
                        string lineaNombre = csv.GetField<string>("Lineitem name");
                        decimal lineaPrecio = csv.GetField<decimal>("Lineitem price");
                        int lineaCantidad = csv.GetField<int>("Lineitem quantity");

                        string clienteNombre = csv.GetField<string>("Shipping Name");
                        string clienteDireccion = csv.GetField<string>("Shipping Street");
                        string clienteCiudad = csv.GetField<string>("Shipping Country");

                        Cliente cliente = new Cliente
                        {
                            Nombre = clienteNombre,
                            Direccion = clienteDireccion,
                            Ciudad = clienteCiudad
                        };

                        Pedido pedido = new Pedido
                        {
                            Nombre = nombre,
                            Email = email,
                            Estado = estado,
                            EstadoDeEnvio = estadoEnvio,
                            Divisa = divisa,
                            Total = total,
                            LineasPedido = new List<LineaPedido>(),
                            Auditoria = new Auditoria
                            {
                                Fecha = DateTime.Now,
                                NombrePedido = nombre
                            },
                            Cliente = cliente
                        };

                        LineaPedido lineaPedido = new LineaPedido
                        {
                            Nombre = lineaNombre,
                            Precio = lineaPrecio,
                            Cantidad = lineaCantidad
                        };

                        pedido.LineasPedido.Add(lineaPedido);
                        pedidos.Add(pedido);
                    }
                    else
                    {
                        Console.WriteLine($"Error: No se pudo convertir el valor '{totalStr}' a decimal para el campo 'Total'");
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: La cabecera del archivo CSV no es válida");
            }
        }

        return pedidos;
    }


}




