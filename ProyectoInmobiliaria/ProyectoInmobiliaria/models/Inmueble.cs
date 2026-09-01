using System;

namespace ProyectoInmobiliaria.models
{
    public class Inmueble
    {
        public int IdInmueble { get; set; }
        public string Direccion { get; set; }
        public int CantidadAmbientes { get; set; }
        public decimal Superficie { get; set; }
        public decimal PrecioPorDia { get; set; }
        public string ImagenPortada { get; set; }
        public int IdPropietario { get; set; }
        public int IdTipoInmueble { get; set; }

        public Inmueble()
        {
        }

        public Inmueble(
            int idInmueble,
            string direccion,
            int cantidadAmbientes,
            decimal superficie,
            decimal precioPorDia,
            string imagenPortada,
            int idPropietario,
            int idTipoInmueble)
        {
            IdInmueble = idInmueble;
            Direccion = direccion;
            CantidadAmbientes = cantidadAmbientes;
            Superficie = superficie;
            PrecioPorDia = precioPorDia;
            ImagenPortada = imagenPortada;
            IdPropietario = idPropietario;
            IdTipoInmueble = idTipoInmueble;
        }

        public override string ToString()
        {
            return $"Inmueble [IdInmueble={IdInmueble}, Direccion={Direccion}, " + $"CantidadAmbientes={CantidadAmbientes}, Superficie={Superficie}, " + $"PrecioPorDia={PrecioPorDia}, ImagenPortada={ImagenPortada}, " + $"IdPropietario={IdPropietario}, IdTipoInmueble={IdTipoInmueble}]";
        }
    }

}