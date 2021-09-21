using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewSample.Interfaces
{
    public interface IObserver
    {
        public void update(ISubject subject);
    }
}
