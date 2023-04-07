using Calc.HelperClasses;
using Calc.Models;
using System.Collections.ObjectModel;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Calc.ViewModels
{
    public class CalculatorViewModel : PropertyChange
    {
        #region private members
        private readonly ObservableCollection<Variable> variables;
        private readonly ObservableCollection<Function> functionsList;
        private float _result = 0;
        #endregion

        #region property
        public ObservableCollection<Variable> Variables { get { return variables; } }
        public ObservableCollection<Function> FunctionsList { get { return functionsList; } }
        public float Result { 
            get 
            { 
                return _result; 
            } 
            set 
            { 
                _result = value; 
                OnPropertyChanged(nameof(Result)); 
            } 
        }
        #endregion
        internal CalculatorViewModel()
        {
            variables = new ObservableCollection<Variable>();
            functionsList = new ObservableCollection<Function>();
        }

        #region internal function
        internal void SetNumberOfVariables(int nrOf)
        {
            variables.Clear();
            for (int i = 1; i <= nrOf; i++)
                variables.Add(new Variable()
                {
                    Id = i,
                });
        }
        internal void Calculate(string lambdaExpression)
        {
            try
            {
                ParameterExpression[] parameters = new ParameterExpression[variables.Count];
                float[] parametersValue = new float[variables.Count];

                for (int i = 0; i < variables.Count; ++i)
                {
                    string name = variables.ElementAt(i).Name;
                    if (String.IsNullOrWhiteSpace(name))
                        throw new Exception($"Please write name of arguments nr: {variables.ElementAt(i).Id}");

                    ParameterExpression temp = Expression.Parameter(typeof(float), name);

                    var value = variables.ElementAt(i).Value ?? throw new Exception($"Please write value to parameter named: {name}");
                    parameters[i] = temp;
                    parametersValue[i] = (float)value;
                }

                if (String.IsNullOrWhiteSpace(lambdaExpression))
                    throw new Exception("Please write a function");

                LambdaExpression expression = DynamicExpressionParser.ParseLambda(parameters, typeof(float), lambdaExpression);

                Result = (float)expression.Compile().DynamicInvoke(parametersValue.Cast<object>().ToArray());

                FunctionsList.Add(new Function() { Parameters = parameters, ParametersValue = parametersValue, LambdaExpression = lambdaExpression });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        internal void UseTempExpression(Function function)
        {
            try
            {
                variables.Clear();
                for (int i = 0; i < function.Parameters.Length; i++)
                    variables.Add(new Variable()
                    {
                        Id = i+1,
                        Name = function.Parameters[i].Name,
                        Value = function.ParametersValue[i],
                    });
   

                LambdaExpression expression = DynamicExpressionParser.ParseLambda(function.Parameters, typeof(float), function.LambdaExpression);

                Result = (float)expression.Compile().DynamicInvoke(function.ParametersValue.Cast<object>().ToArray());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion
    }
}
