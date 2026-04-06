using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                // --- AQUI ESTÁ A VERIFICAÇÃO DE INTERNET (AGENDA 07) ---
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("Sem Internet", "Por favor, verifique sua conexão (Wi-Fi ou Dados) e tente novamente.", "OK");
                    return; // Interrompe a função aqui mesmo, não tenta buscar nada.
                }

                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = $"Clima: {t.description} \n" +
                                                $"Temperatura Máxima: {t.temp_max} °C \n" +
                                                $"Temperatura Mínima: {t.temp_min} °C \n" +
                                                $"Velocidade do Vento: {t.speed} m/s \n" +
                                                $"Visibilidade: {t.visibility} metros \n" +
                                                $"Latitude: {t.lat} \n" +
                                                $"Longitude: {t.lon} \n" +
                                                $"Nascer do Sol: {t.sunrise} \n" +
                                                $"Pôr do Sol: {t.sunset} \n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (Exception ex)
            {
                // Como nós usamos o "throw new Exception" lá no DataService, 
                // a mensagem "Cidade não encontrada" vai cair aqui dentro!
                await DisplayAlert("Ops", ex.Message, "OK");
                lbl_res.Text = "Busca cancelada.";
            }
        }
    }
}