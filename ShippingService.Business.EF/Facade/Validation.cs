using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade
{
    [Serializable]
    public class Validation
    {
        protected List<string> brokenRules;
        protected List<string> warnings;


        public Validation()
        {
            brokenRules = new List<string>();
            warnings = new List<string>();
        }

        public void Add(Validation v)
        {
            if (v == null)
                return;

            foreach (string warning in v.Warnings)
                this.warnings.Add(warning);

            foreach (string brokenRule in v.BrokenRules)
                this.brokenRules.Add(brokenRule);
        }

        public bool IsValid
        {
            get { return brokenRules.Count == 0; }
        }

        public bool HasWarnings
        {
            get { return warnings.Count > 0; }
        }

        public void AddBrokenRule(string rule)
        {
            brokenRules.Add(rule);
        }

        public void AddWarning(string warning)
        {
            warnings.Add(warning);
        }

        public string[] BrokenRules
        {
            get { return brokenRules.ToArray(); }
        }

        public string[] Warnings
        {
            get { return warnings.ToArray(); }
        }
    }
}
