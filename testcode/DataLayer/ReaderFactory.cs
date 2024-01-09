using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReaderFactory
    {
        public IDataReader GetReader(string path)
        {
            IDataReader reader = null;
            switch (path.Split('.').Last())
            {
                case "json":
                    reader = new JsonDataReader();
                    break;
                default:
                    throw new ArgumentException("Unknown file extension");
            }
            return reader;
        }
    }
}
