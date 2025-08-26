using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Proyecto1.Services.Interfaces;

namespace Proyecto1.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly IServicioEmpleado servicioEmpleado;
        private const string Modulo = "Empleado";

        public EmpleadoController(IServicioEmpleado servicioEmpleado)
        {
            this.servicioEmpleado = servicioEmpleado;
        }

        public async Task<IActionResult> Index(string filtro)
        {
            var empleados = await servicioEmpleado.ObtenerTodosAsync(filtro);
            ViewBag.Filtro = filtro;
            return View(empleados);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            empleado.Identificacion = empleado.Identificacion?.Trim() ?? string.Empty;

            if (!ModelState.IsValid)
            {
                TempData[$"Mensaje{Modulo}XError"] = "⚠️ Verificá los campos requeridos.";
                return View(empleado);
            }

            var empleados = await servicioEmpleado.ObtenerTodosAsync();
            bool cedulaDuplicada = empleados.Any(e =>
                e.Identificacion.Equals(empleado.Identificacion, StringComparison.OrdinalIgnoreCase));

            if (cedulaDuplicada)
            {
                TempData[$"Mensaje{Modulo}XError"] = "❌ Ya existe un empleado con esta cédula.";
                return View(empleado);
            }

            var resultado = await servicioEmpleado.AgregarAsync(empleado);

            if (resultado.Exito)
            {
                TempData[$"Mensaje{Modulo}XExito"] = $"✅ {Modulo} registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData[$"Mensaje{Modulo}XError"] = resultado.Mensaje ?? $"❌ No se pudo registrar el {Modulo.ToLower()}.";
                return View(empleado);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await servicioEmpleado.ObtenerPorIdAsync(id);

            if (empleado is null)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ {Modulo} no encontrado para edición.";
                return RedirectToAction(nameof(Index));
            }

            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id)
                return BadRequest("❌ El ID proporcionado no coincide con el empleado.");

            empleado.Identificacion = empleado.Identificacion?.Trim() ?? string.Empty;

            if (!ModelState.IsValid)
            {
                TempData[$"Mensaje{Modulo}XError"] = "⚠️ Verificá los campos requeridos.";
                return View(empleado);
            }

            var empleados = await servicioEmpleado.ObtenerTodosAsync();
            bool cedulaDuplicada = empleados.Any(e =>
                e.Id != empleado.Id &&
                e.Identificacion.Equals(empleado.Identificacion, StringComparison.OrdinalIgnoreCase));

            if (cedulaDuplicada)
            {
                TempData[$"Mensaje{Modulo}XError"] = "❌ Ya existe otro empleado con esta cédula.";
                return View(empleado);
            }

            var resultado = await servicioEmpleado.ActualizarAsync(id, empleado);

            if (resultado.Exito)
            {
                TempData[$"Mensaje{Modulo}XExito"] = $"✅ {Modulo} actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData[$"Mensaje{Modulo}XError"] = resultado.Mensaje ?? $"❌ No se pudo actualizar el {Modulo.ToLower()}.";
                return View(empleado);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await servicioEmpleado.ObtenerPorIdAsync(id);

            if (empleado is null)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ {Modulo} no encontrado para eliminación.";
                return RedirectToAction(nameof(Index));
            }

            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var resultado = await servicioEmpleado.EliminarAsync(id);

                if (resultado.Exito)
                {
                    TempData[$"Mensaje{Modulo}XExito"] = $"🗑️ {Modulo} eliminado correctamente.";
                }
                else
                {
                    TempData[$"Mensaje{Modulo}XError"] = resultado.Mensaje ?? $"❌ No se pudo eliminar el {Modulo.ToLower()}.";
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ El {Modulo.ToLower()} no se puede eliminar porque está referenciado por otros registros.";
            }
            catch (Exception)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ Error inesperado al intentar eliminar el {Modulo.ToLower()}.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var empleado = await servicioEmpleado.ObtenerPorIdAsync(id);

            if (empleado is null)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ {Modulo} no encontrado para ver detalles.";
                return RedirectToAction(nameof(Index));
            }

            return View(empleado);
        }
    }
}
