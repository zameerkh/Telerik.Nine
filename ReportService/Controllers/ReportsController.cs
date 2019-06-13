using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web;
using Domain.Models;
using ReportService.ReportTemplate;
using Telerik.Reporting;
using Telerik.Reporting.Services.Engine;
using Telerik.Reporting.Services.WebApi;
namespace ReportService.Controllers
{


    public class ReportsController : ReportsControllerBase
    {
        private static Telerik.Reporting.Services.ReportServiceConfiguration configurationInstance =
            new Telerik.Reporting.Services.ReportServiceConfiguration
            {
                HostAppId = "Application1",
                ReportResolver = new ReportFileResolver(HttpContext.Current.Server.MapPath("~/Reports1"))
                    .AddFallbackResolver(new CustomReportResolver()),
                Storage = new Telerik.Reporting.Cache.File.FileStorage(),
            };

        public ReportsController()
        {
            ReportServiceConfiguration = configurationInstance;
        }


        protected override HttpStatusCode SendMailMessage(MailMessage mailMessage)
        {
            using (var smtpClient = new SmtpClient("smtp.companyname.com", 25))
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            return HttpStatusCode.OK;
        }

        protected override IReportResolver CreateReportResolver()
        {
            return new CustomReportResolver();
        }

    }

    public class CustomReportResolver : IReportResolver
    {
        public ReportSource Resolve(string reportName)
        {
            var objectDataSource = new ObjectDataSource();
            objectDataSource.DataSource = GetData();

            var report = new Report1();
            report.DataSource = objectDataSource;// you can use also GetData() directly since it is a List

            var reportSource = new InstanceReportSource();
            reportSource.ReportDocument = report;

            return reportSource;
        }

        public static IList<Employee> GetData()
        {
            return new List<Employee>()

            {
                new Employee() {Name = "Zameer"},
                new Employee() {Name = "Bilal"}
            };

        }
    }
}
