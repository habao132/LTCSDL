using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LTCSDL.Web.Controllers
{
    using BLL;
    using DAL.Models;
    using Common.Req;
    using System.Collections.Generic;
    //using BLL.Req;
    using Common.Rsp;

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController()
        {
            _svc = new CategoriesSvc();
        }

        [HttpPost("get-by-id")]
        public IActionResult getCategoryById([FromBody]SimpleReq req)
        {
            var res = new SingleRsp();
            res = _svc.Read(req.Id);
            return Ok(res);
        }

        [HttpPost("get-all")]
        public IActionResult getAllCategories()
        {
            var res = new SingleRsp();
            res.Data = _svc.All;
            return Ok(res);
        }

        [HttpPost("get-cust-orders-hist")]
        public IActionResult GetCustOrderHist([FromBody]OrderReq req)
        {
            var res = new SingleRsp();
            var hist = _svc.GetCustOrderHist(req.custId);
            res.Data = hist;
            return Ok(res);
        }

        [HttpPost("get-cust-orders-hist-Linq")]
        public IActionResult GetCustOrderHist_Linq([FromBody]OrderReq req)
        {
            var res = new SingleRsp();
            var hist = _svc.GetCustOrderHist_Linq(req.custId);
            res.Data = hist;
            return Ok(res);
        }

        [HttpPost("get-cust-orders-detail-Linq1")]
        public IActionResult GetCustOrdersDetail_Linq1([FromBody]OrderReq req)
        {
            var res = new SingleRsp();
            var hist = _svc.GetCustOrdersDetail_Linq1(req.OrderId);
            res.Data = hist;
            return Ok(res);
        }

        [HttpPost("get-cust-orders-detail-Linq2")]
        public IActionResult GetCustOrdersDetail_Linq2([FromBody]OrderReq req)
        {
            var res = new SingleRsp();
            var hist = _svc.GetCustOrdersDetail_Linq2(req.OrderId);
            res.Data = hist;
            return Ok(res);
        }



        [HttpPost("xuất chi tiết đơn hàng")]
        public IActionResult XuatDSDonHang([FromBody] OrderReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.GetDSDonHang(req.dateF, req.dateT);
            return Ok(res);
        }
        private readonly CategoriesSvc _svc;

        
    }
}