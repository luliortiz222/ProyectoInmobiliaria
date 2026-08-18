using System;

public class Propietario
{
    public int IdPropietario { get; set; }
    public string Dni { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }



    public Propietario(int idPropietario, string dni, string nombre, string apellido, string telefono, string email)
    {
        IdPropietario = idPropietario;
        Dni = dni;
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        Email = email;
    }
    public Propietario() { 
    }
}
