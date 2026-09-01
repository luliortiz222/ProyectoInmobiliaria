using System;

namespace ProyectoInmobiliaria.models
{
    public class Inmueble
    {
        public int IdInmueble { get; set; }
        public string Direccion { get; set; }
        public int Cupo { get; set; }
        public string Coordenadas { get; set; }
        public decimal PrecioPorDia { get; set; }
        public string ImagenPortada { get; set; }
        public bool Estado { get; set; }
        public int IdPropietario { get; set; }
        public int IdTipoInmueble { get; set; }

        public Inmueble()
        {
        }

        public Inmueble(
            int idInmueble,
            string direccion,
            int cupo,
            string coordenadas,
            decimal precioPorDia,
            string imagenPortada,
            bool estado,
            int idPropietario,
            int idTipoInmueble)
        {
            IdInmueble = idInmueble;
            Direccion = direccion;
            Cupo = cupo;
            Coordenadas = coordenadas;
            PrecioPorDia = precioPorDia;
            ImagenPortada = imagenPortada;
            Estado = estado;
            IdPropietario = idPropietario;
            IdTipoInmueble = idTipoInmueble;
        }

        public override string ToString()
        {
            return $"Inmueble [IdInmueble={IdInmueble}, Direccion={Direccion}, " + $"Cupo={Cupo}, Coordenadas={Coordenadas}, " + $"PrecioPorDia={PrecioPorDia},ImagenPortada={ImagenPortada}, " + $"Estado={Estado}, IdPropietario={IdPropietario}, " + $"IdTipoInmueble={IdTipoInmueble}]";
        }
    }

}