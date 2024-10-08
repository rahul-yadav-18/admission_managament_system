using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using mymainproject;
using MyProject.Models;

namespace mymainproject.Controllers
{
    [CheckStudentSession]
    public class StudentController : Controller
    {
        // GET: Student
        DBManager db = new DBManager();
        public ActionResult Index()
        {
            string email = Session["semail"].ToString();
            string query = "select * from tbl_student where email='" + email + "'";
            DataTable da = db.ExecuteSelect(query);
            ViewBag.stu = da;

            string qu = "select pic from tbl_profilepic where email_id='" + email + "'";
            ViewBag.pic = db.ExecuteSelect(qu);
            return View();
        }

        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(string opass, string npass, string cnpass)
        {
            string query = "select password from tbl_student where password='" + opass + "'";
            DataTable dt = db.ExecuteSelect(query);
            if (dt.Rows.Count > 0)
            {
                if (npass == cnpass)
                {
                    string que = "update tbl_student set password='" + npass + "'";
                    DataTable dd = db.ExecuteSelect(que);
                    return Content("<script>alert('Password Change Successfully');location.href='/student/changepass'</script>");
                }
                else
                {
                    return Content("<script>alert('Fill Same New Password ');location.href='/student/changepass'</script>");
                }

            }
            else
            {
                return Content("<script>alert('Ennter Invalid old Password');location.href='/student/changepass'</script>");

            }

        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("signin", "home");
        }
        public ActionResult VideoCategory()
        {
            if (Session["sbatch"]!=null)
            {
                string q = "select * from tbl_videocategory order by cid desc";
                ViewBag.vcat = db.ExecuteSelect(q);
                return View();

            }
            

            return View();
        }
        public ActionResult Video(int? cid)
        {
            if (Session["sbatch"] != null && cid.HasValue)
            {
                int batch = Convert.ToInt32(Session["sbatch"]);
                string query = "select * from tbl_video where batch_id=" + batch + "and cat_id=" + cid + " order by vid desc ";
                ViewBag.Video = db.ExecuteSelect(query);
                return View();
            }
            else
            {
                return Content("<script>alert('Error occured.Please Login again');location.href='/student/Lecturevideo'</script>");
            }

           
        }

