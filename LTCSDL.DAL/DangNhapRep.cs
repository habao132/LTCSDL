﻿using LTCSDL.Common.DAL;
using LTCSDL.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTCSDL.DAL
{
    using LTCSDL.Common.Rsp;
    using Microsoft.EntityFrameworkCore.Internal;
    using Models;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    public class DangNhapRep : GenericRep<MyPhamContext, User>
    {
        #region -- Overrides --

        /// <summary>
        /// Read single object
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Return the object</returns>
        public override User Read(int id)
        {
            var res = All.FirstOrDefault(p => p.Id == id);
            return res;
        }


        /// <summary>
        /// Remove and not restore
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Number of affect</returns>
        public int Remove(int id)
        {
            var m = base.All.First(i => i.Id == id);
            m = base.Delete(m); //TODO
            return m.Id;
        }

        public User findByUserNameAndPassWord(String username, String password)
        {
            /*var context = new MyPhamContext();
            var res = from s in Context.User
                      join r in Context.Role on s.Roleid equals r.Id
                      where s.Username == Username && s.Password == password
                      select s;
            
            return res.FirstOrDefault();*/
            var res = Context.User.Join(Context.Role, a => a.Roleid, b => b.Id, (a, b) => new
            {
                a,
                b,
            }).Where(x => x.a.Username == username && x.a.Password == password).
            Select(x => new User { 
                Id = x.a.Id,
                Username = x.a.Username,
                Password = x.a.Password,
                Ho = x.a.Ho,
                Ten = x.a.Ten,
                Email = x.a.Email,
                Sdt = x.a.Sdt,
                AccessToken = x.a.AccessToken,
                Roleid = x.a.Roleid,
                Role = x.b,
                Transaction = x.a.Transaction
            });
            return res.FirstOrDefault();
            
        }

        
        public SingleRsp CreateNewUser(User dn)
        {
            var res = new SingleRsp();

            using (var context = new MyPhamContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (checkExistbyUserName(dn.Username))
                        {
                            var t = context.User.Add(dn);
                            context.SaveChanges();
                            tran.Commit();
                        }
                        else
                        {
                            res.SetMessage("Exist User");

                            tran.Rollback();
                        }


                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        res.SetError(e.StackTrace);
                    }
                }

            }

            return res;
        }

        public SingleRsp UpdateUser(User dn)
        {
            var res = new SingleRsp();

            using (var context = new MyPhamContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!checkExistbyID(dn.Id))
                        {
                            var t = context.User.Update(dn);
                            context.SaveChanges();
                            tran.Commit();
                        }
                        else
                        {
                            res.SetMessage("No User Match");
                            tran.Rollback();
                        }

                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        res.SetError(e.StackTrace);
                    }
                }

            }

            return res;
        }

        public SingleRsp DeleteUser(User dn)
        {
            var res = new SingleRsp();

            using (var context = new MyPhamContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!checkExistbyID(dn.Id))
                        {
                            var t = context.User.Remove(dn);
                            context.SaveChanges();
                            tran.Commit();
                        }
                        else
                        {

                            res.SetMessage("No User Match");
                            tran.Rollback();
                        }


                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        res.SetError(e.StackTrace);
                    }
                }

            }

            return res;
        }




        private Boolean checkExistbyUserName(String username)
        {
            var id = All.FirstOrDefault(p => p.Username == username);
            if (id == null)
            {
                return true;
            }
            return false;
        }

        private Boolean checkExistbyID(int Id)
        {
            var id = All.FirstOrDefault(p => p.Id == Id);
            if (id == null)
            {
                return true;
            }
            return false;
        }

        private Role GetRoleById(int Id) {
            var res = Context.Role.FirstOrDefault(p => p.Id == Id);
            return res;
        }


        #endregion


    }
}