
public class Presupuesto
{
    int idPresupuesto;
    Cliente cliente; 
    DateTime fechaCreacion;
    List<PresupuestoDetalle> detalle;

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
    public Cliente Cliente { get => cliente; set => cliente = value; }

    public Presupuesto()
    {
        detalle = new List<PresupuestoDetalle>();
    }

    public Presupuesto(int idPresupuesto, Cliente cliente, DateTime fecha)
    {
        this.idPresupuesto = idPresupuesto;
        this.Cliente = cliente; 
        FechaCreacion = fecha;
        detalle = new List<PresupuestoDetalle>();
    }
    public Presupuesto(CrearPresupuestoVM p)
    {
        cliente = new Cliente();
        cliente.ClienteId = p.IdCliente;
        FechaCreacion = p.FechaCreacion;
    }
    public Presupuesto(ModificarPresupuestoVM p)
    {
        cliente = new Cliente();
        idPresupuesto = p.IdPresupuesto;
        cliente.ClienteId = p.IdCliente;
        fechaCreacion = p.FechaCreacion;
    }

    public double MontoPresupuesto()
    {
        double monto = detalle.Sum(d => d.Cantidad*d.Producto.Precio);
        return monto;
    }
    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto()*1.21;
    }
    public int CantidadProductos ()
    {
        return detalle.Sum(d => d.Cantidad);
    }
}