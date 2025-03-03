using System.ComponentModel.DataAnnotations; 
public class CrearPresupuestoVM
{
    int idCliente;

    DateTime fechaCreacion;

    public CrearPresupuestoVM()
    {}
    
    [Required(ErrorMessage = "El cliente es obligatorio.")]
    public int IdCliente { get => idCliente; set => idCliente = value; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
}