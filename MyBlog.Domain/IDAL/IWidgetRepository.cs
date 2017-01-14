using MyBlog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.IDAL
{
    public interface IWidgetRepository
    {
         IEnumerable<Widget> WidgetIEnum { get; }
        Widget GetSetting { get; }
        void Save(Widget widget);
        Widget Details(int? Id);
    }
}
