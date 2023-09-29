using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using UserScreen.Models;

namespace UserScreen.Models
{
    public class DataAccess
    {
        private SqlConnection _connection;
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
            string query = "select ad.advertiseTitle,ad.advertiseDescription,ad.advertisePrice,ar.areaName,ct.cityName,st.stateName" +
                " from tbl_MyAdvertise ad\r\n" +
                "join tbl_Area ar on ad.areaId=ar.areaId\r\n" +
                "join tbl_City ct on ar.cityId=ct.cityId\r\n" +
                "join tbl_State st on ct.stateId=st.stateId\r\nwhere ct.cityId=@cityid";
            SqlCommand cmd= new SqlCommand(query, _connection);
            _connection.Open();
            cmd.Parameters.AddWithValue("@cityid",cityid);
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
            string query = "select ad.advertiseTitle,ad.advertiseDescription,ad.advertisePrice,ar.areaName,ct.cityName,st.stateName" +
                " from tbl_MyAdvertise ad\r\njoin tbl_Area ar on ad.areaId=ar.areaId\r\n" +
                "join tbl_City ct on ar.cityId=ct.cityId\r\njoin tbl_State st on ct.stateId=st.stateId\r\nwhere st.stateId=@stateid";
            SqlCommand cmd=new SqlCommand(query, _connection);
            _connection.Open();
            cmd.Parameters.AddWithValue("@stateid", stateid);
            SqlDataReader dr=   cmd.ExecuteReader();
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

    }
}