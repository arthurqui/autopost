using System.IO;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE";
            WebClient client = new WebClient();
            string latestVersion = client.DownloadString(url);
            string downloadUrl = $"https://chromedriver.storage.googleapis.com/{latestVersion}/chromedriver_win32.zip";
            string driverPath = Path.Combine(Directory.GetCurrentDirectory(), "chromedriver.exe");

            // Faz o download do arquivo zip com o chromedriver
            client.DownloadFile(downloadUrl, "chromedriver.zip");

            if (!File.Exists(driverPath))
            {
                // Extrai o arquivo chromedriver.exe do arquivo zip baixado
                System.IO.Compression.ZipFile.ExtractToDirectory("chromedriver.zip", Directory.GetCurrentDirectory());

                // Renomeia o arquivo descompactado
                File.Move(Path.Combine(Directory.GetCurrentDirectory(), "chromedriver.exe"), driverPath);
            }

            // Configura o caminho do chromedriver no objeto ChromeOptions
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-browser-side-navigation");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument($"--webdriver.chrome.driver={driverPath}");

            // Inicia o ChromeDriver com as configurações definidas acima
            IWebDriver driver = new ChromeDriver(options);

            // Navega para o site do Google
            driver.Navigate().GoToUrl("https://www.google.com/");

            // Localiza o campo de pesquisa e insere o texto de pesquisa
            IWebElement searchField = driver.FindElement(By.Name("q"));
            searchField.SendKeys("brasil drop keys");

            // Submete o formulário pressionando a tecla Enter
            searchField.Submit();

            // Encerra o driver e deleta o arquivo chromedriver.exe
            driver.Quit();
            File.Delete(driverPath);

            // Exibe uma mensagem indicando que o teste foi finalizado
            System.Console.WriteLine("Teste finalizado com sucesso!");
        }
    }
}