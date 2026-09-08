using System;
namespace ProyectoInmobiliaria.models
{
    public class Pago
    {
        public int idPago { get; set; }
        public int idReserva { get; set; }
        public string concepto { get; set; }
        public DateTime fechaPago { get; set; }
        public decimal importe { get; set; }
        public bool estado { get; set; }

        public int idUsuarioCreador { get; set; }
        public int? idUsuarioAnulador { get; set; }

        public Reserva Reserva { get; set; }

        
        public Pago()
        {
        }
    }
}

