public interface IProductoRepository
{
    bool CrearProducto(Producto p);
    Producto ModificarProducto(Producto p , int id);
    List<Producto> ListarProductos();
    void EliminarProducto(int id);

}