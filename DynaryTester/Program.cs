using System.IO;
using System.Text;
using Dynary.IO;
using Dynary;

namespace DynaryTester
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var fs = new FileStream("data.dat", FileMode.Create))
            {
                using (var dw = new DynaryWriter(fs, Encoding.Unicode))
                {
                    dw.WriteEnumerable(new []
                    {
                        new Pair("key01", "value01"),
                        new Pair("key02", "value02"),  
                    });
                    dw.EndStream();
                }
            }

            using (var fs = new FileStream("data.dat", FileMode.Open))
            {
                using (var dr = new DynaryReader(fs, Encoding.Unicode))
                {
                    var list = dr.ReadToEnd();
                }
            }
        }
    }
}
