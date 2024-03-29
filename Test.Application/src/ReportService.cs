﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Dto;
using Test.Application.Services;
using Test.Domain.Models;
using Test.Domain.Repository;

namespace Test.Application.src
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _uow;
        public ReportService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public IEnumerable<ReportResultDto> GetReport(ReportFormDto model)
        {
            List<ReportResultDto> list = new List<ReportResultDto>();
            foreach (var item in model.consultants)
            {
                CaoUsuario user = _uow.UsuarioRepository.Queryable().Where(d => d.NoUsuario == item).First();
               
                ReportResultDto consultant = new ReportResultDto();
                consultant.consultant = user.NoUsuario;

                DateTime date = model.startDate;
                while(date <= model.endDate)
                {
                    ProfitDto profit = new ProfitDto();
                    profit.period = getPeriod(date.Month, date.Year);
                    var query = from f in _uow.FacturaRepository.Queryable()
                                join os in _uow.OSRepository.Queryable()
                                on f.CoOs equals os.CoOs
                                where os.CoUsuario == user.CoUsuario && f.DataEmissao.Month == date.Month && f.DataEmissao.Year == date.Year select f;

                    profit.receitaLiquida = query.Sum(d => d.Valor - (d.TotalImpInc/100*d.Valor));
                    if(_uow.SalarioRepository.Queryable().Where(d => d.CoUsuario == user.CoUsuario).Any())
                    {
                        profit.custoFixo = _uow.SalarioRepository.Queryable().Where(d => d.CoUsuario == user.CoUsuario).First().BrutSalario;
                    }
                    else
                    {
                        profit.custoFixo = 0;
                    }
                    profit.commissao = query.Sum(d => (d.Valor - (d.TotalImpInc / 100 * d.Valor))* d.ComissaoCn/100);
                    profit.lucro = profit.receitaLiquida - (profit.commissao + profit.custoFixo);
                    consultant.profits.Add(profit);

                    date = date.AddMonths(1);
                }
                list.Add(consultant);
            }
            return list;
        }

        private string getPeriod(int month, int year)
        {
            string result = "";
            switch (month)
            {
                case 1:
                    result = "Janeiro de "+ year;
                    break;
                case 2:
                    result = "Fevereiro de " + year;
                    break;
                case 3:
                    result = "Março de " + year;
                    break;
                case 4:
                    result = "Abril de " + year;
                    break;
                case 5:
                    result = "Maio de" + year;
                    break;
                case 6:
                    result = "Junho de" + year;
                    break;
                case 7:
                    result = "Julho de" + year;
                    break;
                case 8:
                    result = "Agosto de" + year;
                    break;
                case 9:
                    result = "Setembro de" + year;
                    break;
                case 10:
                    result = "Outubro de" + year;
                    break;
                case 11:
                    result = "Novembro de" + year;
                    break;
                case 12:
                    result = "Dezembro de" + year;
                    break;
                default:
                    break;
            }

            return result;
        }

    }
}
