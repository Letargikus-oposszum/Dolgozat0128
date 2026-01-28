using Dolgozat0128.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolgozat0128
{
    public class Data
    {
        public List<Parts> parts { get; set; }
        public List<Customers> customers { get; set; }
        public List<Orders> orders { get; set; }
        public Summary summary { get; set; }


    }
}
