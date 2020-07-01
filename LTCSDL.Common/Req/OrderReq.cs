using System;
using System.Collections.Generic;
using System.Text;

namespace LTCSDL.Common.Req
{
    public class OrderReq
    {
        public int OrderId { get; set; }

        public string custId { get; set; }

        public DateTime dateF{ get; set; }

        public DateTime dateT { get; set; }
       

    }
}
