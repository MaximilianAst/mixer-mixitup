﻿using MixItUp.Base;
using MixItUp.Base.Model.User;
using MixItUp.Base.Util;
using MixItUp.Base.ViewModel.User;
using MixItUp.WPF.Controls.Dialogs;
using MixItUp.WPF.Windows.Users;
using StreamingClient.Base.Util;
using System.Threading.Tasks;

namespace MixItUp.WPF.Controls.Chat
{
    public static class ChatUserDialogControl
    {
        public static async Task ShowUserDialog(UserViewModel user)
        {
            if (user != null && !user.IsAnonymous)
            {
                object result = await DialogHelper.ShowCustom(new UserDialogControl(user));
                if (result != null)
                {
                    UserDialogResult dialogResult = EnumHelper.GetEnumValueFromString<UserDialogResult>(result.ToString());
                    switch (dialogResult)
                    {
                        case UserDialogResult.Purge:
                            await ChannelSession.Services.Chat.PurgeUser(user);
                            break;
                        case UserDialogResult.Timeout1:
                            await ChannelSession.Services.Chat.TimeoutUser(user, 60);
                            break;
                        case UserDialogResult.Timeout5:
                            await ChannelSession.Services.Chat.TimeoutUser(user, 300);
                            break;
                        case UserDialogResult.Ban:
                            if (await DialogHelper.ShowConfirmation(string.Format(Resources.BanUserPrompt, user.DisplayName)))
                            {
                                await ChannelSession.Services.Chat.BanUser(user);
                            }
                            break;
                        case UserDialogResult.Unban:
                            await ChannelSession.Services.Chat.UnbanUser(user);
                            break;
                        case UserDialogResult.Follow:
                            await ChannelSession.TwitchUserConnection.FollowUser(ChannelSession.TwitchUserNewAPI, user.GetTwitchNewAPIUserModel());
                            break;
                        case UserDialogResult.Unfollow:
                            await ChannelSession.TwitchUserConnection.UnfollowUser(ChannelSession.TwitchUserNewAPI, user.GetTwitchNewAPIUserModel());
                            break;
                        case UserDialogResult.PromoteToMod:
                            if (await DialogHelper.ShowConfirmation(string.Format(Resources.PromoteUserPrompt, user.DisplayName)))
                            {
                                await ChannelSession.Services.Chat.ModUser(user);
                            }
                            break;
                        case UserDialogResult.DemoteFromMod:
                            if (await DialogHelper.ShowConfirmation(string.Format(Resources.DemoteUserPrompt, user.DisplayName)))
                            {
                                await ChannelSession.Services.Chat.UnmodUser(user);
                            }
                            break;
                        case UserDialogResult.ChannelPage:
                            ProcessHelper.LaunchLink(user.ChannelLink);
                            break;
                        case UserDialogResult.EditUser:
                            UserDataModel userData = ChannelSession.Settings.GetUserData(user.ID);
                            if (userData != null)
                            {
                                UserDataEditorWindow window = new UserDataEditorWindow(userData);
                                await Task.Delay(100);
                                window.Show();
                                await Task.Delay(100);
                                window.Focus();
                            }
                            break;
                        case UserDialogResult.Close:
                        default:
                            // Just close
                            break;
                    }
                }
            }
        }
    }
}
