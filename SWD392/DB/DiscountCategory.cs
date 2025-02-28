    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    namespace SWD392.DB
    {
        public class DiscountCategory
        {
            [Key]
            public int Id { get; set; }
            public string Name { get; set; }

            public virtual List<Discount> Discounts { get; set; } = new List<Discount>();
        }
    }