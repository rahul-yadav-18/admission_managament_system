using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Antlr.Runtime.Misc;
using mymainproject;
using MyProject.Models;

namespace mymainproject.Controllers
{
    [CheckSession]

    public class AdminController : Controller
    {
        // GET: Admin

        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Category(int? id)
        {

            DataTable dt = db.ExecuteSelect("select * from tbl_videocategory order by cid desc");
            ViewBag.data = dt;
            if (id != null)
            {
                string query1 = "delete from tbl_videocategory where cid=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("category");
            }
            return View();
        }
        DBManager db = new DBManager();
        [HttpPost]
        public ActionResult Category(string catname, HttpPostedFileBase caticon)
        {
            if (catname != null && caticon != null)
            {
                int x = db.ExecuteInsertUpdateDelete("insert into tbl_videocategory values('" + catname + "','" + caticon.FileName + "')");
                if (x > 0)
                {
                    caticon.SaveAs(Server.MapPath("/Content/caticons/") + caticon.FileName);
                    return Content("<script>alert('Thanks for contacting with us..'); location.href='/admin/category'</script>");
                }
                else
                {
                    return Content("<script>alert('Data Not Saved..'); location.href='/admin/category'</script>");
                }
            }
            else
            {
                return Content("<script>alert('Fill All Field Properly'); location.href='/admin/category'</script>");
            }

        }
        public ActionResult Changepass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(string opass, string npass, string cnpass)
        {
            string query = "select password from tbl_adminlogin where password='" + opass + "'";
            DataTable dt = db.ExecuteSelect(query);
            if (dt.Rows.Count > 0)
            {
                if (npass == cnpass)
                {
                    string que = "update tbl_adminlogin set password='" + npass + "'";
                    DataTable dd = db.ExecuteSelect(que);
                    return Content("<script>alert('Password Change Successfully');location.href='/admin/changepass'</script>");
                }
                else
                {
                    return Content("<script>alert('Fill Same New Password ');location.href='/admin/changepass'</script>");
                }

            }
            else
            {
                return Content("<script>alert('Ennter Invalid old Password');location.href='/admin/changepass'</script>");

            }

        }
        public ActionResult Batch(int? id)
        {
            DataTable dt = db.ExecuteSelect("select * from tbl_batch order by bid desc");
            ViewBag.dat = dt;

            if (id != null)
            {
                string query1 = "delete from tbl_batch where bid=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("Batch");
            }
            return View();

        }
        [HttpPost]
        public ActionResult Batch(string bname, DateTime sdate, DateTime edate, int? tfee, string btopic, HttpPostedFileBase bpic)
        {
            if (bname != null && sdate != null && edate != null && btopic != null && bpic != null)
            {
                int x = db.ExecuteInsertUpdateDelete("insert into tbl_batch values('" + bname + "','" + DateTime.Parse(sdate.ToShortDateString()).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(edate.ToShortDateString()).ToString("yyyy-MM-dd") + "'," + tfee + ",'" + btopic + "','" + bpic.FileName + "')");
                if (x > 0)
                {
                    bpic.SaveAs(Server.MapPath("/Content/batchpic/") + bpic.FileName);
                    return Content("<script>alert('Thanks for contacting with us..'); location.href='/admin/batch'</script>");
                }
                else
                {
                    return Content("<script>alert('Data Not Saved..'); location.href='/admin/batch'</script>");
                }
            }
            else
            {
                return Content("<script>alert('Fill All Field Properly'); location.href='/admin/batch'</script>");
            }
        }
        public ActionResult Assignment(int? id)
        {
            DataTable dt = db.ExecuteSelect("select bid,bname from tbl_batch");
            ViewBag.batches = dt;
            DataTable da = db.ExecuteSelect("select * from tbl_task order by task_id desc");
            ViewBag.dd = da;
            if (id != null)
            {
                string query1 = "delete from tbl_task where task_id=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("Assignment");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Assignment(int? cbatch, string title, string description, HttpPostedFileBase task, string aurthor)
        {
            if (title != null && description != null && task != null && aurthor != null)
            {
                int x = db.ExecuteInsertUpdateDelete("insert into tbl_task values('" + cbatch + "','" + title + "','" + description + "','" + task.FileName + "','" + aurthor + "','" + DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd") + "')");
                if (x > 0)
                {
                    task.SaveAs(Server.MapPath("/Content/assignment/") + task.FileName);
                    return Content("<script>alert('Thanks for contacting with us..'); location.href='/admin/assignment'</script>");
                }
                else
                {
                    return Content("<script>alert('Data Not Saved..'); location.href='/admin/assignment'</script>");
                }
            }
            else
            {
                return Content("<script>alert('Fill All Field Properly'); location.href='/admin/assignment'</script>");
            }
        }
        public ActionResult Video(int? id)
        {
            DataTable dcm = db.ExecuteSelect("select cid,cname from tbl_videocategory");
            ViewBag.bat = dcm;
            DataTable dbm = db.ExecuteSelect("select bid,bname from tbl_batch");
            ViewBag.batch = dbm;
            DataTable dtm = db.ExecuteSelect("select * from tbl_video order by vid desc");
            ViewBag.b = dtm;
            if (id != null)
            {
                string query1 = "delete from tbl_video where vid=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("Video");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Video(int? cbatch, int? vcategory, string title, string description, HttpPostedFileBase video)
        {
            int x = db.ExecuteInsertUpdateDelete("insert into tbl_video values('" + cbatch + "','" + vcategory + "','" + title + "','" + description + "','" + video.FileName + "','" + DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd") + "')");
            if (x > 0)
            {
                video.SaveAs(Server.MapPath("/Content/video/") + video.FileName);
                return Content("<script>alert('Thanks for contacting with us..'); location.href='/admin/video'</script>");
            }
            else
            {
                return Content("<script>alert('Data Not Saved..'); location.href='/admin/video'</script>");
            }
        }


        public ActionResult Classlink(int? id)
        {
            DataTable dt = db.ExecuteSelect("select bid,bname from tbl_batch");
            ViewBag.bac = dt;
            DataTable dm = db.ExecuteSelect("select * from tbl_classlink order by lid desc");
            ViewBag.dm = dm;
            if (id != null)
            {
                string query1 = "delete from tbl_classlink where lid=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("classlink");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Classlink(int? cbatch, string link, DateTime date, string stime, string etime, string message)
        {
            int result = db.ExecuteInsertUpdateDelete("insert into tbl_classlink values(" + cbatch + ",'" + link + "','" + date.ToString("yyyy-MM-dd") + "','" + stime + "','" + etime + "','" + message + "','" + DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd") + "' )");
            if (result > 0)
            {
                return Content("<script>alert('Thanks for contacting with us..'); location.href='/admin/classlink'</script>");
            }
            else
            {
                return Content("<script>alert('Data Not Saved..'); location.href='/admin/classlink'</script>");
            }
        }
        public ActionResult SubmitedTask()
        {

            string q1 = "select sr,task_name,code_file,output_file,tbl_submittask.email,tbl_student.name from tbl_submittask left join tbl_student on tbl_submittask.email=tbl_student.email  order by sr desc  ";
            ViewBag.submittask = db.ExecuteSelect(q1);
            return View();
        }
        [HttpPost]
        public ActionResult SubmitedTask(int? ansid, int? maxnum, int? obnum)
        {
            string q1 = "update tbl_submittask set max_mark=" + maxnum + ",ob_mark=" + obnum + " where sr=" + ansid + " ";
            int results = db.ExecuteInsertUpdateDelete(q1);
            if (results > 0)
            {
                return Content("<script>alert('Thanks for contacting with us..'); location.href='/admin/SubmitedTask'</script>");
            }
            else
            {
                return Content("<script>alert('Data Not Saved..'); location.href='/admin/SubmitedTask'</script>");
            }

        }
        public ActionResult Slider()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Slider(HttpPostedFileBase im1)
        {
            string q = "select * from tbl_slider";
            ViewBag.im = db.ExecuteSelect(q);
            if (ViewBag.im.Rows.Count > 3)
            {
                //string q1= "update tbl_slider set @ViewBag.im.Rows[0][0]='"+im1.FileName+"'";
                //int res =db.ExecuteInsertUpdateDelete(q1);
                //if(res>0)
                //{
                //    im1.SaveAs(Server.MapPath("/Content/slider/") + im1.FileName);
                //    return Content("<script>alert('image update..'); location.href='/admin/slider'</script>");
                //}
                //else
                //{
                //    return Content("<script>alert('Data Not update..'); location.href='/admin/slider'</script>");
                //}
            }
            else
            {
                string q2 = " insert into tbl_slider values('" + im1.FileName + "')";
                int result = db.ExecuteInsertUpdateDelete(q2);
                if (result > 0)
                {
                    im1.SaveAs(Server.MapPath("/Content/slider/") + im1.FileName);
                    return Content("<script>alert('image insert..'); location.href='/admin/slider'</script>");
                }
                else
                {
                    return Content("<script>alert('Data Not Insert..'); location.href='/admin/slider'</script>");
                }
            }
            return View();
        }
        public ActionResult Studentmgmt()
        {
            ViewBag.regstudent = db.ExecuteSelect("select * from tbl_student order by name ");
            return View();
        }
        public ActionResult Devloper()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Devloper(string name, HttpPostedFileBase img)
        {
            string q1 = "insert into tbl_dev values('" + name + "','" + img.FileName + "') ";

            int res = db.ExecuteInsertUpdateDelete(q1);
            if (res > 0)
            {
                img.SaveAs(Server.MapPath("/Content/devimg/") + img.FileName);
                return Content("<script>alert('insert data..'); location.href='/admin/devloper'</script>");
            }
            else
            {
                return Content("<script>alert('not insert data..'); location.href='/admin/devloper'</script>");
            }
        }
        public ActionResult DevloperDet()
        {
            ViewBag.dev = db.ExecuteSelect("select * from tbl_dev");
            return View();
        }
        [HttpPost]
        public ActionResult DevloperDet(int? cbatch, HttpPostedFileBase resume, string fb, string insta, string linkdin)
        {
            string q2 = "insert into tbl_devprofile values(" + cbatch + ",'" + resume.FileName + "','" + fb + "','" + linkdin + "','" + insta + "')";
            int result = db.ExecuteInsertUpdateDelete(q2);
            if (result > 0)
            {
                resume.SaveAs(Server.MapPath("/Content/resume1/") + resume.FileName);
                return Content("<script>alert('Data Insert ..'); location.href='/admin/devloperdet'</script>");
            }
            else
            {
                return Content("<script>alert('Data Not Insert..'); location.href='/admin/devloper'</script>");
            }
        }
        public ActionResult Studentfeedback(int? id)
        {
            string que = "select sr, message,tbl_sfeedback.email,date,tbl_student.name,tbl_student.mobno from tbl_sfeedback left join tbl_student on tbl_sfeedback.email=tbl_student.email order by sr desc";
            ViewBag.sfeed = db.ExecuteSelect(que);
            if (id != null)
            {
                string query1 = "delete from tbl_sfeedback where sr=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("studentfeedback");
            }
            return View();
        }
        public ActionResult UserFeedback(int? id)
        {
            string que = "select * from tbl_feedback order by sr desc";
            ViewBag.feed = db.ExecuteSelect(que);
            if (id != null)
            {
                string query1 = "delete from tbl_feedback where sr=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("userfeedback");
            }
            return View();
        }

        public ActionResult Studenthelp(int? id)
        {
            string que = "select sr,message,tbl_shelp.email,date,tbl_student.name,tbl_student.mobno from tbl_shelp left join tbl_student on tbl_shelp.email=tbl_student.email order by sr desc";
            ViewBag.shelp = db.ExecuteSelect(que);
            if (id != null)
            {
                string query1 = "delete from tbl_shelp where sr=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("studenthelp");
            }
            return View();
        }
        public ActionResult Contactmgmt(int? id)
        {
            ViewBag.con = db.ExecuteSelect("select * from tbl_contact order by sr desc");
            if (id != null)
            {
                string query1 = "delete from tbl_contact where sr=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("contactmgmt");
            }
            return View();
        }
        public ActionResult StudyMaterial(int? id)
        {
            DataTable dt = db.ExecuteSelect("select bid,bname from tbl_batch");
            ViewBag.bac = dt;

            ViewBag.sm = db.ExecuteSelect("select * from tbl_pdf order by sr desc");
            if (id != null)
            {
                string query1 = "delete from tbl_pdf where sr=" + id + " ";
                int res = db.ExecuteInsertUpdateDelete(query1);
                return RedirectToAction("studymaterial");
            }
            return View();
        }
        [HttpPost]

        public ActionResult StudyMaterial(string title, HttpPostedFileBase pdf, int? cbatch)
        {
            string qu = "insert into tbl_pdf values('" + title + "','" + pdf.FileName + "'," + cbatch + ")";
            int res = db.ExecuteInsertUpdateDelete(qu);
            if (res > 0)
            {
                pdf.SaveAs(Server.MapPath("/Content/pdf/") + pdf.FileName);
                return Content("<script>alert('Pdf insert..'); location.href='/admin/studymaterial'</script>");
            }
            else
            {
                return Content("<script>alert('Data Not Insert..'); location.href='/admin/studymaterial'</script>");
            }

        }
        public ActionResult Mydetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Mydetails(string email, long mobno, long pin, string address, string state, string country)
        {
            string q = "select * from tbl_mydetails";
            ViewBag.im = db.ExecuteSelect(q);
            if (ViewBag.im.Rows.Count > 0)
            {
                string q1 = "update tbl_mydetails set mobno=" + mobno + ",email='" + email + "',address='" + address + "',pincode=" + pin + ",state='" + state + "',country='" + country + "' ";
                int res = db.ExecuteInsertUpdateDelete(q1);
                if (res > 0)
                {
                    return Content("<script>alert('Data Update..'); location.href='/admin/mydetails'</script>");

                }
                else
                {
                    return Content("<script>alert('Data Not Update..'); location.href='/admin/mydetails'</script>");
                }
            }
            else
            {
                string query = "insert into tbl_mydetails values(" + mobno + ",'" + email + "','" + address + "'," + pin + ",'" + state + "','" + country + "') ";
                int result = db.ExecuteInsertUpdateDelete(query);
                if (result > 0)
                {

                    return Content("<script>alert('Data Insert..'); location.href='/admin/mydetails'</script>");
                }
                else
                {
                    return Content("<script>alert('Data Not Insert..'); location.href='/admin/mydetails'</script>");
                }
            }


        }
        public ActionResult Logout()
        {
            Session.Remove("admin");
            return RedirectToAction("adminlogin", "home");
        }

    }
    class CheckSession : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["admin"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    { "Controller","Home"},
                    {"Action","adminlogin" }
                });
            }
        }
    }
}
