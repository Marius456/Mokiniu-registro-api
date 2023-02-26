using System.ComponentModel.DataAnnotations;

namespace Mokiniu_registro_api.Models
{
    public class Parent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 12)]
        public string Password { get; set; }
    }
}
