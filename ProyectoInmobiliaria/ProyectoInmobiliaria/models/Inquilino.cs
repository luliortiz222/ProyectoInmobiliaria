using System;

public class Inquilino
{
    public int IdInquilino{ get; set; }
    public string Dni { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }

    public Inquilino (int idInquilino, string dni, string nombre, string apellido, string telefono, string email)
    {
        IdInquilino = idInquilino;
        Dni = dni;
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        Email = email;
    }

}
