using SkiaSharp;
using SugahriStore.Datos;
using SugahriStore.Modelos;

namespace SugahriStore.Logica
{
    public static class EtiquetaManager
    {
        private static readonly ClienteRepositorio ClienteRepositorio = new ClienteRepositorio();

        public static async Task CrearEtiqueta(string RutaArchivo, Pedido Pedido)
        {
            string rutaImagenFondo = Path.Combine(Path.Combine(AppContext.BaseDirectory, "Resources", "ImagesView\\fondoEtiquetas.jpg"));
            SKBitmap imagenFondo = SKBitmap.Decode(rutaImagenFondo);

            // Crear un nuevo lienzo SkiaSharp con las dimensiones de la imagen de fondo
            var surface = SKSurface.Create(new SKImageInfo(imagenFondo.Width, imagenFondo.Height));

            // Obtener el contexto de dibujo
            var canvas = surface.Canvas;

            // Dibujar la imagen de fondo
            canvas.DrawBitmap(imagenFondo, new SKRect(0, 0, imagenFondo.Width, imagenFondo.Height));

            // Dibujar la información del pedido
            float textX = imagenFondo.Width / 2; // Centrar el texto horizontalmente
            float textY = imagenFondo.Height / 3.7f; // Centrar el texto verticalmente

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
            string nombre = Pedido.Cliente.Nombre;
            string direccion = Pedido.Cliente.Direccion;
            string Pais = Pedido.Cliente.Ciudad;

            // Estilo del texto
            SKTypeface typeface = SKTypeface.FromFile(Path.Combine(AppContext.BaseDirectory, "Resources/Fonts/QuicksandMedium500.ttf"));

            SKPaint pedidoPaint = new SKPaint
            {
                Color = new SKColor(220, 20, 60),
                TextSize = GetScaledTextSize(imagenFondo, 0.1f),
                TextAlign = SKTextAlign.Center,
                Typeface = typeface,
                IsAntialias = true
            };

            SKPaint direccionPaint = new SKPaint
            {
                Color = new SKColor(220, 20, 60),
                TextSize = GetScaledTextSize(imagenFondo, 0.1f),
                TextAlign = SKTextAlign.Center,
                Typeface = typeface,
                IsAntialias = true
            };

            SKPaint paisPaint = new SKPaint
            {
                Color = new SKColor(220, 20, 60),
                TextSize = GetScaledTextSize(imagenFondo, 0.1f),
                TextAlign = SKTextAlign.Center,
                Typeface = typeface,
                IsAntialias = true
            };


            // Calcular posiciones verticales de las líneas de texto
            float pedidoY = textY - 75 + GetScaledTextOffset(imagenFondo, 0.1f);
            float direccionPedidoY = pedidoY + pedidoPaint.TextSize + GetScaledTextOffset(imagenFondo, 0.02f);
            float paisY = direccionPedidoY + direccionPaint.TextSize + GetScaledTextOffset(imagenFondo, 0.1f);

            canvas.DrawText(nombre, textX, pedidoY, pedidoPaint);

            canvas.DrawText(direccion, textX, direccionPedidoY, direccionPaint);

            canvas.DrawText(Pais, textX, paisY, paisPaint);

            // Exportar el lienzo como un archivo JPG 
            string rutaArchivo = RutaArchivo;

            using (var image = surface.Snapshot())
            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
            using (var stream = File.OpenWrite(rutaArchivo + "\\etiqueta_" + Pedido.Cliente.Nombre + ".jpg"))
            {
                data.SaveTo(stream);
            }

        }
        // Función para obtener el desplazamiento de texto escalado en base al tamaño de la imagen de fondo
        private static float GetScaledTextOffset(SKBitmap background, float scaleFactor)
        {
            float maxOffset = Math.Min(background.Width, background.Height) * scaleFactor;
            return maxOffset;
        }
        // Función para obtener el tamaño de texto escalado en base al tamaño de la imagen de fondo
        private static float GetScaledTextSize(SKBitmap background, float scaleFactor)
        {
            float maxTextSize = Math.Min(background.Width, background.Height) * scaleFactor;
            return maxTextSize;
        }


    }
}

