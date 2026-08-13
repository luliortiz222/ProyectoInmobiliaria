using System;

public class Inquilino
{
    public int Id{ get; set; }
    public string Dni { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }

    public Inquilino (int id, string dni, string nombre, string apellido, string telefono, string email)
    {
        Id = id;
        Dni = dni;
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        Email = email;
    }

}
