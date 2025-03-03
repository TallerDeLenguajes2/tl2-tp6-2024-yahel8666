using Microsoft.AspNetCore.Mvc;
public class ProductoController : Controller
{

    private readonly ILogger<ProductoController> _logger;

    private IProductosRepository _productosRepository;

    public ProductoController(ILogger<ProductoController> logger, IProductosRepository productosRepository)
    {
        _logger = logger;
        _productosRepository = productosRepository;
    }
    public IActionResult Index()
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
            return View(_productosRepository.ObtenerProductos());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult AltaProducto()
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario de alta de producto.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult CrearProducto(AltaProductoViewModel productoVM)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var producto = new Producto(productoVM);
            _productosRepository.CrearProducto(producto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            var producto = _productosRepository.ObtenerProductoPorId(id);
            var productoVM = new ModificarProductoViewModel(producto);
            return View(productoVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario de modificación del producto.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult ModificarProducto(ModificarProductoViewModel productoVM)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var producto = new Producto(productoVM);
            _productosRepository.ModificarProducto(producto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo modificar el producto.";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            return View(_productosRepository.ObtenerProductoPorId(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el producto para eliminar.";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult EliminarProductoPorId(int id)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Index", "Login");

            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            _productosRepository.EliminarProductoPorId(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("AccessLevel") == "Admin";
    }

    private bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("User"));
    }
}