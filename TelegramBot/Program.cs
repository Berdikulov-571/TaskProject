using MediatR;
using System;
using System.Text.Json;
using TaskProject.Domain.Entities;
using TaskProject.Service.UseCases.Products.Queries;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types;
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
                text: "Choose an option:",
                replyMarkup: await SendInlineKeyboard()
            );


            }
        }
        public static string[] GetVideos()
        {
            string[] videos = Directory.GetFiles("../../../../TaspProject/wwwroot/media/videos", "FILE_1dd5bf84-d1c9-4714-a594-791c96832dd3.mp4");
            return videos;
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

            var products = await GetProductsFromApiAsync();

            var product = products.FirstOrDefault(x => x.Id == int.Parse(callbackData));

            using (var videoStream = System.IO.File.OpenRead("../../../../TaspProject/wwwroot/" + product.VideoPath))
            {
                var videoInputFile = new InputOnlineFile(videoStream, Path.GetFileName("../../../../TaspProject/wwwroot/" + product.VideoPath));
                var message = await _botClient.SendVideoAsync(e.CallbackQuery.Message.Chat.Id, videoInputFile);
                // Optionally, you can handle the message response here
            }


            //_botClient.SendTextMessageAsync(
            //    chatId: e.CallbackQuery.Message.Chat.Id,
            //    text: $"You clicked on: {callbackData}"
            //);
        }
    }
}