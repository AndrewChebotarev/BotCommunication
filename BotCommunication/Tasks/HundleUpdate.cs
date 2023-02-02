namespace TelegramBot.Tasks
{
    public class HundleUpdate
    {
        private UserTasks userTasks = new();
        private AdminTasks adminTasks = new();
        private AssistantTasks assistantTasks = new();
        private UnauthorizedUserTasks unauthorizedUserTasks = new();
        private HundleCheckingMessage hundleCheckingMessage = new();

        private List<long> UnauthorizedUserList = new();
        private List<long> BlackList = new();
        private List<long> UserList = new();
        private List<long> AssistantList = new();

        private List<string> MessagesForCommand = new() { "Обычный пользователь", "обычный пользователь", "Администратор", "администратор",
            "/start", "/stop", "/Смена пароля", "/смена пароля", "/Стать помощником", "/стать помощником", "/Добавить помощника", "/добавить помощника",
            "/Удалить помощника", "/удалить помощника", "/Черный список", "/черный список",
            "/All user list", "/all user list", "/Help", "/help", "/Назад", "/назад", "/Белый список", "/белый список", "/SendAllUser" };

        private Dictionary<long, bool> AdminDictionary = new();
        private Dictionary<long, long> AdminTransferUser = new();
        private Dictionary<long, bool> AdminForChangePassword = new();
        private Dictionary<long, bool> UserForAssistant = new();
        private Dictionary<long, bool> AdminAddAssistant = new();
        private Dictionary<long, bool> AdminDeleteAssistant = new();
        private Dictionary<long, bool> AdminAddToBlackList = new();
        private Dictionary<long, bool> AdminAddToWhiteList = new();
        private Dictionary<long, bool> AdminAllUserListSend = new();

        private Dictionary<long, long> AssistantTransferUser = new();

        private string password = "a12345z";

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var checkingForMessage = hundleCheckingMessage.CheckingForMessage(update);

            if (!checkingForMessage)
            {
                return;
            }

            if (BlackList.Contains(update.Message.Chat.Id))
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: "Вы в черном списке.",
                        cancellationToken: cancellationToken);

                return;
            }

            try
            {
                if (update.Message.Text == MessagesForCommand[4])
                {
                    UnauthorizedUserList.Add(update.Message.Chat.Id);
                    await unauthorizedUserTasks.StartHundleAsync(botClient, update, cancellationToken);
                    return;
                }

                if (!UnauthorizedUserList.Contains(update.Message.Chat.Id) && !UserList.Contains(update.Message.Chat.Id) && !AdminDictionary.ContainsKey(update.Message.Chat.Id) && !UserForAssistant.ContainsKey(update.Message.Chat.Id) && !AssistantList.Contains(update.Message.Chat.Id))
                {
                    UnauthorizedUserList.Add(update.Message.Chat.Id);
                    await unauthorizedUserTasks.StartHundleAsync(botClient, update, cancellationToken);
                    return;
                }

                if (UnauthorizedUserList.Contains(update.Message.Chat.Id))
                {
                    if (update.Message.Text == MessagesForCommand[0] || update.Message.Text == MessagesForCommand[1])
                    {
                        await userTasks.StandardUserChoiceHundleAsync(botClient, update, cancellationToken);
                        return;
                    }

                    if ((update.Message.Text.StartsWith('+') || update.Message.Text.StartsWith('8')) && (update.Message.Text.Length == 11 || update.Message.Text.Length == 12))
                    {
                        UnauthorizedUserList.Remove(update.Message.Chat.Id);
                        UserList.Add(update.Message.Chat.Id);
                        await userTasks.EnterPhoneNumberHundleAsync(update);
                        return;
                    }

                    if (update.Message.Text == MessagesForCommand[2] || update.Message.Text == MessagesForCommand[3])
                    {
                        UnauthorizedUserList.Remove(update.Message.Chat.Id);
                        AdminDictionary.Add(update.Message.Chat.Id, false);
                        await adminTasks.AdminChoiceHundleAsync(botClient, update, cancellationToken);
                        return;
                    }

                    if (!MessagesForCommand.Contains(update.Message.Text))
                    {
                        await unauthorizedUserTasks.RepeatHundleAsync(update);
                        return;
                    }

                    if (update.Message.Text == MessagesForCommand[4] || update.Message.Text == MessagesForCommand[5])
                    {
                        StopCommandHundle(botClient, update, cancellationToken);
                        return;
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка на уровне незарегистрированного пользователя. {ex.Message}");
                await unauthorizedUserTasks.RepeatHundleAsync(update);
                return;
            }

            try
            {
                if (UserList.Contains(update.Message.Chat.Id))
                {
                    if (update.Message.Text == MessagesForCommand[4] || update.Message.Text == MessagesForCommand[5])
                    {
                        StopCommandHundle(botClient, update, cancellationToken);
                        return;
                    }

                    if (AdminDictionary.Count == 0)
                    {
                        await userTasks.UserTransferButNoAdministratorHundleAsync(update);
                        return;
                    }

                    foreach (var Admin in AdminDictionary)
                    {
                        await userTasks.UserTransferToAdministratorHundleAsync(update, Admin.Key);
                    }

                    foreach (var assistant in AssistantList)
                    {
                        await userTasks.UserTransferAssistantHundleAsync(update, assistant);
                    }

                    if (update.Message.Text == MessagesForCommand[8] || update.Message.Text == MessagesForCommand[9])
                    {
                        await userTasks.BecomeAssistantHundleAsync(update);
                        UserList.Remove(update.Message.Chat.Id);
                        UserForAssistant.Add(update.Message.Chat.Id, false);
                        await assistantTasks.AssistantChoiceHundleAsync(botClient, update, cancellationToken);
                        return;
                    }
                }

                if (UserForAssistant.ContainsKey(update.Message.Chat.Id))
                {
                    if (UserForAssistant[update.Message.Chat.Id] == false)
                    {
                        if (update.Message.Text == MessagesForCommand[4] || update.Message.Text == MessagesForCommand[5])
                        {
                            StopCommandHundle(botClient, update, cancellationToken);
                            return;
                        }

                        await userTasks.BecomeAssistantWaitingHundleAsync(update);
                        return;
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка на уровне зарегистрированного пользователя. {ex.Message}");
                return;
            }

            try
            {
                if (AdminDictionary.ContainsKey(update.Message.Chat.Id))
                {
                    if (update.Message.Text == MessagesForCommand[4] || update.Message.Text == MessagesForCommand[5])
                    {
                        StopCommandHundle(botClient, update, cancellationToken);
                        return;
                    }

                    if (AdminDictionary[update.Message.Chat.Id] == false && update.Message.Text == password)
                    {
                        AdminDictionary[update.Message.Chat.Id] = true;
                        await adminTasks.AdminPasswordHundleAsync(update);
                        return;
                    }

                    if (AdminDictionary[update.Message.Chat.Id] == false && update.Message.Text != password)
                    {
                        await adminTasks.AdminPasswordErrorHundleAsync(update);
                        return;
                    }

                    if (AdminTransferUser.ContainsKey(update.Message.Chat.Id))
                    {
                        await adminTasks.AdminTransferToUserHundleAsync(update, AdminTransferUser);
                        AdminTransferUser.Remove(update.Message.Chat.Id);
                        return;
                    }

                    if (AdminDictionary[update.Message.Chat.Id] == true)
                    {

                        if (update.Message.Text == MessagesForCommand[18] || update.Message.Text == MessagesForCommand[19])
                        {
                            await adminTasks.AllCommandHundleAsync(update);
                            return;
                        }

                        if (update.Message.Text == MessagesForCommand[16] || update.Message.Text == MessagesForCommand[17])
                        {
                            if (UnauthorizedUserList.Count > 0)
                            {
                                foreach (var UnauthorizedUser in UnauthorizedUserList)
                                {
                                    await adminTasks.AllUnauthorizedUserListAddHundleAsync(update, UnauthorizedUser);
                                }
                            }

                            if (UserList.Count > 0)
                            {
                                foreach (var User in UserList)
                                {
                                    await adminTasks.AllUserListAddHundleAsync(update, User);
                                }
                            }

                            if (AssistantList.Count > 0)
                            {
                                foreach (var Assistant in AssistantList)
                                {
                                    await adminTasks.AllAssistantListAddHundleAsync(update, Assistant);
                                }
                            }

                            if (AdminDictionary.Count > 0)
                            {
                                foreach (var Admin in AdminDictionary.Keys)
                                {
                                    await adminTasks.AllAdminListAddHundleAsync(update, Admin);
                                }
                            }

                            return;
                        }

                        if (UserList.Count > 0 && AdminAllUserListSend.ContainsKey(update.Message.Chat.Id) == true)
                        {
                            foreach (var User in UserList)
                            {
                                await adminTasks.ResultAllUserListSendHundleAsync(update, User);
                                AdminAllUserListSend[update.Message.Chat.Id] = false;
                                return;
                            }
                        }

                        if (update.Message.Text == MessagesForCommand[24])
                        {
                            if (UserList.Count > 0)
                            {
                                await adminTasks.AllUserListSendHundleAsync(update);
                                AdminAllUserListSend.Add(update.Message.Chat.Id, true);
                                return;
                            }

                            if (UserList.Count == 0)
                            {
                                await adminTasks.ErrorAllUserListSendHundleAsync(update);
                                return;
                            }
                        }

                        if (update.Message.Text == MessagesForCommand[6] || update.Message.Text == MessagesForCommand[7])
                        {
                            await adminTasks.EnterChangePasswordHundleAsync(update);
                            AdminForChangePassword.Add(update.Message.Chat.Id, true);
                            return;
                        }

                        if (AdminForChangePassword.Count != 0)
                        {
                            if (AdminForChangePassword[update.Message.Chat.Id] == true)
                            {
                                password = update.Message.Text;
                                AdminForChangePassword.Remove(update.Message.Chat.Id);
                                await adminTasks.ResultChangePasswordHundleAsync(update);
                                return;
                            }
                        }
                    }

                    if (AdminDictionary[update.Message.Chat.Id] == true)
                    {

                        if (update.Message.Text == MessagesForCommand[10] || update.Message.Text == MessagesForCommand[11])
                        {
                            await adminTasks.AppointAssistantHundleAsync(update);
                            AdminAddAssistant.Add(update.Message.Chat.Id, true);
                            return;
                        }

                        if (AdminAddAssistant.ContainsKey(update.Message.Chat.Id))
                        {
                            if (!update.Message.Text.StartsWith("/") && AdminAddAssistant[update.Message.Chat.Id] == true)
                            {
                                if (UserForAssistant.ContainsKey(Convert.ToInt64(update.Message.Text)))
                                {
                                    await adminTasks.ResultAppointAssistantHundleAsync(update);
                                    UserForAssistant.Remove(Convert.ToInt64(update.Message.Text));
                                    AssistantList.Add(Convert.ToInt64(update.Message.Text));
                                    AdminAddAssistant.Remove(update.Message.Chat.Id);
                                    return;
                                }

                                if (!UserForAssistant.ContainsKey(Convert.ToInt64(update.Message.Text)))
                                {
                                    await adminTasks.ErrorAppointAssistantHundleAsync(update);
                                    AdminAddAssistant.Remove(update.Message.Chat.Id);
                                    return;
                                }
                            }
                        }

                        if (update.Message.Text == MessagesForCommand[12] || update.Message.Text == MessagesForCommand[13])
                        {
                            await adminTasks.DeleteAssistantHundleAsync(update);
                            AdminDeleteAssistant.Add(update.Message.Chat.Id, true);
                            return;
                        }

                        if (AdminDeleteAssistant.ContainsKey(update.Message.Chat.Id))
                        {
                            if (!update.Message.Text.StartsWith("/"))
                            {
                                if (AssistantList.Contains(Convert.ToInt64(update.Message.Text)) && AdminDeleteAssistant[update.Message.Chat.Id] == true)
                                {
                                    AssistantList.Remove(Convert.ToInt64(update.Message.Text));
                                    UnauthorizedUserList.Add(Convert.ToInt64(update.Message.Text));
                                    await adminTasks.ResultDeleteAssistantHundleAsync(update);
                                    AdminDeleteAssistant.Remove(update.Message.Chat.Id);
                                    return;
                                }

                                if (!AssistantList.Contains(Convert.ToInt64(update.Message.Text)) && AdminDeleteAssistant[update.Message.Chat.Id] == true)
                                {
                                    await adminTasks.ErrorDeleteAssistantHundleAsync(update);
                                    return;
                                }
                            }
                        }

                        if (AdminDeleteAssistant.ContainsKey(update.Message.Chat.Id))
                        {
                            if (AdminDeleteAssistant[update.Message.Chat.Id] == true && update.Message.Text == MessagesForCommand[20] || update.Message.Text == MessagesForCommand[21])
                            {
                                await adminTasks.AdminPasswordHundleAsync(update);
                                AdminDeleteAssistant.Remove(update.Message.Chat.Id);
                                return;
                            }
                        }

                        if (update.Message.Text == MessagesForCommand[14] || update.Message.Text == MessagesForCommand[15])
                        {
                            await adminTasks.BlackListAddHundleAsync(update);
                            AdminAddToBlackList.Add(update.Message.Chat.Id, true);
                            return;
                        }

                        if (AdminAddToBlackList.ContainsKey(update.Message.Chat.Id))
                        {
                            if (!update.Message.Text.StartsWith("/"))
                            {
                                if (UserList.Contains(Convert.ToInt64(update.Message.Text)) && AdminAddToBlackList[update.Message.Chat.Id] == true)
                                {
                                    UserList.Remove(Convert.ToInt64(update.Message.Text));
                                    BlackList.Add(Convert.ToInt64(update.Message.Text));
                                    await adminTasks.ResultBlackListAddHundleAsync(update);
                                    AdminAddToBlackList.Remove(update.Message.Chat.Id);
                                    return;
                                }

                                if (!UserList.Contains(Convert.ToInt64(update.Message.Text)) && AdminAddToBlackList[update.Message.Chat.Id] == true)
                                {
                                    await adminTasks.ErrorBlackListAddHundleAsync(update);
                                    return;
                                }
                            }
                        }

                        if (AdminAddToBlackList.ContainsKey(update.Message.Chat.Id))
                        {
                            if (AdminAddToBlackList[update.Message.Chat.Id] == true && update.Message.Text == MessagesForCommand[20] || update.Message.Text == MessagesForCommand[21])
                            {
                                await adminTasks.AdminPasswordHundleAsync(update);
                                AdminAddToBlackList.Remove(update.Message.Chat.Id);
                                return;
                            }
                        }

                        if (update.Message.Text == MessagesForCommand[22] || update.Message.Text == MessagesForCommand[23])
                        {
                            await adminTasks.WhiteListAddHundleAsync(update);
                            AdminAddToWhiteList.Add(update.Message.Chat.Id, true);
                            return;
                        }

                        if (AdminAddToWhiteList.ContainsKey(update.Message.Chat.Id))
                        {
                            if (!update.Message.Text.StartsWith("/"))
                            {
                                if (BlackList.Contains(Convert.ToInt64(update.Message.Text)) && AdminAddToWhiteList[update.Message.Chat.Id] == true)
                                {
                                    BlackList.Remove(Convert.ToInt64(update.Message.Text));
                                    UnauthorizedUserList.Add(Convert.ToInt64(update.Message.Text));
                                    await adminTasks.ResultWhiteListAddHundleAsync(update);
                                    AdminAddToWhiteList.Remove(update.Message.Chat.Id);
                                    return;
                                }

                                if (!BlackList.Contains(Convert.ToInt64(update.Message.Text)) && AdminAddToWhiteList[update.Message.Chat.Id] == true)
                                {
                                    await adminTasks.ErrorWhiteListAddHundleAsync(update);
                                    return;
                                }
                            }
                        }

                        if (AdminAddToWhiteList.ContainsKey(update.Message.Chat.Id))
                        {
                            if (AdminAddToWhiteList[update.Message.Chat.Id] == true && (update.Message.Text == MessagesForCommand[20] || update.Message.Text == MessagesForCommand[21]))
                            {
                                await adminTasks.AdminPasswordHundleAsync(update);
                                AdminAddToWhiteList.Remove(update.Message.Chat.Id);
                                return;
                            }
                        }

                        if (!update.Message.Text.StartsWith("/"))
                        {
                            if (UserList.Contains(Convert.ToInt64(update.Message.Text)))
                            {
                                AdminTransferUser.Add(update.Message.Chat.Id, Convert.ToInt64(update.Message.Text));
                                await adminTasks.AdminMessageIdHundleAsync(update);
                                return;
                            }

                            if (!UserList.Contains(Convert.ToInt64(update.Message.Text)))
                            {
                                await adminTasks.AdminMessageIdErrorHundleAsync(update);
                                return;
                            }
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка на уровне администратора. {ex.Message}");
                await adminTasks.AdminMessageIdErrorHundleAsync(update);
                return;
            }

            try
            {
                if (AssistantList.Contains(update.Message.Chat.Id))
                {
                    if (update.Message.Text == MessagesForCommand[4] || update.Message.Text == MessagesForCommand[5])
                    {
                        StopCommandHundle(botClient, update, cancellationToken);
                        return;
                    }

                    if (AssistantTransferUser.ContainsKey(update.Message.Chat.Id))
                    {
                        await assistantTasks.AssistantTransferToUserHundleAsync(update, AssistantTransferUser);
                        AssistantTransferUser.Remove(update.Message.Chat.Id);
                        return;
                    }

                    if (!update.Message.Text.StartsWith("/"))
                    {
                        if (UserList.Contains(Convert.ToInt64(update.Message.Text)))
                        {
                            AssistantTransferUser.Add(update.Message.Chat.Id, Convert.ToInt64(update.Message.Text));
                            await assistantTasks.AssistantMessageIdHundleAsync(update);
                            return;
                        }

                        if (!UserList.Contains(Convert.ToInt64(update.Message.Text)))
                        {
                            await assistantTasks.AssistantMessageIdErrorHundleAsync(update);
                            return;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка на уровне помощника. {ex.Message}");
                await assistantTasks.AssistantMessageIdErrorHundleAsync(update);
                return;
            }


            if (update.Message.Text.StartsWith("/"))
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Неопознанная команда. Попробуйте снова.",
                    cancellationToken: cancellationToken);

                return;
            }
        }

        private async void StopCommandHundle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UnauthorizedUserList.Remove(update.Message.Chat.Id);
            UserList.Remove(update.Message.Chat.Id);
            AdminDictionary.Remove(update.Message.Chat.Id);
            await unauthorizedUserTasks.StopMessageHundleAsync(update);
            await unauthorizedUserTasks.StartHundleAsync(botClient, update, cancellationToken);
            UnauthorizedUserList.Add(update.Message.Chat.Id);
        }
    }
}

