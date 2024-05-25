using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discos_app
{
    internal class Disco
    {
        public string Titulo { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public int CantidadCanciones { get; set; }
        public string UrlImagen { get; set; }
        public Estilo Genero { get; set; }
        public Edicion TipoEdicion { get; set; }
    }
}
