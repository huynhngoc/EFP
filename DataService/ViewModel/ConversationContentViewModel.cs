﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class ConversationContentViewModel
    {
        public List<MessageContentViewModel> Messages { get; set; }
        public string NextUrl { get; set; }

        public ConversationContentViewModel()
        {
            Messages = new List<MessageContentViewModel>();
            NextUrl = "null";
        }
    }

    public class MessageContentViewModel
    {
        public string MessId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public string MessContent { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }

        public MessageContentViewModel()
        {
            Attachments = new List<AttachmentViewModel>();
        }

    }
}
