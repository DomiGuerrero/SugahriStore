using SkiaSharp;
using SugahriStore.Datos;
using SugahriStore.Modelos;

namespace SugahriStore.Logica
{
    public static class EtiquetaManager
    {
        private static readonly ClienteRepositorio ClienteRepositorio = new ClienteRepositorio();
        public static async void CrearEtiqueta(string RutaArchivo, string NombreArchivo, Pedido Pedido)
        {
            string rutaImagenFondo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images\\fondo2.jpg");
            SKBitmap imagenFondo = SKBitmap.Decode(rutaImagenFondo);

            // Crear un nuevo lienzo SkiaSharp con las dimensiones de la imagen de fondo
            var surface = SKSurface.Create(new SKImageInfo(imagenFondo.Width, imagenFondo.Height));

            // Obtener el contexto de dibujo
            var canvas = surface.Canvas;

            // Dibujar la imagen de fondo
            canvas.DrawBitmap(imagenFondo, new SKRect(0, 0, imagenFondo.Width, imagenFondo.Height));

            // Dibujar la información del pedido
            float textX = imagenFondo.Width / 2; // Centrar el texto horizontalmente
            float textY = imagenFondo.Height / 2; // Centrar el texto verticalmente

            // Configurar el estilo del borde redondeado
            SKPaint borderPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White,
                StrokeWidth = 5
            };
            SKRect labelRect = new SKRect(textX - 200, textY - 75, textX + 200, textY + 75); // Ajustar el tamaño del rectángulo

            float cornerRadius = 10;

            canvas.DrawRoundRect(labelRect, cornerRadius, cornerRadius, borderPaint);

            Pedido.Cliente = ClienteRepositorio.ObtenerClientePorPedido(Pedido);

            // Dibujar la información del pedido
            string nombre = Pedido.Cliente.Nombre + "\n";
            string direccion = Pedido.Cliente.Direccion + "\n";
            string Pais = Pedido.Cliente.Ciudad;

            // Estilo del texto
            SKTypeface typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            SKPaint pedidoPaint = new SKPaint
            {
                Color = new SKColor(0, 0, 128), // Color azul marino
                TextSize = 480,
                TextAlign = SKTextAlign.Center,
                Typeface = typeface,
                IsAntialias = true
            };

            SKPaint numeroPedidoPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 720,
                TextAlign = SKTextAlign.Center,
                Typeface = typeface,
                IsAntialias = true
            };

            SKPaint fechaPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 480,
                TextAlign = SKTextAlign.Center,
                Typeface = typeface,
                IsAntialias = true
            };

            // Calcular posiciones verticales de las líneas de texto
            float pedidoY = textY - 75 + 60; // Posición vertical para la línea "Número de pedido:"
            float numeroPedidoY = textY - 75 + 160; // Posición vertical para el número de pedido
            float fechaY = textY + 75 - 60; // Posición vertical para la línea de fecha

            // Dibujar texto en lienzo
            canvas.DrawText(nombre, textX, pedidoY, pedidoPaint);

            // Calcular la posición vertical para el número de pedido en una nueva línea
            numeroPedidoY = pedidoY + pedidoPaint.TextSize;

            canvas.DrawText(direccion, textX, numeroPedidoY, numeroPedidoPaint);

            // Calcular la posición vertical para la línea de fecha en una nueva línea
            fechaY = numeroPedidoY + numeroPedidoPaint.TextSize;

            canvas.DrawText(Pais, textX, fechaY, fechaPaint);

            // Exportar el lienzo como un archivo JPG 
            string rutaArchivo = RutaArchivo;

            using (var image = surface.Snapshot())
            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
            using (var stream = File.OpenWrite(rutaArchivo + "\\" + NombreArchivo + ".jpg"))
            {
                data.SaveTo(stream);
            }


        }
    }
}

