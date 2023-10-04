using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserScreen.Models;

namespace UserScreen.Controllers
{
    public class HomeController : Controller
    {
        DataAccess  dataAccess=new DataAccess();


        // GET: Home

        public ActionResult newfilter(int? categoryid,int?subcategoryid,int?stateid,int?cityid,int?areaid,decimal?minprice,decimal?maxprice)
        {
            List<ModelMyAdvertise> list = dataAccess.newFilter(categoryid,subcategoryid,stateid,cityid,areaid,minprice,maxprice);

            return View(list);
        }


        public ActionResult All() { 
        
            List<categoryViewModel>obj=dataAccess.all();
            return View(obj);
        
        }
        public ActionResult Index(int categoryid)
        {
            DataAccess access = new DataAccess();

            List<ModelMyAdvertise> advertises = access.GetCategoryById(categoryid);
            if (advertises != null && advertises.Count > 0)
            {
                return View(advertises);
            }
           
            

            return View();
        }

        public ActionResult ShowByArea(int areaid) 
        {
            List<ModelMyAdvertise>Area=dataAccess.GetByArea(areaid);
            if (Area.Count>0)
            {
                return View(Area);
            }
            return View();
        }
        public ActionResult ShowBySubCategory(int subCategoryId)
        { 
            DataAccess dataAccess = new DataAccess();
            List<ModelMyAdvertise>list=dataAccess.GetProductBySubcategory(subCategoryId);
            if (list!=null)
            {
                return View(list);
            }

            return View();
        
        }

        public ActionResult ShowByState(int stateId)
        {
            DataAccess dataAccess = new DataAccess();
            List<ModelMyAdvertise> list = dataAccess.GetProductByState(stateId);
            if (list != null)
            {
                return View(list);
            }

            return View();

        }

        public ActionResult ShowByCity(int cityid)
        {
            DataAccess dataAccess = new DataAccess();
            List<ModelMyAdvertise>citylist=dataAccess.GetProductByCity(cityid);
            if (citylist!=null)
            {
                return View(citylist);
            }

            return View();
        }
        public ActionResult ShowByPrice(decimal min,decimal max)
        {
            DataAccess dataAccess=new DataAccess();
            List<ModelMyAdvertise>price=dataAccess.GetByPrice(min,max);
            if (price!=null)
            {
                return View(price);
            }
            return View();
        }
        public ActionResult ShowAllProducts() 
        {
            List<ModelMyAdvertise> all = dataAccess.GetAllProducts();
            if(all!=null)
            {
                return View(all);
            }
            return View();
        }

        public ActionResult Showsubcategorydetails(int procategoryid)
        {
            DataAccess dataAccess = new DataAccess();
            List<ModelProductSubCategory>sublist=dataAccess.GetSubByCategoryId(procategoryid);
            if (sublist!=null)
            {
                return View(sublist);
            }
            return View();

        }


    }
}