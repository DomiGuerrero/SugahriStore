using CsvHelper;
using CsvHelper.Configuration;
using SugahriStore.Datos;
using SugahriStore.Modelos;
using System.Globalization;


namespace SugahriStore.Lógica.DatosCSV;

public static class CsvManagement
{

    private static readonly string Placeholder = "https://i.etsystatic.com/isla/69d1eb/46127016/isla_500x500.46127016_qr9ms8u7.jpg?version=0";
    private static AuditoriaRepositorio AuditoriaRepositorio = new();

    public static List<Producto> DeserializarProductos(string rutaArchivo)
    {
        // Crear una lista vacía para almacenar los productos
        List<Producto> productos = new List<Producto>();

        // Configuración del CSV
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",                        // Delimitador de campos
            HasHeaderRecord = true,                  // Indica si el archivo tiene una línea de encabezado
            MissingFieldFound = null                 // Acción a realizar si se encuentra un campo faltante (ninguna en este caso)
        };

        // Abrir el archivo para leer
        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvReader(reader, csvConfig))
        {
            csv.Read();                             // Saltar la primera línea (normalmente el encabezado)
            csv.ReadHeader();                       // Leer la línea de encabezado

            // Verificar si los encabezados necesarios están presentes
            if (csv.HeaderRecord.Contains("Title") && csv.HeaderRecord.Contains("Variant Inventory Qty") &&
                csv.HeaderRecord.Contains("Variant Price") && csv.HeaderRecord.Contains("Image Src"))
            {
                // Leer cada línea del archivo
                while (csv.Read())
                {
                    // Obtener los valores de los campos de la línea actual
                    string nombre = csv.GetField<string>("Title");
                    decimal? inventarioDecimal = csv.GetField<decimal?>("Variant Inventory Qty");
                    int inventario = inventarioDecimal.HasValue ? (int)inventarioDecimal.Value : 0;
                    decimal? costeDecimal = csv.GetField<decimal?>("Variant Price");
                    decimal coste = costeDecimal.HasValue ? costeDecimal.Value : 0;
                    string imageUrl = csv.GetField<string>("Image Src");

                    // Si la URL de la imagen está vacía, asignar un marcador de posición
                    string finalImageUrl = string.IsNullOrEmpty(imageUrl) ? Placeholder : imageUrl;

                    // Verificar que el nombre no esté vacío y que el costo sea mayor que 0 antes de agregar el producto
                    if (!string.IsNullOrEmpty(nombre))
                    {
                        if (coste > 0)
                        {
                            // Crear un nuevo objeto Producto y agregarlo a la lista
                            Producto producto = new Producto(nombre, inventario, coste, finalImageUrl);
                            productos.Add(producto);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: La cabecera del archivo CSV no es válida");
            }
        }

        return productos;   // Devolver la lista de productos
    }

    public static void SerializarProductos(List<Producto> productos, string rutaArchivo)
    {
        // Abrir el archivo para escribir
        using StreamWriter writer = new(rutaArchivo + ".csv");

        // Escribir el encabezado en el archivo
        writer.WriteLine("Handle,Variant Inventory Qty,Variant Price,Variant Image");

        // Escribir cada producto en una línea del archivo
        foreach (Producto producto in productos)
        {
            string nombre = producto.Nombre.Replace(",", "");    // Reemplazar las comas en el nombre del producto
            int inventario = producto.Inventario;
            decimal coste = producto.Coste;
            string imageUrl = producto.ImageUrl;

            // Crear una línea con los valores del producto separados por comas
            string line = $"{nombre},{inventario},{coste},{imageUrl}";

            // Escribir la línea en el archivo
            writer.WriteLine(line);
        }
    }


    public static void SerializarPedidos(List<Pedido> pedidos, string rutaArchivo)
    {
        // Configuración del CSV
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",                // Delimitador de campos
            HasHeaderRecord = true          // Indica si el archivo tiene una línea de encabezado
        };

        // Abrir el archivo para escribir
        using (var writer = new StreamWriter(rutaArchivo))
        using (var csv = new CsvWriter(writer, csvConfig))
        {
            // Escribir los encabezados en la primera línea del archivo
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

            // Iterar sobre cada pedido
            foreach (var pedido in pedidos)
            {
                // Verificar si el pedido tiene líneas de pedido
                if (pedido.LineasPedido != null && pedido.LineasPedido.Count > 0)
                {
                    bool isFirstLine = true;

                    // Iterar sobre cada línea de pedido del pedido actual
                    foreach (var lineaPedido in pedido.LineasPedido)
                    {
                        // Escribir los campos del pedido en la línea actual
                        csv.WriteField(pedido.Nombre);
                        csv.WriteField(pedido.Email);
                        csv.WriteField(isFirstLine ? pedido.Estado : "");
                        csv.WriteField(isFirstLine ? pedido.EstadoDeEnvio : "");
                        csv.WriteField(isFirstLine ? pedido.Divisa : "");
                        csv.WriteField(isFirstLine ? pedido.Total.ToString(CultureInfo.InvariantCulture) : "");

                        // Escribir los campos específicos de la línea de pedido
                        csv.WriteField(lineaPedido.Nombre);
                        csv.WriteField(lineaPedido.Precio.ToString(CultureInfo.InvariantCulture));
                        csv.WriteField(lineaPedido.Cantidad);
                        csv.WriteField(isFirstLine ? pedido.Cliente.Nombre : "");
                        csv.WriteField(isFirstLine ? pedido.Cliente.Direccion : "");
                        csv.WriteField(isFirstLine ? pedido.Cliente.Ciudad : "");

                        csv.NextRecord();

                        isFirstLine = false;
                    }
                }

                // Registrar la exportación en la tabla Auditorias
                var auditoria = new Auditoria
                {
                    Fecha = DateTime.Now,
                    NombrePedido = pedido.Nombre
                };

                AuditoriaRepositorio.AgregarAuditoria(auditoria);
            }
        }
    }


    public static List<Pedido> DeserializarPedidos(string rutaArchivo)
    {
        List<Pedido> pedidos = new List<Pedido>();

        // Configuración del CSV
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",                // Delimitador de campos
            HasHeaderRecord = true,         // Indica si el archivo tiene una línea de encabezado
            MissingFieldFound = null        // Configuración para campos faltantes
        };

        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvReader(reader, csvConfig))
        {
            // Leer y validar el encabezado del archivo
            csv.Read();
            csv.ReadHeader();

            if (csv.HeaderRecord.Contains("Name") && csv.HeaderRecord.Contains("Email") &&
                csv.HeaderRecord.Contains("Financial Status") && csv.HeaderRecord.Contains("Fulfillment Status") &&
                csv.HeaderRecord.Contains("Currency") && csv.HeaderRecord.Contains("Total") &&
                csv.HeaderRecord.Contains("Lineitem name") && csv.HeaderRecord.Contains("Lineitem price") &&
                csv.HeaderRecord.Contains("Lineitem quantity") && csv.HeaderRecord.Contains("Shipping Name") &&
                csv.HeaderRecord.Contains("Shipping Street") && csv.HeaderRecord.Contains("Shipping Country"))
            {
                Pedido pedidoActual = null;

                // Leer y procesar cada línea del archivo
                while (csv.Read())
                {
                    string nombreActual = csv.GetField<string>("Name");

                    if (pedidoActual == null || pedidoActual.Nombre != nombreActual)
                    {
                        // Crear un nuevo pedido
                        if (pedidoActual != null)
                        {
                            pedidos.Add(pedidoActual);
                        }

                        // Obtener los valores del pedido y crear un nuevo objeto Pedido
                        decimal total;
                        string totalStr = csv.GetField<string>("Total");

                        if (!string.IsNullOrEmpty(totalStr) && decimal.TryParse(totalStr.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out total))
                        {
                            pedidoActual = new Pedido
                            {
                                Nombre = nombreActual,
                                Email = csv.GetField<string>("Email"),
                                Estado = csv.GetField<string>("Financial Status"),
                                EstadoDeEnvio = csv.GetField<string>("Fulfillment Status"),
                                Divisa = csv.GetField<string>("Currency"),
                                Total = total,
                                LineasPedido = new List<LineaPedido>(),
                                Auditoria = new Auditoria
                                {
                                    Fecha = DateTime.Now,
                                    NombrePedido = nombreActual
                                },
                                Cliente = new Cliente
                                {
                                    Nombre = csv.GetField<string>("Shipping Name"),
                                    Direccion = csv.GetField<string>("Shipping Street"),
                                    Ciudad = csv.GetField<string>("Shipping Country")
                                }
                            };

                            pedidos.Add(pedidoActual);
                        }
                        else
                        {
                            Console.WriteLine($"Error: No se pudo convertir el valor '{totalStr}' a decimal para el campo 'Total'");
                        }
                    }

                    // Leer los valores de la línea y agregarlos al pedido actual
                    string lineaNombre = csv.GetField<string>("Lineitem name");
                    decimal lineaPrecio = csv.GetField<decimal>("Lineitem price");
                    int lineaCantidad = csv.GetField<int>("Lineitem quantity");

                    LineaPedido lineaPedido = new LineaPedido
                    {
                        Nombre = lineaNombre,
                        Precio = lineaPrecio,
                        Cantidad = lineaCantidad
                    };

                    pedidoActual.LineasPedido.Add(lineaPedido);
                }

                // Agregar el último pedido actual a la lista de pedidos
                if (pedidoActual != null)
                {
                    pedidos.Add(pedidoActual);
                }
            }
            else
            {
                Console.WriteLine("Error: La cabecera del archivo CSV no es válida");
            }
        }
        return pedidos; //Devuelve la lista creada
    }

}




