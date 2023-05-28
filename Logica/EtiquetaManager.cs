using SkiaSharp;

namespace SugahriStore.Logica
{
    public class EtiquetaManager
    {
        public void CrearEtiqueta()
        {
            string rutaProductos = "C:\\Users\\Domi\\source\\repos\\SugahriStore\\Resources\\Images\\fondo2.jpg";
            SKBitmap imagenFondo = SKBitmap.Decode(rutaProductos);

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

            // Dibujar la información del pedido
            string pedido = "Número de pedido:\n";
            string numeroPedido = "12345";
            string fecha = "Fecha: 2023-05-28";

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
            canvas.DrawText(pedido, textX, pedidoY, pedidoPaint);

            // Calcular la posición vertical para el número de pedido en una nueva línea
            numeroPedidoY = pedidoY + pedidoPaint.TextSize;

            canvas.DrawText(numeroPedido, textX, numeroPedidoY, numeroPedidoPaint);

            // Calcular la posición vertical para la línea de fecha en una nueva línea
            fechaY = numeroPedidoY + numeroPedidoPaint.TextSize;

            canvas.DrawText(fecha, textX, fechaY, fechaPaint);

            // Exportar el lienzo como un archivo JPG en la ruta C:/Database
            string rutaArchivo = @"C:/databases/etiqueta.jpg";

            using (var image = surface.Snapshot())
            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
            using (var stream = File.OpenWrite(rutaArchivo))
            {
                data.SaveTo(stream);
            }


        }
    }
}

