using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototipo_1_SugahriStore.Lógica
{
    public static class LogicaViews
    {
        public static async void StartTimer(CarouselView carouselView, List<string> Images)
        {
            while (true)
            {
                // Esperar 5 segundos antes de cambiar la imagen
                await Task.Delay(5000);

                // Obtener el índice actual de la imagen
                var index = carouselView.Position;

                // Cambiar al siguiente índice (o volver al inicio si es la última imagen)
                index = (index + 1) % Images.Count;

                // Cambiar la imagen actual
                carouselView.Position = index;
            }
        }
    }
}
