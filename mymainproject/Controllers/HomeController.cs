using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace mymainproject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        DBManager dm = new DBManager();
        public ActionResult Index()
        {
            string q1 = "select img_1 from tbl_slider";
            ViewBag.im = dm.ExecuteSelect(q1);
            string q = "select * from tbl_videocategory order by cid desc";
            ViewBag.vcat = dm.ExecuteSelect(q);
            ViewBag.dev = dm.ExecuteSelect("select * from tbl_dev");          
            return View();
            
        }
        public ActionResult DevProfile(int? dev_id)
        {
            if (dev_id.HasValue)
            {
                string query = "select * from tbl_devprofile where dev_sr=" + dev_id + "";
                ViewBag.devp = dm.ExecuteSelect(query);
            }
            return View();
        }
        public ActionResult About() 
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Contact(string name, long mobile, string email, string message)
        {
            
            int x = dm.ExecuteInsertUpdateDelete("insert into tbl_contact values('" + name + "'," + mobile + ",'" + email + "','" + message + "','" + DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd") + "')");
            if (x > 0)
                Response.Write("<script>alert('Thanks for contacting with us..')</script>");
            else
                Response.Write("<script>alert('Data Not Saved..')</script>");
            return View();
        }
        public ActionResult SignUp()
        {
            DataTable dt = dm.ExecuteSelect("select * from tbl_batch order by bid desc");
            ViewBag.bts = dt;
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string name,string email,long mobno,string gender,string college, string password,string course,int? batch)
        {
            int x = dm.ExecuteInsertUpdateDelete("insert into tbl_student values('" + name + "','" + email + "','" + password + "'," + mobno + ",'"+gender+"','"+college+"','" + course + "',"+ batch + ",'"+ DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd") + "')");
            if (x > 0)
            {               
                return Content("<script>alert('Thanks for contacting with us..'); location.href='/home/signup'</script>");
            }
            else
            {
                return Content("<script>alert('Data Not Saved..'); location.href='/home/signup'</script>");
            }
        }
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(string email,string password)
        {
            string query = "select * from tbl_student where email='" + email + "' and password='" + password + "'";
            DataTable dt = dm.ExecuteSelect(query);
            if (dt.Rows.Count > 0)
            {
                Session["semail"] = dt.Rows[0][1];
                Session["sbatch"] = dt.Rows[0][7];
                return RedirectToAction("index", "student");
            }
            else
            {
                return Content("<script>alert('Id are Password is Invailid');location.href='/home/signin'</script>");

            }
        }
        public ActionResult Feedback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Feedback(string name,string email,long mobno,string message, HttpPostedFileBase pic)
        {
            int x = dm.ExecuteInsertUpdateDelete("insert into tbl_feedback values('"+name+"','"+email+"',"+mobno+",'"+message+"','"+pic.FileName+"')");
            if (x > 0)
            {
                pic.SaveAs(Server.MapPath("/Content/caticons/") + pic.FileName);
                return Content("<script>alert('Thanks for contacting with us..'); location.href='/home/feedback'</script>");
            }
            else
            {
                return Content("<script>alert('Data Not Saved..'); location.href='/home/feedback'</script>");
            }
           
        }
        public ActionResult StudentLogin()
        {
            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(string adminid,string password)
        {
            string query = "select * from tbl_adminlogin where adminid='"+adminid+"' and password='"+password+"'";
            DataTable dt = dm.ExecuteSelect(query);
            if(dt.Rows.Count>0)
            {
                Session["admin"] = adminid;
                return RedirectToAction("index","admin");
            }
            else
            {
                return Content("<script>alert('Id are Password is Invailid');location.href='/home/adminlogin'</script>");

            }
           
        }
       

    }
   
}