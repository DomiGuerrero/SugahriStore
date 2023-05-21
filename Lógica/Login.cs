using Prototipo_1_SugahriStore.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototipo_1_SugahriStore.Lógica
{
    public class Login
    {
        public bool LoginUser(string nombreUsuario, string contraseña, Usuario usuario)
        {
            if (usuario.Nombre == nombreUsuario && usuario.Contraseña == contraseña)
                return true;
            else
                return false;
        }
    }
}
