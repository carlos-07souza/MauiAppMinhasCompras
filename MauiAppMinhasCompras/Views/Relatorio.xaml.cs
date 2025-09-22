using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MauiAppMinhasCompras.Models; // Certifique-se de que esta linha está no topo

namespace MauiAppMinhasCompras.Views
{
    public partial class Relatorio : ContentPage
    {
        public ObservableCollection<Produto> ListaRelatorio { get; set; }

        public Relatorio()
        {
            InitializeComponent();
            ListaRelatorio = new ObservableCollection<Produto>();
            BindingContext = this; // Para que o XAML possa usar ListaRelatorio
            lst_relatorio.ItemsSource = ListaRelatorio;
        }

        private async void Button_Filtrar_Clicked(object sender, EventArgs e)
        {
            try
            {
                DateTime dataInicial = dp_data_inicial.Date;
                DateTime dataFinal = dp_data_final.Date;

                // Ajusta a data final para incluir todo o dia
                dataFinal = dataFinal.AddDays(1).AddSeconds(-1);

                if (dataInicial > dataFinal)
                {
                    await DisplayAlert("Erro", "A data inicial não pode ser maior que a data final.", "OK");
                    return;
                }

                // Chamada para o método do banco de dados. Certifique-se de que o método
                // GetByDate no seu ProdutoRepository.cs esteja implementado para usar DataCadastro.
                List<Produto> produtosFiltrados = await App.Db.GetByDate(dataInicial, dataFinal);

                ListaRelatorio.Clear();
                foreach (var produto in produtosFiltrados)
                {
                    ListaRelatorio.Add(produto);
                }

                lst_relatorio.IsVisible = true; // Mostra a lista após filtrar
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Ocorreu um erro ao filtrar os dados: " + ex.Message, "OK");
            }
        }
    }
}