namespace ConcesionarioApi.Models
{
    public class Cafe
    {
        public int Id { get; set; }
        public int MarcaId { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Precio { get; set; }
    }
}
