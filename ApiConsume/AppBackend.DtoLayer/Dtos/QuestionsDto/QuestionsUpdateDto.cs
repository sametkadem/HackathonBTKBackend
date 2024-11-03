﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionsUpdateDto
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public bool OnlyLastAnswer { get; set; } = false;
    }
}
