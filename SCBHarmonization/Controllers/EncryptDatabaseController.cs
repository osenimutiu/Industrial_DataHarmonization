using SCBHarmonization.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml;

namespace SCBHarmonization.Controllers
{
    public class EncryptDatabaseController : Controller
    {
        // GET: EncryptDatabase
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PersonModel personModel)
        {
            //DecryptConnString();
            EncryptEntity(personModel);
            Encrypttwo(personModel);
            string name = "DefaultConnection";
            bool isNew = false;
            string path = Server.MapPath("~/Web.Config");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList list = doc.DocumentElement.SelectNodes(string.Format("connectionStrings/add[@name='{0}']", name));
            XmlNode node;
            isNew = list.Count == 0;
            if (isNew)
            {
                node = doc.CreateNode(XmlNodeType.Element, "add", null);
                XmlAttribute attribute = doc.CreateAttribute("name");
                attribute.Value = name;
                node.Attributes.Append(attribute);

                attribute = doc.CreateAttribute("connectionString");
                attribute.Value = "";
                node.Attributes.Append(attribute);

                attribute = doc.CreateAttribute("providerName");
                attribute.Value = "System.Data.SqlClient";
                node.Attributes.Append(attribute);
            }
            else
            {
                node = list[0];
            }
            string conString = node.Attributes["connectionString"].Value;
            SqlConnectionStringBuilder conStringBuilder = new SqlConnectionStringBuilder(conString);
            conStringBuilder.InitialCatalog = personModel.Database;
            conStringBuilder.DataSource = personModel.Server;
            conStringBuilder.IntegratedSecurity = false;
            conStringBuilder.UserID = personModel.User;
            conStringBuilder.Password = personModel.Password;
            node.Attributes["connectionString"].Value = conStringBuilder.ConnectionString;
            if (isNew)
            {
                doc.DocumentElement.SelectNodes("connectionStrings")[0].AppendChild(node);
            }
            doc.Save(path);
            EncryptConnString();
            return View();
        }
        public void EncryptEntity(PersonModel personModel)
        {
            string name = "SCBDBEntities";
            bool isNew = false;
            string path = Server.MapPath("~/Web.Config");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList list = doc.DocumentElement.SelectNodes(string.Format("connectionStrings/add[@name='{0}']", name));
            XmlNode node;
            isNew = list.Count == 0;
            node = list[0];

        
            // Specify the provider name, server and database.
            string providerName = "System.Data.SqlClient";
            string serverName = personModel.Server;
            string databaseName = personModel.Database;

            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = true;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            //entityBuilder.Metadata = @"res://*/AdventureWorksModel.csdl|
            //                res://*/AdventureWorksModel.ssdl|
            //                res://*/AdventureWorksModel.msl";
            entityBuilder.Metadata = @"res://*/Models.SCBModel.csdl|res://*/Models.SCBModel.ssdl|res://*/Models.SCBModel.msl";
            //Console.WriteLine(entityBuilder.ToString());
            node.Attributes["connectionString"].Value = entityBuilder.ToString();
            if (isNew)
            {
                doc.DocumentElement.SelectNodes("connectionStrings")[0].AppendChild(node);
            }
            doc.Save(path);
        }

        public void Encrypttwo(PersonModel personModel)
        {
            string name = "elmah-sqlserver";
            bool isNew = false;
            string path = Server.MapPath("~/Web.Config");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList list = doc.DocumentElement.SelectNodes(string.Format("connectionStrings/add[@name='{0}']", name));
            XmlNode node;
            isNew = list.Count == 0;
            if (isNew)
            {
                node = doc.CreateNode(XmlNodeType.Element, "add", null);
                XmlAttribute attribute = doc.CreateAttribute("name");
                attribute.Value = name;
                node.Attributes.Append(attribute);

                attribute = doc.CreateAttribute("connectionString");
                attribute.Value = "";
                node.Attributes.Append(attribute);

                attribute = doc.CreateAttribute("providerName");
                attribute.Value = "System.Data.SqlClient";
                node.Attributes.Append(attribute);
            }
            else
            {
                node = list[0];
            }
            string conString = node.Attributes["connectionString"].Value;
            SqlConnectionStringBuilder conStringBuilder = new SqlConnectionStringBuilder(conString);
            conStringBuilder.InitialCatalog = personModel.Database;
            conStringBuilder.DataSource = personModel.Server;
            conStringBuilder.IntegratedSecurity = false;
            conStringBuilder.UserID = personModel.User;
            conStringBuilder.Password = personModel.Password;
            node.Attributes["connectionString"].Value = conStringBuilder.ConnectionString;
            if (isNew)
            {
                doc.DocumentElement.SelectNodes("connectionStrings")[0].AppendChild(node);
            }
            doc.Save(path);
        }

        public static void EncryptConnString()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection section = config.GetSection("connectionStrings");

            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }
        }

        public static void DecryptConnString()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }
        }
    }
}