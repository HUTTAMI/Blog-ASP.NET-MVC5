﻿using MyBlog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.IDAL
{
  public  interface IPageRepository
    {
        void Save(Page page);
        IEnumerable<Page> PageIEnum { get; }
        List<Page> PageList { get; }
        Page Details(int? Id);
       
    }
}
