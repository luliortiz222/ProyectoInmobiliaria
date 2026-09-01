using System;
namespace ProyectoInmoviliaria.models
{
    public class TipoInmueble
    {
        public int IdTipoInmueble { get; set; }
        public string Nombre { get; set; }

        public TipoInmueble()
        {
        }
        public TipoInmueble(int idTipoInmueble, string nombre)
        {
            IdTipoInmueble = idTipoInmueble;
            Nombre = nombre;
        }
    }
}
