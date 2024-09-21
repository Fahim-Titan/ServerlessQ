using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class Attachment
    {
        public int Id { get; set; }
        public Byte[] Svg { get; set; }
        // need to make the FK on the Person
    }
}
