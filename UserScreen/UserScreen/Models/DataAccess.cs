using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using UserScreen.Models;

namespace UserScreen.Models
{
    public class DataAccess
    {
        private readonly SqlConnection _connection;
        public DataAccess()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

        }

        #region BYPRICE
        public List<ModelMyAdvertise> GetByPrice(decimal min,decimal max)
        {
            List<ModelMyAdvertise>price = new List<ModelMyAdvertise>();
            string query = "select ad.advertiseTitle,ad.advertiseDescription,ad.advertisePrice,ar.areaName,ct.cityName,st.stateName" +
                " from tbl_MyAdvertise ad\r\n" +
                "join tbl_Area ar on ad.areaId=ar.areaId\r\n" +
                "join tbl_City ct on ar.cityId=ct.cityId\r\n" +
                "join tbl_State st on ct.stateId=st.stateId\r\nwhere advertisePrice>=@minprice and advertisePrice<=@maxprice"
                ;
            SqlCommand cmd=new SqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@minprice",min);
            cmd.Parameters.AddWithValue("@maxprice",max);
            _connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ModelMyAdvertise byprice = new ModelMyAdvertise()
                {
                    advertiseTitle = reader["advertiseTitle"].ToString(),
                    advertiseDescription = reader["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToInt32(reader["advertisePrice"]),
                    cityName = reader["cityName"].ToString()

            };
                price.Add(byprice);


        }
            _connection.Close();
            return price;

        }
        #endregion


