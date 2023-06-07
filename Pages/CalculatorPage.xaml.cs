using Calc.Models;
using Calc.ViewModels;
using System.Runtime.CompilerServices;

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

    private void NewExpressionsButtonClicked(object sender, EventArgs e)
    {
        ViewModel.Clear();
        clear.IsVisible = false;
        calcLayout.IsVisible = false;
        error.IsVisible = false;
        error.Text = "";
        lambdaExpression.Text = "";
        numberOfVariables.Text = "";
    }
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
            calcLayout.IsVisible = true;
            Button button = (Button)sender;
            Function function = (Function)button.BindingContext;
            
            ViewModel.UseTempExpression(function);
            lambdaExpression.Text = function.LambdaExpression;
            clear.IsVisible = true;
            numberOfVariables.Text = ViewModel.Variables.Count.ToString();
        }
        catch (Exception ex)
        {
            error.Text = ex.Message;
            error.IsVisible = true;
        }
    }
    #endregion
}