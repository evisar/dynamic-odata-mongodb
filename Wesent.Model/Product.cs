using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wesent.Model
{
    public class Product : Wesent.Common.IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Payment> Payments { get; set; }

        public Product()
        {
            this.Payments = new List<Payment>();
        }
    }
}
