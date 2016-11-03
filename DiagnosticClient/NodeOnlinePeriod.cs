﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace DiagnosticClient
{
    public class NodeOnlinePeriod
    {
        public NodeOnlinePeriod()
        {

        }
        public int Id { get; set; }

        public int NodeId { get; set; }
        public Node Node { get; set; }

        [Required]
        public DateTime TimeStart { get; set; }
        [Required]
        public DateTime TimeEnd { get; set; }
    }
}