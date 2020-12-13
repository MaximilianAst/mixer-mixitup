﻿using MixItUp.Base.Model.Commands;
using MixItUp.Base.Model.Commands.Games;
using MixItUp.Base.Model.Currency;
using MixItUp.Base.Util;
using System.Threading.Tasks;

namespace MixItUp.Base.ViewModel.Games
{
    public class HeistGameCommandEditorWindowViewModel : GameCommandEditorWindowViewModelBase
    {
        public int MinimumParticipants
        {
            get { return this.minimumParticipants; }
            set
            {
                this.minimumParticipants = value;
                this.NotifyPropertyChanged();
            }
        }
        private int minimumParticipants;

        public int TimeLimit
        {
            get { return this.timeLimit; }
            set
            {
                this.timeLimit = value;
                this.NotifyPropertyChanged();
            }
        }
        private int timeLimit;

        public CustomCommandModel StartedCommand
        {
            get { return this.startedCommand; }
            set
            {
                this.startedCommand = value;
                this.NotifyPropertyChanged();
            }
        }
        private CustomCommandModel startedCommand;

        public CustomCommandModel UserJoinCommand
        {
            get { return this.userJoinCommand; }
            set
            {
                this.userJoinCommand = value;
                this.NotifyPropertyChanged();
            }
        }
        private CustomCommandModel userJoinCommand;

        public CustomCommandModel NotEnoughPlayersCommand
        {
            get { return this.notEnoughPlayersCommand; }
            set
            {
                this.notEnoughPlayersCommand = value;
                this.NotifyPropertyChanged();
            }
        }
        private CustomCommandModel notEnoughPlayersCommand;

        public GameOutcomeViewModel UserSuccessOutcome
        {
            get { return this.userSuccessOutcome; }
            set
            {
                this.userSuccessOutcome = value;
                this.NotifyPropertyChanged();
            }
        }
        private GameOutcomeViewModel userSuccessOutcome;

        public CustomCommandModel UserFailureCommand
        {
            get { return this.userFailureCommand; }
            set
            {
                this.userFailureCommand = value;
                this.NotifyPropertyChanged();
            }
        }
        private CustomCommandModel userFailureCommand;

        public CustomCommandModel AllSucceedCommand
        {
            get { return this.allSucceedCommand; }
            set
            {
                this.allSucceedCommand = value;
                this.NotifyPropertyChanged();
            }
        }
        private CustomCommandModel allSucceedCommand;

        public CustomCommandModel TopThirdsSucceedCommand
        {
            get { return this.topThirdsSucceedCommand; }
            set
            {
                this.topThirdsSucceedCommand = value;
                this.NotifyPropertyChanged();
            }
        }
        private CustomCommandModel topThirdsSucceedCommand;

        public CustomCommandModel MiddleThirdsSucceedCommand
        {
            get { return this.middleThirdsSucceedCommand; }
            set
            {
                this.middleThirdsSucceedCommand = value;
                this.NotifyPropertyChanged();
            }
        }
        private CustomCommandModel middleThirdsSucceedCommand;

        public CustomCommandModel LowThirdsSucceedCommand
        {
            get { return this.lowThirdsSucceedCommand; }
            set
            {
                this.lowThirdsSucceedCommand = value;
                this.NotifyPropertyChanged();
            }
        }
        private CustomCommandModel lowThirdsSucceedCommand;

        public CustomCommandModel NoneSucceedCommand
        {
            get { return this.noneSucceedCommand; }
            set
            {
                this.noneSucceedCommand = value;
                this.NotifyPropertyChanged();
            }
        }
        private CustomCommandModel noneSucceedCommand;

        public HeistGameCommandEditorWindowViewModel(HeistGameCommandModel command)
            : base(command)
        {
            this.MinimumParticipants = command.MinimumParticipants;
            this.TimeLimit = command.TimeLimit;
            this.StartedCommand = command.StartedCommand;
            this.UserJoinCommand = command.UserJoinCommand;
            this.NotEnoughPlayersCommand = command.NotEnoughPlayersCommand;
            this.UserSuccessOutcome = new GameOutcomeViewModel(command.UserSuccessOutcome);
            this.userFailureCommand = command.UserFailureCommand;
            this.AllSucceedCommand = command.AllSucceedCommand;
            this.TopThirdsSucceedCommand = command.TopThirdsSucceedCommand;
            this.MiddleThirdsSucceedCommand = command.MiddleThirdsSucceedCommand;
            this.LowThirdsSucceedCommand = command.LowThirdsSucceedCommand;
            this.NoneSucceedCommand = command.NoneSucceedCommand;
        }

        public HeistGameCommandEditorWindowViewModel(CurrencyModel currency)
            : base(currency)
        {
            this.MinimumParticipants = 2;
            this.TimeLimit = 60;
            this.StartedCommand = this.CreateBasicChatCommand(MixItUp.Base.Resources.GameCommandHeistStartedExample);
            this.UserJoinCommand = this.CreateBasicCommand();
            this.NotEnoughPlayersCommand = this.CreateBasicChatCommand(MixItUp.Base.Resources.GameCommandNotEnoughPlayersExample);
            this.UserSuccessOutcome = new GameOutcomeViewModel(string.Empty, 50, 200);
            this.userFailureCommand = this.CreateBasicCommand();
            this.AllSucceedCommand = this.CreateBasicChatCommand(string.Format(MixItUp.Base.Resources.GameCommandHeistAllSucceedExample, currency.Name));
            this.TopThirdsSucceedCommand = this.CreateBasicChatCommand(string.Format(MixItUp.Base.Resources.GameCommandHeistTopThirdsSucceedExample, currency.Name));
            this.MiddleThirdsSucceedCommand = this.CreateBasicChatCommand(string.Format(MixItUp.Base.Resources.GameCommandHeistMiddleThirdsSucceedExample, currency.Name));
            this.LowThirdsSucceedCommand = this.CreateBasicChatCommand(string.Format(MixItUp.Base.Resources.GameCommandHeistLowThirdsSucceedExample, currency.Name));
            this.NoneSucceedCommand = this.CreateBasicChatCommand(string.Format(MixItUp.Base.Resources.GameCommandHeistNoneSucceedExample, currency.Name));
        }

        public override Task<CommandModelBase> GetCommand()
        {
            return Task.FromResult<CommandModelBase>(new HeistGameCommandModel(this.Name, this.GetChatTriggers(), this.MinimumParticipants, this.TimeLimit, this.StartedCommand, this.UserJoinCommand, this.NotEnoughPlayersCommand,
                this.UserSuccessOutcome.GetModel(), this.UserFailureCommand, this.AllSucceedCommand, this.TopThirdsSucceedCommand, this.MiddleThirdsSucceedCommand, this.LowThirdsSucceedCommand, this.NoneSucceedCommand));
        }

        public override async Task<Result> Validate()
        {
            Result result = await base.Validate();
            if (!result.Success)
            {
                return result;
            }

            if (this.MinimumParticipants < 1)
            {
                return new Result(MixItUp.Base.Resources.GameCommandMinimumParticipantsMustBeGreaterThan0);
            }

            if (this.TimeLimit <= 0)
            {
                return new Result(MixItUp.Base.Resources.GameCommandTimeLimitMustBePositive);
            }

            return new Result();
        }
    }
}