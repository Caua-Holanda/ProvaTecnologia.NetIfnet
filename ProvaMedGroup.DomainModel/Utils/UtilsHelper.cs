using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProvaMedGroup.DomainModel.Utils
{
    public static class UtilsHelper
    {
        public static bool MinimumAge(int age, DateTime niver)
        {
            DateTime minimumDate = DateTime.Now.AddYears(-age);
            return niver <= minimumDate;
        }

        public static int CalcularIdade(DateTime dataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;
            if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }

    }
}
