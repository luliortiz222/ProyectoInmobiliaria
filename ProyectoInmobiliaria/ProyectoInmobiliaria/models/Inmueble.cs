using System;

namespace ProyectoInmoviliaria.Models
{
	public class Inmueble	
	{
		public int IdInmueble { get; set; }
		public string direccion { get; set; }
		public int cantAmbientes { get; set; }
		public double superficie { get; set; }
		public double precipPorDia { get; set; }
		public char imagenPortada { get; set; }
		public int idPropietario { get; set; }
		public int IdTipo { get; set; }

        public Inmueble(int IdInmueble, string direccion, int cantAmbientes, double superficie, double precipPorDia, char imagenPortada, int idPropietario, int idTipo)
		{
			this.IdInmueble = IdInmueble;
			this.direccion = direccion;
			this.cantAmbientes = cantAmbientes;
			this.superficie = superficie;
			this.precipPorDia = precipPorDia;
			this.imagenPortada = imagenPortada;
			this.idPropietario = idPropietario;
			this.IdTipo= idTipo; 
        }
        public Inmueble() {
        }

		public override string ToString()
        {
            return $"Inmueble [IdInmueble={IdInmueble}, direccion={direccion}, cantAmbientes={cantAmbientes}, superficie={superficie}, precipPorDia={precipPorDia}, imagenPortada={imagenPortada}, idPropietario={idPropietario}], IdTipo={IdTipo}]";
        }
    }
