﻿using System;
using System.Collections.Generic;

namespace FullApiOnlineStore.Models
{
    public partial class Category
    {
        public Category()
        {
            Items = new HashSet<Item>();
        }

        public int CategoryId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
