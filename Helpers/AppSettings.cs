using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeWebsiteApi.Helpers
{
    public class AppSettings : IAppSetting
    {
        public string Secret { get; set; }
    }

    public interface IAppSetting
    {
        string Secret { get; set; }
    }
}
