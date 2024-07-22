using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory_Management_System.Test
{
    internal class TestManager
    {
        public String[] testNames = {"FBS","T CHO" ,"TG" ,"S CR" ,"ALT" ,"AST" ,"T PROTEIN" ,"ALBUMIN" ,"T BILL" ,"D BILL"};
        public double[] minValues = {70,140,10,0.4,0.1,0.1,6.4,3.5,0,0};
        public double[] maxValues = { 110,200,150,1.25,40,40,8.3,5,17,5.1};
    }
}
