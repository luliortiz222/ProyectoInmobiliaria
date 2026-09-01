using System;
namespace ProyectoInmoviliaria.Models
{
	public class TipoInmueble
	{
		public int IdTipo { get; set; }
		public string nombre { get; set; }

		public TipoInmueble(int idTipo, string nombre)
		{
			IdTipo = idTipo;
			nombre = nombre;
		}
		public TipoInmueble() { 
		
		}
    }
}
