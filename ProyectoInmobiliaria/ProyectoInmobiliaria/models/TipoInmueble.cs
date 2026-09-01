using System;
namespace ProyectoInmobiliaria.models
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