        public ActionResult Task()
        {
            int batch = Convert.ToInt32(Session["sbatch"]);
            string q2 = "select * from tbl_task where batch_id="+batch+"   ";
            DataTable task = db.ExecuteSelect(q2);
            ViewBag.task = task;
            return View();
        }
        public ActionResult UploadTask()
        {
            string email = Session["semail"].ToString();
            string q2 = "select task_name,max_mark,ob_mark from tbl_submittask where email='"+email+"' order by sr desc ";
            ViewBag.mark = db.ExecuteSelect(q2);
            return View();
        }
        [HttpPost]
        public ActionResult UploadTask(string tname,HttpPostedFileBase cfile,HttpPostedFileBase ofile)
        {
            string email = Session["semail"].ToString();
            int result = db.ExecuteInsertUpdateDelete("insert into tbl_submittask values('"+tname+"','"+cfile.FileName+"','"+ofile.FileName+"',0,0,'"+email+"')  ");
            if(result>0)
            {
                cfile.SaveAs(Server.MapPath("/Content/codefile/") + cfile.FileName);
                ofile.SaveAs(Server.MapPath("/Content/outputfile/") + ofile.FileName);
                return Content("<script>alert('Thanks for contacting with us..'); location.href='/student/uploadtask'</script>");
            }
            else
            {
                return Content("<script>alert('Data Not Saved..'); location.href='/student/uploadtask'</script>");
            }
           
        }
        public ActionResult Profile()
        {
            int batch = Convert.ToInt32(Session["sbatch"]);
            string email = Session["semail"].ToString();
            string query = "select * from tbl_student where email='" + email + "'";
            DataTable dm = db.ExecuteSelect(query);
            ViewBag.profile = dm;
            string querys = "select bname from tbl_batch where bid=" + batch + "";
            DataTable dcx = db.ExecuteSelect(querys);
            ViewBag.batc = dcx;

            string qu = "select pic from tbl_profilepic where email_id='" + email + "'";
            ViewBag.pic = db.ExecuteSelect(qu);
            return View();
        }
        public ActionResult UpdateProfile()
        {
            string email = Session["semail"].ToString();
            string query = "select * from tbl_student where email='" + email + "'";
            DataTable dm = db.ExecuteSelect(query);
            ViewBag.profiles = dm;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateProfile(string name,long mobno,string college,string course, HttpPostedFileBase pic)
        {
            string email = Session["semail"].ToString();           
            string ques = "select pic from tbl_profilepic where email_id='"+email+"'";
            string que = "update tbl_profilepic set pic='" + pic.FileName + "' where email_id='" + email + "'";
            string quest = "update tbl_student set name='" + name + "' ,mobno= " + mobno + ",college='" + college + "',course='" + course + "'  where email='" + email + "'  ";
            string querys = "insert into tbl_profilepic values('" + pic.FileName + "','" + email + "')";
            int up = db.ExecuteInsertUpdateDelete(quest);
            if (up > 0)
            {
                DataTable dct = db.ExecuteSelect(ques);
                if (dct.Rows.Count > 0)
                {
                    int rest = db.ExecuteInsertUpdateDelete(que);
                    if (rest > 0)
                    {
                        pic.SaveAs(Server.MapPath("/Content/profilepic/") + pic.FileName);
                        return Content("<script>alert('Profile  Upadate successfully ..'); location.href='/student/profile'</script>");
                    }
                    else
                    {
                        return Content("<script>alert('please select the image..'); location.href='/student/updateprofile'</script>");
                    }
                }
                else
                {                  
                    int res = db.ExecuteInsertUpdateDelete(querys);
                    if (res > 0)
                    {
                        pic.SaveAs(Server.MapPath("/Content/profilepic/") + pic.FileName);
                        return Content("<script>alert('Profile  Upadate successfully ..'); location.href='/student/profile'</script>");
                    }
                    else
                    {
                        return Content("<script>alert('Data fill properly..'); location.href='/student/updateprofile'</script>");
                    }
                }

            }
            else
            {
                return Content("<script>alert('Profile  Upadate successfully  1 ..'); location.href='/student/profile'</script>");
            }
        }
        public ActionResult SoftwareKit()
        {
            return View();
        }
        public ActionResult StudyMaterial()
        {
            int batch = Convert.ToInt32(Session["sbatch"]);
            ViewBag.stm = db.ExecuteSelect("select * from tbl_pdf where bid="+batch+"");
            return View();
        }
        public ActionResult SendFeedback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendFeedback(string message)
        {
            string email = Session["semail"].ToString();
            string q = "insert into tbl_sfeedback values('"+message+"','"+email+"','"+ DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd") + "')";
            int result = db.ExecuteInsertUpdateDelete(q);
            if(result>0)
            {
                return Content("<script>alert('Thanks for submit Valuable feedback ..'); location.href='/student/sendfeedback'</script>");
            }
            else
            {
                return Content("<script>alert('error again Login..'); location.href='/home/signin'</script>");
            }
           
        }
        public ActionResult Help()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Help(string message)
        {
            string email = Session["semail"].ToString();
            ViewBag.sth = db.ExecuteSelect("select * from tbl_shelp where email='"+email+"'");
            if(ViewBag.sth.Rows.Count>0)
            {
                return Content("<script>alert('Wait some times for responsen ..'); location.href='/student/help'</script>");
            }
            else
            {
                string q = "insert into tbl_shelp values('" + message + "','" + email + "','"+ DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd") + "')";
                int result = db.ExecuteInsertUpdateDelete(q);
                if (result > 0)
                {
                    return Content("<script>alert('Wait some times for response ..'); location.href='/student/help'</script>");
                }
                else
                {
                    return Content("<script>alert('error again Login..'); location.href='/home/signin'</script>");
                }
            }
            
            
        }
         public ActionResult Classlink()
        {
            int batch = Convert.ToInt32(Session["sbatch"]);
            string q1 = "select * from tbl_classlink where batch=" + batch + " order by lid desc ";
            ViewBag.link = db.ExecuteSelect(q1);
            return View();
        }

    }
    class CheckStudentSession : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["semail"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    { "Controller","Home"},
                    {"Action","signin" }
                });
            }
        }
    }
}