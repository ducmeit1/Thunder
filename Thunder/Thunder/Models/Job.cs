using SQLite;
using System;

namespace Thunder.Models
{
    public partial class Job
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(255)]
        public string Name { get; set; }
        [NotNull]
        public string Description { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(255)]
        public string Note { get; set; }
        [NotNull]
        public double Reward { get; set; }
        [NotNull]
        public DateTime DateStart { get; set; }
        [NotNull]
        public DateTime DateEnd { get; set; }
        public TimeSpan TimeDeadLine { get; set; }
        [NotNull]
        public bool IsFinished { get; set; }
        [NotNull]
        public int UserId { get; set; }

    }
}
