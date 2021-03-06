﻿using MixItUp.Base.Model.Commands;
using MixItUp.Base.Util;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Twitch.Base.Models.NewAPI.ChannelPoints;

namespace MixItUp.Base.ViewModel.MainControls
{
    public class TwitchChannelPointsMainControlViewModel : GroupedCommandsMainControlViewModelBase
    {
        public ICommand CreateChannelPointRewardCommand { get; set; }

        public ICommand ChannelPointsEditorCommand { get; set; }

        public TwitchChannelPointsMainControlViewModel(MainWindowViewModel windowViewModel)
            : base(windowViewModel)
        {
            GroupedCommandsMainControlViewModelBase.OnCommandAddedEdited += GroupedCommandsMainControlViewModelBase_OnCommandAddedEdited;

            this.CreateChannelPointRewardCommand = this.CreateCommand(async () =>
            {
                string name = await DialogHelper.ShowTextEntry(MixItUp.Base.Resources.ChannelPointRewardName);
                if (!string.IsNullOrEmpty(name))
                {
                    CustomChannelPointRewardModel reward = await ChannelSession.TwitchUserConnection.CreateCustomChannelPointRewards(ChannelSession.TwitchUserNewAPI, new UpdatableCustomChannelPointRewardModel()
                    {
                        title = name,
                        cost = 1,
                        is_enabled = true,
                    });

                    if (reward != null)
                    {
                        this.AddCommand(new TwitchChannelPointsCommandModel(reward.title, reward.id));

                        await DialogHelper.ShowMessage(MixItUp.Base.Resources.CreateChannelPointRewardSuccess);
                    }
                    else
                    {
                        await DialogHelper.ShowMessage(MixItUp.Base.Resources.CreateChannelPointRewardFailure);
                    }
                }
            });

            this.ChannelPointsEditorCommand = this.CreateCommand(() =>
            {
                if (ChannelSession.TwitchUserConnection != null)
                {
                    ProcessHelper.LaunchLink($"https://dashboard.twitch.tv/u/{ChannelSession.TwitchUserNewAPI.login}/viewer-rewards/channel-points");
                }
            });
        }

        protected override IEnumerable<CommandModelBase> GetCommands()
        {
            return ChannelSession.TwitchChannelPointsCommands.ToList();
        }

        private void GroupedCommandsMainControlViewModelBase_OnCommandAddedEdited(object sender, CommandModelBase command)
        {
            if (command.Type == CommandTypeEnum.TwitchChannelPoints)
            {
                this.AddCommand(command);
            }
        }
    }
}
