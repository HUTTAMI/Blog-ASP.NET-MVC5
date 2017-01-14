﻿using MyBlog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.IDAL
{
    public interface ICommentRepository
    {
        void Save(Comment commnet);
        IEnumerable<Comment> CommentIEnum { get; }
        IEnumerable<Comment> Last10Comment { get; }

        Comment Details(int? Id);
        List<Comment> CommentList { get; }

        Comment Delete(int? Id);
    }
}
