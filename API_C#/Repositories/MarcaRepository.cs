using ConcesionarioApi.Interfaces;
using ConcesionarioApi.Models;

namespace ConcesionarioApi.Repositories
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly List<Marca> _marcas;

        public MarcaRepository()
        {
            _marcas = new List<Marca>
            {
                new Marca { Id = 1, Nombre = "Toyota", Descripcion = "Marca japonesa" },
                new Marca { Id = 2, Nombre = "Renault", Descripcion = "Marca francesa" },
                new Marca { Id = 3, Nombre = "Chevrolet", Descripcion = "Marca estadounidense" }
            };
        }

        public IEnumerable<Marca> GetAll() => _marcas;

        public Marca? GetById(int id) => _marcas.FirstOrDefault(m => m.Id == id);

        public Marca Create(Marca marca)
        {
            marca.Id = _marcas.Any() ? _marcas.Max(m => m.Id) + 1 : 1;
            _marcas.Add(marca);
            return marca;
        }

        public bool Update(int id, Marca updatedMarca)
        {
            var existingMarca = GetById(id);
            if (existingMarca == null) return false;

            existingMarca.Nombre = updatedMarca.Nombre;
            existingMarca.Descripcion = updatedMarca.Descripcion;
            return true;
        }

        public bool Delete(int id)
        {
            var marca = GetById(id);
            if (marca == null) return false;

            _marcas.Remove(marca);
            return true;
        }
    }
}
