using ConcesionarioApi.Models;

namespace ConcesionarioApi.Interfaces
{
    public interface IMarcaRepository
    {
        IEnumerable<Marca> GetAll();
        Marca? GetById(int id);
        Marca Create(Marca marca);
        bool Update(int id, Marca marca);
        bool Delete(int id);
    }
}
