using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Proyecto1.Services.Interfaces;

namespace Proyecto1.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IServicioCliente servicioCliente;
        private const string Modulo = "Cliente";

        public ClienteController(IServicioCliente servicioCliente)
        {
            this.servicioCliente = servicioCliente;
        }

        public async Task<IActionResult> Index(string filtro)
        {
            var clientes = await servicioCliente.ObtenerTodosAsync(filtro);
            ViewBag.Filtro = filtro;
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            cliente.Identificacion = cliente.Identificacion?.Trim() ?? string.Empty;

            if (!ModelState.IsValid)
            {
                TempData[$"Mensaje{Modulo}XError"] = "⚠️ Verificá los campos requeridos.";
                return View(cliente);
            }

            // 🔁 Validación de cédula existente
            var clientes = await servicioCliente.ObtenerTodosAsync();
            bool cedulaDuplicada = clientes.Any(c =>
                c.Identificacion.Equals(cliente.Identificacion, StringComparison.OrdinalIgnoreCase));

            if (cedulaDuplicada)
            {
                TempData[$"Mensaje{Modulo}XError"] = "❌ Ya existe un cliente con esta cédula.";
                return View(cliente);
            }

            var resultado = await servicioCliente.AgregarAsync(cliente);

            if (resultado.Exito)
            {
                TempData[$"Mensaje{Modulo}XExito"] = $"✅ {Modulo} registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData[$"Mensaje{Modulo}XError"] = resultado.Mensaje ?? $"❌ No se pudo registrar el {Modulo.ToLower()}.";
                return View(cliente);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await servicioCliente.ObtenerPorIdAsync(id);

            if (cliente is null)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ {Modulo} no encontrado para edición.";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest("❌ El ID proporcionado no coincide con el cliente.");

            cliente.Identificacion = cliente.Identificacion?.Trim() ?? string.Empty;

            if (!ModelState.IsValid)
            {
                TempData[$"Mensaje{Modulo}XError"] = "⚠️ Verificá los campos requeridos.";
                return View(cliente);
            }

            // 🔁 Validación de cédula duplicada en edición
            var clientes = await servicioCliente.ObtenerTodosAsync();
            bool cedulaDuplicada = clientes.Any(c =>
                c.Id != cliente.Id &&
                c.Identificacion.Equals(cliente.Identificacion, StringComparison.OrdinalIgnoreCase));

            if (cedulaDuplicada)
            {
                TempData[$"Mensaje{Modulo}XError"] = "❌ Ya existe otro cliente con esta cédula.";
                return View(cliente);
            }

            var resultado = await servicioCliente.ActualizarAsync(id, cliente);

            if (resultado.Exito)
            {
                TempData[$"Mensaje{Modulo}XExito"] = $"✅ {Modulo} actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData[$"Mensaje{Modulo}XError"] = resultado.Mensaje ?? $"❌ No se pudo actualizar el {Modulo.ToLower()}.";
                return View(cliente);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await servicioCliente.ObtenerPorIdAsync(id);

            if (cliente is null)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ {Modulo} no encontrado para eliminación.";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var resultado = await servicioCliente.EliminarAsync(id);

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
            var cliente = await servicioCliente.ObtenerPorIdAsync(id);

            if (cliente is null)
            {
                TempData[$"Mensaje{Modulo}XError"] = $"❌ {Modulo} no encontrado para ver detalles.";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }
    }
}
