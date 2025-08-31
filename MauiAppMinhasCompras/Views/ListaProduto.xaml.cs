using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{

         private SQLiteDatabaseHelper _db;

    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;

    }

    protected async override void OnAppearing()
    {
        List<Produto> tmp = await App.Db.GetAll();

        tmp.ForEach(i => lista.Add(i));
    }

    //CarregarProduto
    //     private async Task CarregarProdutos()
    // {
    //    Lista.Clear();
    //    List<Produto> tmp = await _db.GetAll();
    // tmp.ForEach(p => Lista.Add(p));
    // }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");

        }

    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue;

        lista.Clear();

        List<Produto> tmp = await App.Db.Search(q);

        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total de produtos", msg, "Ok");
    }

    // método delete

    private async Task CarregarProdutos()
    {
        lista.Clear();
        List<Produto> tmp = await _db.GetAll();
        tmp.ForEach(p => lista.Add(p));
    }

    private async void MenuItem_Remover(object sender, EventArgs e)
    {
        try 
        {
            var menuItem = sender as MenuItem;
            var itemSelecionado = menuItem.BindingContext as Produto;

            if (itemSelecionado != null)
            {
                bool confirmar = await DisplayAlert("Remover", $"Deseja remover '{itemSelecionado.Descricao}'?", "Sim", "Não");
                if (confirmar)
                {
                    await App.Db.Delete(itemSelecionado.Id);

                    var itemNaLista = lista.FirstOrDefault(p => p.Id == itemSelecionado.Id);
                    if (itemNaLista != null)
                    {
                        lista.Remove(itemSelecionado);
                    }


                }
            }



        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
       
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

        try
        {

            Produto p = e.SelectedItem as Produto;
            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,

            });
        }



        catch (Exception ex) 
        {
            DisplayAlert("Ops", ex.Message, "Ok");

        }

    }
}

internal class SQLiteDatabaseHelper
{
    internal async Task<List<Produto>> GetAll()
    {
        throw new NotImplementedException();
    }
}