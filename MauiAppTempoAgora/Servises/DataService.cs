using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net; // Linha que faz reconhecer o nome "HttpStatusCode"

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "05d560a316dcc7b92a24e45b583ecd94";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    }; // Fecha obj do Tempo.
                } // Fecha if se o status do servidor foi de sucesso
                // --- AQUI COMEÇA O TRATAMENTO DE ERRO DA API ---
                else if (resp.StatusCode == HttpStatusCode.NotFound)
                {
                    // Se for 404, joga esse erro lá para a tela principal capturar
                    throw new Exception("Cidade não encontrada. Verifique o nome digitado.");
                }
                else
                { // Qualquer outro erro do servidor (ex: API fora do ar)
                    throw new Exception("Erro ao buscar a previsão. Código: " + resp.StatusCode);
                }
                } // fecha laço using

            return t;
        }
    }
}