        #region ByCity
        public List<ModelMyAdvertise> GetProductByCity(int cityid)
        {
            List<ModelMyAdvertise>city=new List<ModelMyAdvertise>();
            //string query = "select ad.advertiseTitle,ad.advertiseDescription,ad.advertisePrice,ar.areaName,ct.cityName,st.stateName" +
            //    " from tbl_MyAdvertise ad\r\n" +
            //    "join tbl_Area ar on ad.areaId=ar.areaId\r\n" +
            //    "join tbl_City ct on ar.cityId=ct.cityId\r\n" +
            //    "join tbl_State st on ct.stateId=st.stateId\r\nwhere ct.cityId=@cityid";
            SqlCommand cmd= new SqlCommand("GetByCity", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.AddWithValue("@cityid",cityid);
            _connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ModelMyAdvertise bycity = new ModelMyAdvertise()
                {
                    advertiseTitle = reader["advertiseTitle"].ToString(),
                    advertiseDescription = reader["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToInt32(reader["advertisePrice"]),
                    areaName = reader["areaName"].ToString(),
                    stateName = reader["stateName"].ToString(),
                    cityName = reader["cityName"].ToString()
                };
                city.Add(bycity);
            }
            _connection.Close();
            return city;
        }
        #endregion


        #region Bystate
        public List<ModelMyAdvertise> GetProductByState(int stateid)
        {
            List<ModelMyAdvertise>state=new List<ModelMyAdvertise>();
            //string query = "select ad.advertiseTitle,ad.advertiseDescription,ad.advertisePrice,ar.areaName,ct.cityName,st.stateName" +
            //    " from tbl_MyAdvertise ad\r\njoin tbl_Area ar on ad.areaId=ar.areaId\r\n" +
            //    "join tbl_City ct on ar.cityId=ct.cityId\r\njoin tbl_State st on ct.stateId=st.stateId\r\nwhere st.stateId=@stateid";
            SqlCommand cmd=new SqlCommand("GetByState", _connection);
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@stateid", stateid);
            _connection.Open();
           
            SqlDataReader dr=cmd.ExecuteReader();
            while (dr.Read())
            {
                ModelMyAdvertise modelMyAdvertise = new ModelMyAdvertise()
                {
                    advertiseTitle = dr["advertiseTitle"].ToString(),
                    advertiseDescription = dr["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToInt32(dr["advertisePrice"]),
                    stateName = dr["stateName"].ToString(),
                    cityName = dr["cityName"].ToString()
                };
                  
                state.Add(modelMyAdvertise);    
               

            }
            _connection.Close();
            return state;
        }
        #endregion

        #region ByArea
        public List<ModelMyAdvertise> GetByArea(int areaid)
        {
            List<ModelMyAdvertise>area=new List<ModelMyAdvertise>();
            //string query = "\tselect ad.advertiseTitle,ad.advertiseDescription,ad.advertisePrice,ar.areaName,ct.cityName,st.stateName\r\nfrom tbl_MyAdvertise ad\r\n" +
            //    "join tbl_Area ar on ad.areaId=ar.areaId \r\n" +
            //    "join tbl_City ct on ar.cityId=ct.cityId\r\n" +
            //    "join tbl_State st on ct.stateId=st.stateId where ar.areaId=@areaid";
            SqlCommand cmd=new SqlCommand("GetByArea", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@areaid", areaid);
            _connection.Open();
           
           
                 SqlDataReader r= cmd.ExecuteReader();
            while (r.Read())
            {
                ModelMyAdvertise byarea = new ModelMyAdvertise()
                {
                    advertiseTitle = r["advertiseTitle"].ToString(),
                    advertiseDescription = r["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToInt32(r["advertisePrice"]),
                    areaName = r["areaName"].ToString(),
                    cityName = r["cityName"].ToString(),
                    stateName = r["stateName"].ToString(),

                };
                area.Add(byarea);
            }
            _connection.Close ();
            return area;
        } 

        #endregion

        #region BYSUBcategory
        public List<ModelMyAdvertise>GetProductBySubcategory(int subCategoryId)
        {
            List<ModelMyAdvertise>subcategory = new List<ModelMyAdvertise>();
            string query = "  select ad.advertiseTitle,ad.advertiseDescription,ad.advertisePrice from tbl_MyAdvertise ad" +
                "\r\n  join tbl_ProductSubCategory ps on ad.productSubCategoryId=ps.productSubCategoryId" +
                " \r\n  where ps.productSubCategoryId=@subcategory";
            SqlCommand cmd= new SqlCommand(query, _connection); 
            _connection.Open();
            cmd.Parameters.AddWithValue("@subcategory", subCategoryId);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ModelMyAdvertise advertise = new ModelMyAdvertise()
                {
                    advertiseTitle = dr["advertiseTitle"].ToString(),
                    advertiseDescription = dr["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToInt32(dr["advertisePrice"])
                };
                subcategory.Add(advertise);
            }
            _connection.Close();
            return subcategory;
        }
        #endregion


        #region ByCategory
        public List<ModelMyAdvertise>GetCategoryById(int productCategoryId)
        {
           // SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            List<ModelMyAdvertise> list = new List<ModelMyAdvertise>();
            string query = "select ad.advertiseTitle,ad.advertiseDescription,ad.advertisePrice from tbl_MyAdvertise ad " +
                "join tbl_ProductSubCategory ps on ad.productSubCategoryId=ps.productSubCategoryId" +
                " where ps.productCategoryId=@productCategoryId";

            SqlCommand cmd = new SqlCommand(query,_connection);
            _connection.Open();
            cmd.Parameters.AddWithValue("@productCategoryId", productCategoryId);
            
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ModelMyAdvertise modelMyAdvertise = new ModelMyAdvertise()
                {

                    advertiseTitle = reader["advertiseTitle"].ToString(),
                    advertiseDescription = reader["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToInt32(reader["advertisePrice"])
                };
               
               list.Add(modelMyAdvertise);
            }
            _connection.Close();
            return list;
        }
        #endregion

        #region AllProducts

        public List<ModelMyAdvertise>GetAllProducts()
        {
            List<ModelMyAdvertise>allprod = new List<ModelMyAdvertise>();
            
            SqlCommand cmd=new SqlCommand("allproducts", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            SqlDataReader reader=cmd.ExecuteReader();
            while (reader.Read())
            {
                ModelMyAdvertise modelMyAdvertise = new ModelMyAdvertise()
                {
                    advertiseTitle = reader["advertiseTitle"].ToString(),
                    advertiseDescription = reader["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToInt32(reader["advertisePrice"]),
                    areaName = reader["areaName"].ToString(),
                    cityName = reader["cityName"].ToString(),
                    stateName = reader["stateName"].ToString(),

                };
                allprod.Add(modelMyAdvertise);

            }
            _connection.Close();
            return allprod;

        }
        #endregion

        #region FILTER
        public List<ModelMyAdvertise> GetByFilter(int categoryid,int subcategoryid,int stateid,int cityid,int areaid,
            decimal minprice,decimal maxprice)
        { 
            List<ModelMyAdvertise>modelMyAdvertises = new List<ModelMyAdvertise>();
            SqlCommand cmd = new SqlCommand("filters", _connection);
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productCategoryId", categoryid);
            cmd.Parameters.AddWithValue("@productSubCategoryId", subcategoryid);
            cmd.Parameters.AddWithValue("@stateId", stateid);
            cmd.Parameters.AddWithValue("@cityId", cityid);
            cmd.Parameters.AddWithValue("@areaId", areaid);
            cmd.Parameters.AddWithValue("@minprice", minprice);
            cmd.Parameters.AddWithValue("@maxprice", maxprice);
            _connection.Open();
            SqlDataReader reader=cmd.ExecuteReader();
            while (reader.Read())
            {
                ModelMyAdvertise model = new ModelMyAdvertise()
                {
                    advertiseTitle = reader["advertiseTitle"].ToString(),
                    advertiseDescription = reader["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToDecimal(reader["advertisePrice"]),
                    areaName = reader["areaName"].ToString(),
                    cityName = reader["cityName"].ToString(),
                    stateName = reader["stateName"].ToString()

                };
                modelMyAdvertises.Add(model);
            }
            _connection.Close();
            return modelMyAdvertises;

        }

        #endregion

        #region subcategorydetail
        public List<ModelProductSubCategory> GetSubByCategoryId(int procategoryid)
        {
            List<ModelProductSubCategory>sub=new List<ModelProductSubCategory>();
            SqlCommand cmd = new SqlCommand("subcategorydetails", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productcategoryid", procategoryid);
            _connection.Open();
            SqlDataReader reader=cmd.ExecuteReader();
            while (reader.Read())
            {
                ModelProductSubCategory model = new ModelProductSubCategory()
                {
                    productSubCategoryId = Convert.ToInt32(reader["productSubCategoryId"]),
                    productSubCategoryName = reader["productSubCategoryName"].ToString() 
                };
                sub.Add(model);
            }
            _connection.Close();
            return sub;
        }

        #endregion

        public List<categoryViewModel> all()
        {
            List<categoryViewModel>list= new List<categoryViewModel>();
            SqlCommand cmd = new SqlCommand("allcategories", _connection);
            cmd.CommandType= CommandType.StoredProcedure;
            _connection.Open();
            SqlDataReader reader=cmd.ExecuteReader();
            while (reader.Read())
            {
                categoryViewModel categoryView = new categoryViewModel()
                {
                    category_id = Convert.ToInt32(reader["category_id"]),
                    category_name = reader["category_name"].ToString(),
                    subcategories = reader["subcategories"].ToString()

                };
                list.Add(categoryView);
            }
            _connection.Close();
            return list;
        }
        public List<ModelMyAdvertise> newFilter(int? productCategoryId,int? productSubCategoryId,int? stateId,int? cityid,int? areaid,decimal? minprice,decimal? maxprice )
        {
            List<ModelMyAdvertise>models= new List<ModelMyAdvertise>();
            SqlCommand cmd = new SqlCommand("newfilter", _connection);
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productCategoryId",productCategoryId);
            cmd.Parameters.AddWithValue("@productsubCategoryId",productSubCategoryId);
            cmd.Parameters.AddWithValue("@stateid",stateId);
            cmd.Parameters.AddWithValue("@cityid",cityid);
            cmd.Parameters.AddWithValue("@areaid",areaid);
            cmd.Parameters.AddWithValue("@minprice",minprice);
            cmd.Parameters.AddWithValue("@maxprice",maxprice);
            _connection.Open();
            SqlDataReader reader= cmd.ExecuteReader();
            while(reader.Read())
            {
                ModelMyAdvertise model = new ModelMyAdvertise()
                {
                    advertiseTitle = reader["advertiseTitle"].ToString(),
                    advertiseDescription = reader["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToDecimal(reader["advertisePrice"]),
                    cityName = reader["cityName"].ToString(),
                    stateName = reader["statename"].ToString(),

                };
                models.Add(model);
            }
            _connection.Close();
            return models;
            
            

        }

        public List<ModelProductSubCategory> GetCategoryWithSubcategories()
        {
            List<ModelProductSubCategory> models=new List<ModelProductSubCategory>();
            
            string query = "\tselect pc.productCategoryId,pc.productCategoryName,sb.productSubCategoryId,sb.productSubCategoryName from tbl_ProductSubCategory sb\r\n\t\t\t\tjoin\r\n\t\t\t\ttbl_ProductCategory pc on pc.productCategoryId=sb.productCategoryId";
               
            SqlCommand cmd= new SqlCommand(query, _connection);
            _connection.Open();
            SqlDataReader reader= cmd.ExecuteReader();
            
            while (reader.Read())
            {
                ModelProductSubCategory subCategory = new ModelProductSubCategory()
                {
                    productCategoryId = Convert.ToInt32(reader["productCategoryId"]),
                    productCategoryName = reader["productCategoryName"].ToString(),
                    productSubCategoryId = Convert.ToInt32(reader["productSubCategoryId"]), 
                    productSubCategoryName = reader["productSubCategoryName"].ToString(),
                };
                models.Add(subCategory);
            }
            _connection.Close(); return models;
        }

        //public List<CategoryWithSubcategoriesViewModel> GetCategoriesWithSubcategories()
        //{
        //    List<CategoryWithSubcategoriesViewModel> categoriesWithSubcategories = new List<CategoryWithSubcategoriesViewModel>();

            
        //        _connection.Open();

        //        string query = @"
        //    SELECT
        //        pc.productCategoryId AS CategoryId,
        //        pc.productCategoryName AS CategoryName,
        //        sb.productSubCategoryId AS SubcategoryId,
        //        sb.productSubCategoryName AS SubcategoryName
        //    FROM
        //        tbl_ProductCategory pc
        //    LEFT JOIN
        //        tbl_ProductSubCategory sb
        //    ON
        //        pc.productCategoryId = sb.productCategoryId
        //    ORDER BY
        //        pc.productCategoryId, sb.productSubCategoryId";

        //        using (SqlCommand command = new SqlCommand(query, _connection))
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            CategoryWithSubcategoriesViewModel currentCategory = null;

        //            while (reader.Read())
        //            {
        //                int categoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));
        //                string categoryName = reader.GetString(reader.GetOrdinal("CategoryName"));
        //                int subcategoryId = reader.GetInt32(reader.GetOrdinal("SubcategoryId"));
        //                string subcategoryName = reader.GetString(reader.GetOrdinal("SubcategoryName"));

        //                if (currentCategory == null || currentCategory.CategoryId != categoryId)
        //                {
        //                    // Start a new category
        //                    currentCategory = new CategoryWithSubcategoriesViewModel
        //                    {
        //                        CategoryId = categoryId,
        //                        CategoryName = categoryName,
        //                        Subcategories = new List<SubcategoryViewModel>()
        //                    };

        //                    categoriesWithSubcategories.Add(currentCategory);
        //                }

        //                if (subcategoryId != 0) // Ensure there's a valid subcategory
        //                {
        //                    currentCategory.Subcategories.Add(new SubcategoryViewModel
        //                    {
        //                        SubcategoryId = subcategoryId,
        //                        SubcategoryName = subcategoryName
        //                    });
        //                }
        //            }
        //        }
            
        //        _connection.Close();
        //    return categoriesWithSubcategories;
        //}

    }
}