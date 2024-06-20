using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface IEmailConfigurationRepository
    {
        EmailConfigurationSetting GetAllEmailConfiguration(EmailConfigurationSetting cls);
        EmailConfigurationSetting addEmailConfiguration(EmailConfigurationSetting cls);

        EmailConfigurationSetting GetEmailConfiguration(EmailConfigurationSetting cls, int? Id);
        EmailConfigurationSetting FillTeacherList(EmailConfigurationSetting cls);

        EmailConfigurationSetting deleteEmailConfiguration(EmailConfigurationSetting cls);
        DataTable ExportEmailConfiguration(EmailConfigurationSetting cls);
    }
}
