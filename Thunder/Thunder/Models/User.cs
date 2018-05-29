using SQLite;

namespace Thunder.Models
{
    public partial class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(255)]
        public string Name { get; set; }

        [NotNull, MaxLength(255)]
        public string Email { get; set; }

        [NotNull, MaxLength(50)]
        public string Password { get; set; }

        [NotNull]
        public string Address { get; set; }

        [NotNull]
        public string ContactNumber { get; set; }

        [NotNull]
        public double Money { get; set; }

        [NotNull]
        public bool IsVerified { get; set; }
    }
}
