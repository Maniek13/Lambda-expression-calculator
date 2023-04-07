using System.Linq.Expressions;

namespace Calc.Models
{
    public class Function
    {
        public ParameterExpression[] Parameters { get; set; }
        public float[] ParametersValue { get; set; }
        public string LambdaExpression { get; set; }
    }
}
