using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class DiscosNegocio
    {
        public List<Disco> listar()
        {
            List<Disco> lista = new List<Disco>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=DISCOS_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select Titulo, FechaLanzamiento, CantidadCanciones, UrlImagenTapa, E.Descripcion Estilo, T.Descripcion Edicion, D.IdEstilo, D.IdTipoEdicion, D.Id From DISCOS D, ESTILOS E, TIPOSEDICION T Where D.IdEstilo = E.Id And D.IdTipoEdicion = T.Id";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Disco aux = new Disco();
                    aux.Id = (int)lector["Id"];
                    aux.Titulo = (string)lector["Titulo"];
                    aux.FechaLanzamiento = (DateTime)lector["FechaLanzamiento"];
                    aux.CantidadCanciones = (int)lector["CantidadCanciones"];
                    if (!(lector["UrlImagenTapa"] is DBNull))
                    {
                        aux.UrlImagen = (string)lector["UrlImagenTapa"];
                    }
                    aux.Genero = new Estilo();
                    aux.Genero.Id = (int)lector["IdEstilo"];
                    aux.Genero.Descripcion = (string)lector["Estilo"];
                    aux.TipoEdicion = new Edicion();
                    aux.TipoEdicion.Id = (int)lector["IdTipoEdicion"];
                    aux.TipoEdicion.Descripcion = (string)lector["Edicion"];

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void agregar(Disco nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Insert into DISCOS (Titulo, FechaLanzamiento, CantidadCanciones, IdEstilo, IdTipoEdicion, UrlImagenTapa) values ('" + nuevo.Titulo + "', @fechaLanzamiento, " + nuevo.CantidadCanciones + ", @idEstilo, @idTipoEdicion, @urlImagenTapa)");
                datos.setearParametro("@fechaLanzamiento", nuevo.FechaLanzamiento);
                datos.setearParametro("@idEstilo", nuevo.Genero.Id);
                datos.setearParametro("@idTipoEdicion", nuevo.TipoEdicion.Id);
                datos.setearParametro("@urlImagenTapa", nuevo.UrlImagen);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Disco disco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Update DISCOS set Titulo = @titulo, FechaLanzamiento = @fechaLanzamiento, CantidadCanciones = @cantCanciones, UrlImagenTapa = @img, IdEstilo = @idEstilo, IdTipoEdicion = @idTipoEdicion where id = @id");
                datos.setearParametro("@titulo", disco.Titulo);
                datos.setearParametro("@fechaLanzamiento", disco.FechaLanzamiento);
                datos.setearParametro("@cantCanciones", disco.CantidadCanciones);
                datos.setearParametro("@img", disco.UrlImagen);
                datos.setearParametro("@idEstilo", disco.Genero.Id);
                datos.setearParametro("@idTipoEdicion", disco.TipoEdicion.Id);
                datos.setearParametro("@id", disco.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            { 
                datos.cerrarConexion();
            }
        }

        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Delete from DISCOS where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Disco> filtrar(string campo, string criterio, string filtro)
        {
            List<Disco> lista = new List<Disco>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Titulo, FechaLanzamiento, CantidadCanciones, UrlImagenTapa, E.Descripcion Estilo, T.Descripcion Edicion, D.IdEstilo, D.IdTipoEdicion, D.Id From DISCOS D, ESTILOS E, TIPOSEDICION T Where D.IdEstilo = E.Id And D.IdTipoEdicion = T.Id And ";

                if (campo == "Título")
                {
                    switch (criterio)
                    {

                        case "Comienza con":
                            consulta += "Titulo like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Titulo like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Titulo like '%" + filtro + "%'";
                            break;
                    }
                    
                }
                else if (campo == "Cantidad de canciones")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "CantidadCanciones > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "CantidadCanciones < " + filtro;
                            break;
                        default:
                            consulta += "CantidadCanciones = " + filtro;
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {

                        case "Comienza con":
                            consulta += "E.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "E.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "E.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }


                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Disco aux = new Disco();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = (string)datos.Lector["Titulo"];
                    aux.FechaLanzamiento = (DateTime)datos.Lector["FechaLanzamiento"];
                    aux.CantidadCanciones = (int)datos.Lector["CantidadCanciones"];
                    if (!(datos.Lector["UrlImagenTapa"] is DBNull))
                    {
                        aux.UrlImagen = (string)datos.Lector["UrlImagenTapa"];
                    }
                    aux.Genero = new Estilo();
                    aux.Genero.Id = (int)datos.Lector["IdEstilo"];
                    aux.Genero.Descripcion = (string)datos.Lector["Estilo"];
                    aux.TipoEdicion = new Edicion();
                    aux.TipoEdicion.Id = (int)datos.Lector["IdTipoEdicion"];
                    aux.TipoEdicion.Descripcion = (string)datos.Lector["Edicion"];

                    lista.Add(aux);
                }

                return lista;
             }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
