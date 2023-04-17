using Calc.Models;
using Calc.ViewModels;

namespace Calc.Pages;
public partial class CalculatorPage : ContentPage
{
	internal CalculatorViewModel ViewModel { get; set; }
    public CalculatorPage()
	{
        InitializeComponent();
        BindingContext = ViewModel = new CalculatorViewModel();
    }
    #region button event hendlers
    private void NumbersOfVariablesButtonClicked(object sender, EventArgs e)
	{
        try
        {
            error.Text = string.Empty;
            error.IsVisible = false;

            if (String.IsNullOrWhiteSpace(numberOfVariables.Text))
                throw new Exception("Write a number of arguments");

            if (int.TryParse(numberOfVariables.Text.ToString(), out int nrOf))
            {
                calcLayout.IsVisible = true;
                SetVariableListHeight(nrOf);
                ViewModel.SetNumberOfVariables(nrOf);
              
            }
            else
                throw new Exception("Numbers of arguments must be a number");

 
            
        }
        catch (Exception ex)
        {
            error.Text = ex.Message;
            error.IsVisible = true;
        }
    }
    private void CalculateButtonClicked(object sender, EventArgs e)
	{
        try
        {
            error.Text = string.Empty;
            error.IsVisible = false;

            ViewModel.Calculate(lambdaExpression.Text);
        }
        catch(Exception ex)
        {
            error.Text = ex.Message;
            error.IsVisible = true;
        }
    }
    private  void UseTempExpressionButtonClicked(object sender, EventArgs e)
    {
        try
        {
            error.Text = string.Empty;
            error.IsVisible = false;


            Button button = (Button)sender;
            Function function = (Function)button.BindingContext;

            SetVariableListHeight(function.Parameters.Length);
            

            ViewModel.UseTempExpression(function);
            lambdaExpression.Text = function.LambdaExpression;
        }
        catch (Exception ex)
        {
            error.Text = ex.Message;
            error.IsVisible = true;
        }
    }

    private void SetVariableListHeight(int nrOf)
    {
        variableList.MinimumHeightRequest = nrOf < 5 ? nrOf * 100 : 500;
    }
    #endregion
}