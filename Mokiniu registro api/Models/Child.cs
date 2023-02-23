namespace Mokiniu_registro_api.Models
{
    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SchoolId { get; set; }

        public int ParentId { get; set; }

    }
}
