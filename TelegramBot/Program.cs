using System.Text.Json;
using TaskProject.Domain.Entities;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    public class Program
    {
        private static readonly HttpClient _client = new HttpClient();

        public static ITelegramBotClient _botClient;

        public static async Task Main(string[] args)
        {
            _botClient = new TelegramBotClient("6530285040:AAFOZe4HLj89L-yHVkyJLKrtqO9H2PSpxx4");

            _botClient.OnMessage += Bot_OnMessage;

            _botClient.OnCallbackQuery += Bot_OnCallbackQuery;

            _botClient.StartReceiving();

            Console.ReadLine();
        }

        public static async void Bot_OnMessage(object? sender, MessageEventArgs e)
        {
            string text = e.Message.Text;

            if (text == "/start")
            {
                await _botClient.SendTextMessageAsync(
                chatId: e.Message.Chat.Id,
                text: $"Assalomu aleykum {e.Message.Chat.FirstName}",
                replyMarkup: await SendKeyboardButton());
            }
            else
            {
                var products = await GetProductsFromApiAsync();

                var product = products.FirstOrDefault(x => x.SortNumber == text);

                if (product != null)
                {
                    await _botClient.SendTextMessageAsync(e.Message.Chat.Id, "Sending video✅");

                    using (var videoStream = System.IO.File.OpenRead("../../../../TaspProject/wwwroot/" + product.VideoPath))
                    {
                        var videoInputFile = new InputOnlineFile(videoStream, Path.GetFileName("../../../../TaspProject/wwwroot/" + product.VideoPath));
                        var message = await _botClient.SendVideoAsync(e.Message.Chat.Id, videoInputFile,replyMarkup: await
                            SendKeyboardButton());
                    }

                    await _botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                }
                else
                {
                    await _botClient.SendTextMessageAsync(e.Message.Chat.Id, "Video Not Found❌", replyMarkup: await SendKeyboardButton());
                }

            }
        }

        public static async Task<InlineKeyboardMarkup> SendInlineKeyboard()
        {
            var response = await GetProductsFromApiAsync();

            var inlineKeyboard = new List<List<InlineKeyboardButton>>();

            foreach (var i in response)
            {
                var row = new List<InlineKeyboardButton>();
                row.Add(new InlineKeyboardButton { Text = i.SortNumber, CallbackData = i.Id.ToString() });
                inlineKeyboard.Add(row);
            }
            return new InlineKeyboardMarkup(inlineKeyboard);
        }

        private static async Task<List<Product>> GetProductsFromApiAsync()
        {
            var response = await _client.GetAsync("https://localhost:44369/api/Product/GetAll");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var products = JsonSerializer.Deserialize<List<Product>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return products;
        }

        public static async void Bot_OnCallbackQuery(object? sender, CallbackQueryEventArgs e)
        {
            string callbackData = e.CallbackQuery.Data;

            if (!callbackData.StartsWith("page"))
            {
                var products = await GetProductsFromApiAsync();

                var product = products.FirstOrDefault(x => x.Id == int.Parse(callbackData));

                await _botClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Sending video✅");

                using (var videoStream = System.IO.File.OpenRead("../../../../TaspProject/wwwroot/" + product.VideoPath))
                {
                    var videoInputFile = new InputOnlineFile(videoStream, Path.GetFileName("../../../../TaspProject/wwwroot/" + product.VideoPath));
                    var message = await _botClient.SendVideoAsync(e.CallbackQuery.Message.Chat.Id, videoInputFile);
                }
            }
        }

        public static async Task<ReplyKeyboardMarkup> SendKeyboardButton()
        {
            var response = await GetProductsFromApiAsync();

            var halfCount = response.Count / 2;
            var firstHalf = response.Take(halfCount);
            var secondHalf = response.Skip(halfCount);

            var keyboardRows = new List<List<KeyboardButton>>();

            for (int i = 0; i < halfCount; i++)
            {
                var row = new List<KeyboardButton>
                {
                    new KeyboardButton { Text = firstHalf.ElementAt(i).SortNumber},
                    new KeyboardButton { Text = secondHalf.ElementAt(i).SortNumber }
                };
                keyboardRows.Add(row);
            }

            if (response.Count % 2 != 0)
            {
                keyboardRows.Last().Add(new KeyboardButton { Text = secondHalf.Last().SortNumber });
            }

            var replyKeyboardMarkup = new ReplyKeyboardMarkup(keyboardRows.ToArray())
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }

        public static async Task<InlineKeyboardMarkup> SendInlineKeyboard(int page = 1, int pageSize = 5)
        {
            try
            {
                var response = await GetProductsFromApiAsync();

                // Paginate the products
                var products = response.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var inlineKeyboard = new List<List<InlineKeyboardButton>>();

                foreach (var product in products)
                {
                    var row = new List<InlineKeyboardButton>();
                    row.Add(new InlineKeyboardButton { Text = product.SortNumber, CallbackData = product.Id.ToString() });
                    inlineKeyboard.Add(row);
                }

                // Add navigation buttons
                var navigationButtons = new List<InlineKeyboardButton>();
                if (page > 1)
                {
                    navigationButtons.Add(new InlineKeyboardButton { Text = "Previous", CallbackData = $"page={page - 1}" });
                }
                if (response.Count > page * pageSize)
                {
                    navigationButtons.Add(new InlineKeyboardButton { Text = "Next", CallbackData = $"page={page + 1}" });
                }
                inlineKeyboard.Add(navigationButtons);

                return new InlineKeyboardMarkup(inlineKeyboard);
            }
            catch (Exception ex)
            {
                // Handle the exception (log, throw, etc.)
                Console.WriteLine($"Error in SendInlineKeyboard: {ex.Message}");
                throw;
            }
        }

    }
}