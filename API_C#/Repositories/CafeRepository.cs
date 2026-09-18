using ConcesionarioApi.Interfaces;
using ConcesionarioApi.Models;

namespace ConcesionarioApi.Repositories
{
    public class CafeRepository : ICafeRepository
    {
        private readonly List<Cafe> _cafes = new List<Cafe>();
        private readonly IMarcaRepository _marcaRepository;

        public CafeRepository(IMarcaRepository marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        public IEnumerable<Cafe> GetAll() => _cafes;

        public Cafe? GetById(int id) => _cafes.FirstOrDefault(c => c.Id == id);

        public Cafe Create(Cafe cafe)
        {
            var marca = _marcaRepository.GetById(cafe.MarcaId);
            if (marca != null)
            {
                cafe.Marca = marca.Nombre;

                if (cafe.Precio > 0)
                {
                    decimal descuento = 0;
                    switch (marca.Nombre.ToLower())
                    {
                        case "premium":
                            descuento = 0.15m;
                            break;
                        case "especial":
                            descuento = 0.20m;
                            break;
                        case "orgánico":
                            descuento = 0.10m;
                            break;
                    }

                    cafe.Precio = cafe.Precio - (cafe.Precio * descuento);
                }
            }

            cafe.Id = _cafes.Any() ? _cafes.Max(c => c.Id) + 1 : 1;
            _cafes.Add(cafe);
            return cafe;
        }

        public bool Update(int id, Cafe updatedCafe)
        {
            var existingCafe = GetById(id);
            if (existingCafe == null) return false;

            var marca = _marcaRepository.GetById(updatedCafe.MarcaId);
            if (marca != null)
            {
                existingCafe.MarcaId = updatedCafe.MarcaId;
                existingCafe.Marca = marca.Nombre;
            }
            else
            {
                existingCafe.MarcaId = updatedCafe.MarcaId;
                existingCafe.Marca = updatedCafe.Marca;
            }

            existingCafe.Nombre = updatedCafe.Nombre;
            existingCafe.Origen = updatedCafe.Origen;
            existingCafe.Stock = updatedCafe.Stock;
            existingCafe.Precio = updatedCafe.Precio;

            return true;
        }

        public bool Delete(int id)
        {
            var cafe = GetById(id);
            if (cafe == null) return false;

            _cafes.Remove(cafe);
            return true;
        }
    }
}
