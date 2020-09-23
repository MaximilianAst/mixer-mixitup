﻿using MixItUp.Base.Model.Actions;
using MixItUp.Base.Util;
using System.Threading.Tasks;

namespace MixItUp.Base.ViewModel.Controls.Actions
{
    public class SpecialIdentifierActionEditorControlViewModel : ActionEditorControlViewModelBase
    {
        public override ActionTypeEnum Type { get { return ActionTypeEnum.SpecialIdentifier; } }

        public string SpecialIdentifierName
        {
            get { return this.specialIdentifierName; }
            set
            {
                this.specialIdentifierName = value;
                this.NotifyPropertyChanged();
            }
        }
        private string specialIdentifierName;

        public string ReplacementText
        {
            get { return this.replacementText; }
            set
            {
                this.replacementText = value;
                this.NotifyPropertyChanged();
            }
        }
        private string replacementText;

        public bool MakeGloballyUsable
        {
            get { return this.makeGloballyUsable; }
            set
            {
                this.makeGloballyUsable = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool makeGloballyUsable;

        public bool ShouldProcessMath
        {
            get { return this.shouldProcessMath; }
            set
            {
                this.shouldProcessMath = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool shouldProcessMath;

        public SpecialIdentifierActionEditorControlViewModel(SpecialIdentifierActionModel action)
        {
            this.SpecialIdentifierName = action.SpecialIdentifierName;
            this.ReplacementText = action.ReplacementText;
            this.MakeGloballyUsable = action.MakeGloballyUsable;
            this.ShouldProcessMath = action.ShouldProcessMath;
        }

        public SpecialIdentifierActionEditorControlViewModel() { }

        public override Task<Result> Validate()
        {
            if (string.IsNullOrEmpty(this.SpecialIdentifierName))
            {
                return Task.FromResult(new Result(MixItUp.Base.Resources.SpecialIdentifierActionMissingSpecialIdentifierName));
            }

            this.SpecialIdentifierName = this.SpecialIdentifierName.Replace("$", "");
            if (!SpecialIdentifierStringBuilder.IsValidSpecialIdentifier(this.SpecialIdentifierName))
            {
                return Task.FromResult(new Result(MixItUp.Base.Resources.SpecialIdentifierActionInvalidSpecialIdentifierName));
            }

            if (string.IsNullOrEmpty(this.ReplacementText))
            {
                return Task.FromResult(new Result(MixItUp.Base.Resources.SpecialIdentifierActionMissingReplacementText));
            }

            return Task.FromResult(new Result());
        }

        public override Task<ActionModelBase> GetAction() { return Task.FromResult<ActionModelBase>(new SpecialIdentifierActionModel(this.SpecialIdentifierName, this.ReplacementText, this.MakeGloballyUsable, this.ShouldProcessMath)); }
    }
}