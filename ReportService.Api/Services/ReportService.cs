using ContactService.Application.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using ReportService.Api.Dto;
using ReportService.Api.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportService.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IConfiguration _configuration;

        public ReportService(IReportRepository reportRepository, IConfiguration configuration)
        {
            _reportRepository = reportRepository;
            _configuration = configuration;
        }

        public async Task GenerateReport(ReportDto reportDto)
        {
            try
            {
                using var client = new HttpClient();

                var result = await client.GetAsync(_configuration["Endpoints:ListAllContacts"]);

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //failed
                    await _reportRepository.UpdateStatus(reportDto.Id, Model.ReportStatus.FAILED);
                    return;
                }

                var contactsJson = await result.Content.ReadAsStringAsync();

                var contacts = JsonConvert.DeserializeObject<List<ContactDto>>(contactsJson);

                var locations = contacts.SelectMany(c => c.ContactDetails)
                    .Where(cd => cd.ContactDetailType == ContactDetailType.LOCATION)
                    .Select(cd => cd.ContactDetailContent?.Trim())
                    .Distinct()
                    .OrderBy(location => location);

                List<ReportData> reportData = new List<ReportData>();

                foreach (var location in locations)
                {
                    var contactCount = contacts.Where(c =>
                            c.ContactDetails.Where(cd => cd.ContactDetailType == ContactDetailType.LOCATION && cd.ContactDetailContent?.Trim() == location).Count() > 0
                        ).Count();

                    var contactPhoneNumberCount = contacts.Where(c =>
                            c.ContactDetails.Where(cd => cd.ContactDetailType == ContactDetailType.LOCATION && cd.ContactDetailContent?.Trim() == location).Count() > 0
                        )
                        .SelectMany(c => c.ContactDetails)
                        .Where(cd => cd.ContactDetailType == ContactDetailType.PHONE)
                        .Count();

                    reportData.Add(new ReportData()
                    {
                        Location = location,
                        ContactCount = contactCount,
                        ContactPhoneNumberCount = contactPhoneNumberCount
                    });
                }

                var stream = new MemoryStream();
                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("ContactReport");
                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                    namedStyle.Style.Font.UnderLine = true;
                    namedStyle.Style.Font.Color.SetColor(Color.Blue);
                    
                    var currentRow = 2;

                    //Create Headers and format them
                    worksheet.Cells["A1"].Value = "Location";
                    worksheet.Cells["B1"].Value = "Contact Count";
                    worksheet.Cells["C1"].Value = "Phone Number Count";
                    using (var r = worksheet.Cells["A1:C1"])
                    {
                        r.Style.Font.Color.SetColor(Color.White);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    }

                    foreach (var data in reportData)
                    {
                        worksheet.Cells[currentRow, 1].Value = data.Location;
                        worksheet.Cells[currentRow, 2].Value = data.ContactCount.ToString();
                        worksheet.Cells[currentRow, 3].Value = data.ContactPhoneNumberCount.ToString();

                        currentRow++;
                    }

                    xlPackage.Save();
                }
                //stream.Position = 0;

                //File file = File.(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
                string reportPath = "/reports/" + reportDto.Id.ToString() + ".xlsx";

                using (FileStream fs = File.Create(reportPath))
                {
                    byte[] fileBytes = stream.ToArray();
                    // Add some information to the file.
                    fs.Write(fileBytes, 0, fileBytes.Length);
                }

                await _reportRepository.UpdateStatus(reportDto.Id, Model.ReportStatus.COMPLETED, reportPath);
                 

            }
            catch (Exception e)
            {
                await _reportRepository.UpdateStatus(reportDto.Id, Model.ReportStatus.FAILED);
                return;
            }

        }
    }

    class ReportData
    {
        public string Location { get; set; }
        public int ContactCount { get; set; }
        public int ContactPhoneNumberCount { get; set; }
    }
}
