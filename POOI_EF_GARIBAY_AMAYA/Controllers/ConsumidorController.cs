using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using POOI_EF_GARIBAY_AMAYA.Models;

namespace POOI_EF_GARIBAY_AMAYA.Controllers
{
    public class ConsumidorController : Controller
    {
        string cadenaConexion = ConfigurationManager.ConnectionStrings["cadenaConexion"].ConnectionString;

        IEnumerable<Consumidor> listarConsumidores()
        {
            List<Consumidor> listaConsumidores = new List<Consumidor>();
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("exec usp_listar_consumidor", sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listaConsumidores.Add(new Consumidor(
                    sqlDataReader.GetString(0),
                    sqlDataReader.GetString(1),
                    sqlDataReader.GetString(2),
                    sqlDataReader.GetString(3),
                    sqlDataReader.GetString(4)
                    )
                    );
            }
            sqlDataReader.Close();
            sqlConnection.Close();
            return listaConsumidores;
        }

        IEnumerable<Pais> listarPaises()
        {
            List<Pais> listaPaises = new List<Pais>();
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("select * from tb_paises ", sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listaPaises.Add(new Pais(
                    sqlDataReader.GetString(0)
                    )
                    );
            }
            sqlDataReader.Close();
            sqlConnection.Close();
            return listaPaises;
        }

        string guardarConsumidor(Consumidor consumidor)
        {
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("exec usp_agrega_consumidor @dnicont, @nomcont, @apecont, @emailcont, @idpais",sqlConnection);

            sqlCommand.Parameters.AddWithValue("@dnicont", consumidor.dnicont);
            sqlCommand.Parameters.AddWithValue("@nomcont", consumidor.nomcont);
            sqlCommand.Parameters.AddWithValue("@apecont", consumidor.apecont);
            sqlCommand.Parameters.AddWithValue("@emailcont", consumidor.emailcont);
            sqlCommand.Parameters.AddWithValue("@idpais", consumidor.idpais);

            int cantidadFilas = sqlCommand.ExecuteNonQuery();
            return cantidadFilas > 0 ? "Dato guardado" : "Error";
        }

        string actualizarConsumidor(Consumidor consumidor, string dniOriginal)
        {
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(
                "UPDATE tb_Consumidor SET " +
                "dnicont = @dniNuevo, " +
                "nomcont = @nomcont, " +
                "apecont = @apecont, " +
                "emailcont = @emailcont, " +
                "idpais = @idpais " +
                "WHERE dnicont = @dniOriginal",
                sqlConnection);

            sqlCommand.Parameters.AddWithValue("@dniOriginal", dniOriginal);
            sqlCommand.Parameters.AddWithValue("@dniNuevo", consumidor.dnicont);
            sqlCommand.Parameters.AddWithValue("@nomcont", consumidor.nomcont);
            sqlCommand.Parameters.AddWithValue("@apecont", consumidor.apecont);
            sqlCommand.Parameters.AddWithValue("@emailcont", consumidor.emailcont);
            sqlCommand.Parameters.AddWithValue("@idpais", consumidor.idpais);

            int cantidadCambios = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            if (cantidadCambios > 0)
            {
                return "Consumidor actualizado correctamente";
            }

            return "No se actualizó ningún consumidor";
        }

        Consumidor recuperarConsumidorPorDni(string dni)
        {
            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("select * from tb_Consumidor where dnicont = @dnicont", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@dnicont", dni);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                return new Consumidor
                {
                    dnicont = sqlDataReader.GetString(0),
                    nomcont = sqlDataReader.GetString(1),
                    apecont = sqlDataReader.GetString(2),
                    emailcont = sqlDataReader.GetString(3),
                    idpais = sqlDataReader.GetString(4)
                };
            }
            ;
            return null;
        }
        string EliminarConsumidorPorDni(string dni)
        {

            SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("usp_eliminar_consumidor", sqlConnection);

            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@dnicont", dni);

            int cantidad = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            if (cantidad > 0)
            {
                return "Consumidor eliminado correctamente";
            }

            return "No se pudo eliminar. Reintentar";
        }

        public ActionResult Index()
        {
            return View(listarConsumidores());
        }

        public ActionResult Create()
        {
            ViewBag.paises = new SelectList(listarPaises(), "idpais", "idpais");
            return View(new Consumidor());
        }

        [HttpPost]
        public ActionResult Create(Consumidor consumidor)
        {
            ViewBag.paises = new SelectList(
                listarPaises(), "idpais", "idpais", consumidor.idpais);

            ViewBag.mensaje = guardarConsumidor(consumidor);

            return View(consumidor);
        }

        public ActionResult Update(string dni)
        {
            Consumidor consumidorEncontrado = recuperarConsumidorPorDni(dni);

            if (consumidorEncontrado == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.paises = new SelectList(
                listarPaises(), "idpais", "idpais", consumidorEncontrado.idpais);

            return View(consumidorEncontrado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Consumidor consumidor, string dniOriginal)
        {
            ViewBag.paises = new SelectList(
                listarPaises(), "idpais", "idpais", consumidor.idpais);

            ViewBag.dniOriginal = dniOriginal;

            if (!ModelState.IsValid ||
                string.IsNullOrWhiteSpace(dniOriginal))
            {
                ViewBag.mensaje = "Verifique los datos ingresados";
                return View(consumidor);
            }

            try
            {
                string mensaje = actualizarConsumidor(consumidor, dniOriginal);

                TempData["Mensaje"] = mensaje;

                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    ViewBag.mensaje = "El DNI nuevo ya está registrado";
                }
                else
                {
                    ViewBag.mensaje = "Error al actualizar: " + ex.Message;
                }

                return View(consumidor);
            }
        }

        public ActionResult Details(string dni)
        {
            Consumidor consumidorEncontrado = recuperarConsumidorPorDni(dni);
            return View(consumidorEncontrado);
        }
        public ActionResult Delete(string dni)
        {
            Consumidor consumidorEncontrado = recuperarConsumidorPorDni(dni);
            return View(consumidorEncontrado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Consumidor consumidor)
        {
            string mensaje = EliminarConsumidorPorDni(consumidor.dnicont);
            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

    }  
}