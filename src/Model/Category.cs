using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GridViewDynamicColumns.Model
{
    public class Category
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public List<CategoryColumn> AllColumns { get; set; } = new List<CategoryColumn>();

        public List<string> SelectedColumnNames { get; set; } = new List<string>();
    }
}
