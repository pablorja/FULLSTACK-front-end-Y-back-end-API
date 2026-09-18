using ConcesionarioApi.Interfaces;
using ConcesionarioApi.Models;
using MySqlConnector;

namespace ConcesionarioApi.Repositories
{
    public class CafeRepository : ICafeRepository
    {
        private readonly string _connectionString;

        public CafeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Cafe> GetAll()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "SELECT c.id, c.marca_id, m.nombre AS marca, c.nombre, c.origen, c.stock, c.precio " +
                "FROM cafes c INNER JOIN marcas m ON m.id = c.marca_id ORDER BY c.id;",
                connection);
            using var reader = command.ExecuteReader();
            var cafes = new List<Cafe>();
            while (reader.Read())
            {
                cafes.Add(Map(reader));
            }

            return cafes;
        }

        public Cafe? GetById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "SELECT c.id, c.marca_id, m.nombre AS marca, c.nombre, c.origen, c.stock, c.precio " +
                "FROM cafes c INNER JOIN marcas m ON m.id = c.marca_id WHERE c.id = @id;",
                connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public Cafe Create(Cafe cafe)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "INSERT INTO cafes (marca_id, nombre, origen, stock, precio) " +
                "VALUES (@marcaId, @nombre, @origen, @stock, @precio); SELECT LAST_INSERT_ID();",
                connection);
            AddParameters(command, cafe);
            cafe.Id = Convert.ToInt32(command.ExecuteScalar());
            cafe.Marca = GetMarcaName(connection, cafe.MarcaId);
            return cafe;
        }

        public bool Update(int id, Cafe updatedCafe)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "UPDATE cafes SET marca_id = @marcaId, nombre = @nombre, origen = @origen, " +
                "stock = @stock, precio = @precio WHERE id = @id;",
                connection);
            command.Parameters.AddWithValue("@id", id);
            AddParameters(command, updatedCafe);
            return command.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand("DELETE FROM cafes WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@id", id);
            return command.ExecuteNonQuery() > 0;
        }

        private static void AddParameters(MySqlCommand command, Cafe cafe)
        {
            command.Parameters.AddWithValue("@marcaId", cafe.MarcaId);
            command.Parameters.AddWithValue("@nombre", cafe.Nombre);
            command.Parameters.AddWithValue("@origen", cafe.Origen);
            command.Parameters.AddWithValue("@stock", cafe.Stock);
            command.Parameters.AddWithValue("@precio", cafe.Precio);
        }

        private static string GetMarcaName(MySqlConnection connection, int marcaId)
        {
            using var command = new MySqlCommand("SELECT nombre FROM marcas WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@id", marcaId);
            return command.ExecuteScalar()?.ToString() ?? string.Empty;
        }

        private static Cafe Map(MySqlDataReader reader)
        {
            return new Cafe
            {
                Id = reader.GetInt32("id"),
                MarcaId = reader.GetInt32("marca_id"),
                Marca = reader.GetString("marca"),
                Nombre = reader.GetString("nombre"),
                Origen = reader.GetString("origen"),
                Stock = reader.GetInt32("stock"),
                Precio = reader.GetDecimal("precio"),
            };
        }
    }
}
