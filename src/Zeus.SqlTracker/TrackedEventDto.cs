using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Trackers
{
    [Table("TrackedEvent")]
    public class TrackedEventDto
    {
        [Key]
        public int EventId { get; set; }

        public int ProductId { get; set; }

        public byte EventType { get; set; }

        public DateTime EventDate { get; set; }
    }
}
