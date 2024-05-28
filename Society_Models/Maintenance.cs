using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Society_Models
{
    public class Maintenance
    {
        public int Id { get; set; }


        public float Amount { get; set; }
        public float ReceivedAmount { get; set; }
        public float PendingAmount { get; set; }
        public DateOnly ReceivedDate { get; set; } 
        public DateOnly EntryDate { get; set; }

        public int Year { get; set; }
        public string Month { get; set; }
        public float FineAmount { get; set; }

        [Column(name:"Creation Date")]
        public string? CreationDate { get; set; }

        [Column(name: "Modified Date")]
        public string? ModifiedDate { get; set; }

        [Column(name: "Modified By")]
        public string? ModifiedBy  { get; set; }

        [Column(name: "Pending Amount Received Date")]

        public string? PendingAmountReceivedDate { get; set; }
        public bool Paid { get; set; } = false;

        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public FlatOwner FlatOwner { get; set; }



    }
}
