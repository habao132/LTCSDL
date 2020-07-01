using LTCSDL.Common.DAL;
using System.Linq;

namespace LTCSDL.DAL
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class CategoriesRep : GenericRep<NorthwindContext, Categories>
    {
        #region -- Overrides --

        /// <summary>
        /// Read single object
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Return the object</returns>
        public override Categories Read(int id)
        {
            var res = All.FirstOrDefault(p => p.CategoryId == id);
            return res;
        }


        /// <summary>
        /// Remove and not restore
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Number of affect</returns>
        public int Remove(int id)
        {
            var m = base.All.First(i => i.CategoryId == id);
            m = base.Delete(m); //TODO
            return m.CategoryId;
        }

        #endregion

        #region -- Methods --

        /// <summary>
        /// Initialize
        /// </summary>
        public List<object> GetCustOrderHist(string cusID)
        {
            List<object> res = new List<object>();
            var cnn = (SqlConnection)Context.Database.GetDbConnection();
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(); 
                DataSet ds = new DataSet();
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "CustOrderHist";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", cusID);
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var x = new
                        {
                            ProductName = row["ProductName"],
                            Total = row["Total"]
                        };
                        res.Add(x);
                    }
                }
            }
            catch (Exception e)
            {
                res = null;
            }
            return res;
        }

        public object GetCustOrderHist_Linq(string cusID)
        {
            var pro = Context.Products
                .Join(Context.OrderDetails, a => a.ProductId, b => b.ProductId, (a, b) => new
                {
                    a.ProductId,
                    a.ProductName,
                    b.Quantity,
                    b.OrderId
                })
                .Join(Context.Orders, a => a.OrderId, b => b.OrderId, (a, b) => new
                {
                    a.ProductId,
                    a.ProductName,
                    a.Quantity,
                    a.OrderId,
                    b.CustomerId
                }).Where(x => x.CustomerId == cusID).ToList();
            var res = pro.GroupBy(x => x.ProductName) 
                .Select(x => new 
                {
                    ProductName = x.First().ProductName,
                    ProductId = x.First().ProductId,

                    Total = x.Sum(c => c.Quantity)

                 }).ToList();
            return res;
        }
        ///

        public object GetCustOrdersDetail_Linq1(int orderId)
        {
            var res = Context.Products
                .Join(Context.OrderDetails, a => a.ProductId, b => b.ProductId, (a, b) => new
                {
                    b.OrderId,
                    a.ProductName,
                    b.UnitPrice,
                    b.Quantity,
                    DisCount = b.Discount * 100,
                    ExtendedPrice = ((decimal)b.Quantity) * (1 - (decimal)b.Discount) * (b.UnitPrice)
                }).Where(x => x.OrderId == orderId).ToList();
                
            return res;
        }

        public object GetCustOrdersDetail_Linq2(int orderId)
        {
            var res = from p in Context.Products
                      join d in Context.OrderDetails on p.ProductId equals d.ProductId
                      where d.OrderId == orderId
                      select new
                      {
                          d.OrderId,
                          p.ProductName,
                          d.UnitPrice,
                          d.Quantity,
                          DisCount = d.Discount * 100,
                          ExtendedPrice = ((decimal)d.Quantity) * (1 - (decimal)d.Discount) * (d.UnitPrice)
                      };

            return res;
        }

        public object getDTNVTrongNgay_LinQ(DateTime date)
        {
            var res = Context.Orders
                .Where(x => x.OrderDate.HasValue
                && x.OrderDate.Value.Date == date.Date)
                .Join(Context.Employees, a => a.EmployeeId, b => b.EmployeeId, (a, b) => new
                {
                    a.OrderId,
                    a.OrderDate,
                    a.EmployeeId,
                    b.LastName,
                    b.FirstName
                })
                .Join(Context.OrderDetails, a => a.OrderId, b => b.OrderId, (a, b) => new
                {
                    a.OrderId,
                    a.OrderDate,
                    a.EmployeeId,
                    a.LastName,
                    a.FirstName,
                    DT = b.UnitPrice * b.Quantity * (1 - (decimal)b.Discount)
                })
                .ToList();
            var data = res.GroupBy(x => x.EmployeeId)
                        .Select(x => new
                        {
                            x.First().EmployeeId,
                            x.First().FirstName,
                            x.First().LastName,
                            DoanhThu = x.Sum(t => t.DT)
                        });
            return data;
        }
        public List<object> XuatDSDonHang(DateTime dateF, DateTime dateT)

        {
            List<object> res = new List<object>();
            var cnn = (SqlConnection)Context.Database.GetDbConnection();
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "DSDonHang";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dateF", dateF);
                cmd.Parameters.AddWithValue("@dateT", dateT);

                da.SelectCommand = cmd;
                da.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var x = new
                        {
                            STT = row["STT"],
                            OrderID = row["OrderID"],
                            EmployeeID = row["EmployeeID"],
                            OrderDate = row["OrderDate"],
                            LastName = row["LastName"]
                        };
                        res.Add(x);
                    }
                }

            }
            catch (Exception e)
            {
                res = null;
            }
            return res;

        }


        #endregion
    }



}