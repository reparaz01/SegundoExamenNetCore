using SegundoExamenNetCore.Models;
using System.Data.SqlClient;
using System.Data;
using Oracle.ManagedDataAccess.Client;

#region

/*
 
 - Insert -

create or replace procedure sp_insert_comic
(
    p_nombre in COMICS.NOMBRE%TYPE,
    p_imagen in COMICS.IMAGEN%TYPE,
    p_descripcion in COMICS.DESCRIPCION%TYPE
)
as
    v_next_id number;
begin
    -- Siguiente ID
    select nvl(max(IDCOMIC), 0) + 1 into v_next_id from COMICS;

    INSERT INTO COMICS VALUES(v_next_id, p_nombre, p_imagen, p_descripcion);
    commit;
end;
 
 
 
 - Delete - 

create or replace procedure sp_delete_comic
(p_idcomic COMICS.IDCOMIC%TYPE)
as
begin
  delete from COMICS where IDCOMIC=p_idcomic;
commit;
end;

 
 
 */


#endregion



namespace SegundoExamenNetCore.Repositories
{
    public class RepositoryComicsOracle : IRepositoryComics
    {

        private DataTable tablaComics;
        private OracleConnection cn;
        private OracleCommand com;


        public RepositoryComicsOracle()

        {
            string connectionString = @"Data Source=DESKTOP-A2FPPFB:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from COMICS";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);
        }

        public List<Comic> GetComics()
        {

            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();

            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {

                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),

                };
                comics.Add(comic);
            }
            return comics;
        }

        public Comic FindComicById(int idComic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idComic
                           select datos;

            var row = consulta.First();
            Comic comic = new Comic();
            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");

            return comic;
        }

        public void InsertComic(string nombre, string imagen, string descripcion)
        {

            OracleParameter pamNombre = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(pamNombre);

            OracleParameter pamImagen = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(pamImagen);

            OracleParameter pamDescripcion = new OracleParameter(":descripcion", descripcion);
            this.com.Parameters.Add(pamDescripcion);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_insert_comic";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();


        }



        public void InsertComicLambda(string nombre, string imagen, string descripcion)
        {
            int nextComicId = tablaComics.AsEnumerable().Any() ? tablaComics.AsEnumerable().Max(row => row.Field<int>("IDCOMIC")) + 1 : 1;

            string sql = "INSERT INTO COMICS VALUES (:IDCOMIC, :NOMBRE, :IMAGEN, :DESCRIPCION)";

            OracleParameter pamId = new OracleParameter(":IDCOMIC", nextComicId);
            this.com.Parameters.Add(pamId);
            OracleParameter pamNombre = new OracleParameter(":NOMBRE", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":IMAGEN", imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":DESCRIPCION", descripcion);
            this.com.Parameters.Add(pamDescripcion);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            cn.Open();
            int af = com.ExecuteNonQuery();
            cn.Close();
        }




        public void DeleteComic(int idComic)
        {
            OracleParameter pamIdComic = new OracleParameter(":p_idcomic", idComic);
            this.com.Parameters.Add(pamIdComic);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_delete_comic";

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

    }
}
