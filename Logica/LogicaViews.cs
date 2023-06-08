using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugahriStore.Lógica
{
    public static class LogicaViews
    {
        private static CancellationTokenSource cancellationTokenSource;

        public static async Task StartTimer(CarouselView carouselView, List<string> Images)
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                while (true)
                {
                    // Esperar 5 segundos antes de cambiar la imagen
                    await Task.Delay(5000, cancellationTokenSource.Token);

                    // Obtener el índice actual de la imagen
                    var index = carouselView.Position;

                    // Cambiar al siguiente índice (o volver al inicio si es la última imagen)
                    index = (index + 1) % Images.Count;

                    // Cambiar la imagen actual
                    carouselView.Position = index;
                }
            }
            catch (TaskCanceledException)
            {
                // La tarea se canceló, no es necesario manejarla
            }
        }

        public static void StopTimer()
        {
            cancellationTokenSource?.Cancel();
        }
    }

}
