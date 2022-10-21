using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileTaggingService
{
    public partial class Service1 : ServiceBase
    {
        private ServiceHost serviceHost = null;
        private Uri serviceUri = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
            serviceUri = new Uri("http://localhost:8000");
            serviceHost = new ServiceHost(typeof(FileTagGetter), serviceUri);
            serviceHost.AddServiceEndpoint(typeof(IFileTagGetter), new BasicHttpBinding(), "GetFileTag");
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
