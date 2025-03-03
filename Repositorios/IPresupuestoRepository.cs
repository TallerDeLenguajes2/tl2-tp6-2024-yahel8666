/*Los metodos declarados en una interfaz 
siempre son implicitamente publicos*/

public interface IPresupuestoRepository
{
    void Create(Presupuesto p); 
    List<Presupuesto> ListarPresupuestos(); 
    Presupuesto GetPresupuestoById(int id); 
    bool EliminarPresupuesto(int id);

    bool AgregarProducto(int id); 
}; 