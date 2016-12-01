using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pingis.DAL
{
    public class DALBase
    {
        public CloudTableClient tableClient = null;

        public DALBase()
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

            this.tableClient = storageAccount.CreateCloudTableClient();
        }
    }
}
