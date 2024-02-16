using SegundoExamenNetCore.Models;
using System.Data;

namespace SegundoExamenNetCore.Repositories
{
    public interface IRepositoryComics
    {

        List<Comic> GetComics();

        Comic FindComicById(int idComic);

        //List<string> GetNombres();

        void InsertComic(string nombre, string imagen, string descripcion);

        void InsertComicLambda(string nombre, string imagen, string descripcion);


        void DeleteComic(int idComic);


    }
}
