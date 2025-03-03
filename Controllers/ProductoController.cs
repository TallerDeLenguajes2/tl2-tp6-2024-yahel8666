using Microsoft.AspNetCore.Mvc;
public class ProductoController : Controller
{
    ILogger<ProductoController> _logger;

    ProductosRepository _productoRepository;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;
        _productoRepository = new ProductosRepository();
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View(_productoRepository.ObtenerProductos());
    }

    // carga los datos para actualizar
    [HttpGet]
    public IActionResult CargarDatos(int id)
    {
        var producto = _productoRepository.ObtenerProductoPorId(id);
        if (producto is not null) return View(producto);
        else return NotFound();
    }
    [HttpPost]

    public IActionResult ModificarProducto(Producto p)
    {
        _productoRepository.ModificarProducto(p);
        return RedirectToAction("Index");
    }
    // muestra los datos del producto antes de eliminarlo
    [HttpGet]
    public IActionResult VerificarEliminacion(int id)
    {
        var producto = _productoRepository.ObtenerProductoPorId(id);
        if (producto is not null) return View(producto);
        else return NotFound();
    }
    [HttpPost]
    public IActionResult EliminarProducto(int id)
    {
        _productoRepository.EliminarProductoPorId(id);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult CrearProducto()
    {
        // Solo retorna la vista para mostrar el formulario vacío
        return View();
    }

    [HttpPost]
    public IActionResult CrearProducto(Producto producto)
    {
        if (ModelState.IsValid)
        {
            _productoRepository.CrearProducto(producto);
            return RedirectToAction("Index");
        }
        return View(producto);
    }

}