using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Utils
{
    // Status của Order
    public enum OrderStatus
    {
        PROCESSING = 1,
        DELIVERING = 2,
        COMPLETED = 3,
        CANCELED = 4
    }
    // Status của Comment
    public enum CommentStatus
    {
        SHOWING = 1,
        WARNING = 2,
        HIDDEN = 3,
        APPROVED = 4,
        DELETED = 5
    }
    // Mode của Comment
    public enum CommentMode
    {
        DEFAULT = 1,
        AUTOHIDE = 2
    }

    public enum ReplyMode
    {
        MANUAL = 1,
        AUTO = 2,
        COMMENT_ONLY = 3,
        MESSAGE_ONLY = 4
    }
}