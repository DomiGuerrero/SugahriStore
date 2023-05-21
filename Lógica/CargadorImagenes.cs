using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugahriStore.Lógica
{
    public static class CargadorImagenes
    {
        public static Image LoadImage(string imageUrl)
        {
            // Aquí se puede implementar la lógica para cargar la imagen desde la URL
            // En este ejemplo, simplemente se devuelve una imagen en blanco como marcador de posición
            return new Image()
            {
                Source = "https://via.placeholder.com/150",
                Aspect = Aspect.AspectFit
            };
        }
    }
}
