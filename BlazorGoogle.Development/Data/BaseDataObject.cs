using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGoogle.Development.Data
{
    public abstract class BaseDataObject
    {
        private string _ID;

        public virtual string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public virtual bool IsEmpty { get { return string.IsNullOrEmpty(_ID); } }

    }
}
