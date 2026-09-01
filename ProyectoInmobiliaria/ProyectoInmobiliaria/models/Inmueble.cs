using System;

namespace ProyectoInmoviliaria.Models
{
	public class Inmueble	
	{
		public int idInmueble { get; set; }
		public string direccion { get; set; }
		public int cantAmbientes { get; set; }
		public double superficie { get; set; }
		public double precipPorDia { get; set; }
		public char imagenPortada { get; set; }
		public int idPropietario { get; set; }
		public int idInmueble { get; set; }

        public Inmueble(int idInmueble, string direccion, int cantAmbientes, double superficie, double precipPorDia, char imagenPortada, int idPropietario)
		{
			this.idInmueble = idInmueble;
			this.direccion = direccion;
			this.cantAmbientes = cantAmbientes;
			this.superficie = superficie;
			this.precipPorDia = precipPorDia;
			this.imagenPortada = imagenPortada;
			this.idPropietario = idPropietario;
		}
        public Inmueble()
        {
        }

		public override string ToString()
        {
            return $"Inmueble [idInmueble={idInmueble}, direccion={direccion}, cantAmbientes={cantAmbientes}, superficie={superficie}, precipPorDia={precipPorDia}, imagenPortada={imagenPortada}, idPropietario={idPropietario}]";
        }
    }
