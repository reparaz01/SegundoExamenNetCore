using SegundoExamenNetCore.Models;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;


#region
/*

- Insertar - 

create procedure SP_INSERT_COMIC
(@NOMBRE NVARCHAR(50), @IMAGEN NVARCHAR(50), @DESCRIPCION NVARCHAR(50))
AS
    DECLARE @nextId INT
	SELECT @nextId = max(IDCOMIC) +1 FROM COMICS
	INSERT INTO COMICS VALUES (@nextId, @NOMBRE, @IMAGEN, @DESCRIPCION)
GO

- Delete - 

create procedure SP_DELETE_COMIC
(@IDCOMIC int)
as
	delete from COMICS where IDCOMIC=@IDCOMIC
go






*/

#endregion



namespace SegundoExamenNetCore.Repositories
{
    public class RepositoryComicsSqlServer : IRepositoryComics
    {

        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;


        public RepositoryComicsSqlServer()

        {
            string connectionString = @"Data Source=DESKTOP-A2FPPFB\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023;";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tablaComics = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
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
            this.com.Parameters.AddWithValue("@NOMBRE", nombre);
            this.com.Parameters.AddWithValue("@IMAGEN", imagen);
            this.com.Parameters.AddWithValue("@DESCRIPCION", descripcion);


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC";

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }



        public void InsertComicLambda(string nombre, string imagen, string descripcion)
        {
            int nextComicId = tablaComics.AsEnumerable().Any() ? tablaComics.AsEnumerable().Max(row => row.Field<int>("IDCOMIC")) + 1 : 1;

            string sql = "INSERT INTO COMICS VALUES (@IDCOMIC, @NOMBRE, @IMAGEN, @DESCRIPCION)";

            this.com.Parameters.AddWithValue("@IDCOMIC", nextComicId);
            this.com.Parameters.AddWithValue("@NOMBRE", nombre);
            this.com.Parameters.AddWithValue("@IMAGEN", imagen);
            this.com.Parameters.AddWithValue("@DESCRIPCION", descripcion);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }



        public void DeleteComic(int idComic)
        {
            this.com.Parameters.AddWithValue("@IDCOMIC", idComic);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_COMIC";

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }

    }
}
