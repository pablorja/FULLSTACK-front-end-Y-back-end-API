using ConcesionarioApi.Interfaces;
using ConcesionarioApi.Models;
using MySqlConnector;

namespace ConcesionarioApi.Repositories
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly string _connectionString;

        public MarcaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Marca> GetAll()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand("SELECT id, nombre, descripcion FROM marcas ORDER BY id;", connection);
            using var reader = command.ExecuteReader();
            var marcas = new List<Marca>();
            while (reader.Read())
            {
                marcas.Add(Map(reader));
            }

            return marcas;
        }

        public Marca? GetById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand("SELECT id, nombre, descripcion FROM marcas WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public Marca Create(Marca marca)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "INSERT INTO marcas (nombre, descripcion) VALUES (@nombre, @descripcion); SELECT LAST_INSERT_ID();",
                connection);
            command.Parameters.AddWithValue("@nombre", marca.Nombre);
            command.Parameters.AddWithValue("@descripcion", marca.Descripcion);
            marca.Id = Convert.ToInt32(command.ExecuteScalar());
            return marca;
        }

        public bool Update(int id, Marca updatedMarca)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "UPDATE marcas SET nombre = @nombre, descripcion = @descripcion WHERE id = @id;",
                connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@nombre", updatedMarca.Nombre);
            command.Parameters.AddWithValue("@descripcion", updatedMarca.Descripcion);
            return command.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var command = new MySqlCommand("DELETE FROM marcas WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@id", id);
            return command.ExecuteNonQuery() > 0;
        }

        private static Marca Map(MySqlDataReader reader)
        {
            return new Marca
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre"),
                Descripcion = reader.GetString("descripcion"),
            };
        }
    }
}
