﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Application.Dto
{
    public class ReportRelatorioResultDto
    {
        public ReportRelatorioResultDto()
        {
            profits = new List<ProfitDto>();
        }
        public string consultant { get; set; }
        public List<ProfitDto> profits { get; set; }
    }
}
