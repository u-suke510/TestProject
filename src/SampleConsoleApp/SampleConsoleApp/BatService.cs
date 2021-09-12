using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleConsoleApp
{
    public interface IBatService
    {
    }

    public class BatService : IBatService
    {
        private ILogger<BatService> logger;

        public BatService(ILogger<BatService> logger)
        {
            this.logger = logger;
        }
    }
}
