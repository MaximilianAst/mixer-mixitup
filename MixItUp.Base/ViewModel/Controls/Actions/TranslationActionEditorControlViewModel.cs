﻿using MixItUp.Base.Model.Actions;
using MixItUp.Base.Util;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace MixItUp.Base.ViewModel.Controls.Actions
{
    public class TranslationActionEditorControlViewModel : ActionEditorControlViewModelBase
    {
        public override ActionTypeEnum Type { get { return ActionTypeEnum.Translation; } }

        public ObservableCollection<CultureInfo> Languages { get; private set; } = new ObservableCollection<CultureInfo>();

        public CultureInfo SelectedLanguage
        {
            get { return this.selectedLanguage; }
            set
            {
                this.selectedLanguage = value;
                this.NotifyPropertyChanged();
            }
        }
        private CultureInfo selectedLanguage;

        public bool AllowProfanity
        {
            get { return this.allowProfanity; }
            set
            {
                this.allowProfanity = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool allowProfanity;

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                this.NotifyPropertyChanged();
            }
        }
        private string text;

        public TranslationActionEditorControlViewModel(TranslationActionModel action)
        {
            this.SelectedLanguage = action.Culture;
            this.AllowProfanity = action.AllowProfanity;
            this.Text = action.Text;
        }

        public TranslationActionEditorControlViewModel() { }

        protected override async Task OnLoadedInternal()
        {
            foreach (CultureInfo language in await ChannelSession.Services.Translation.GetAvailableLanguages())
            {
                this.Languages.Add(language);
            }
            this.NotifyPropertyChanged("SelectedLanguage");
            await base.OnLoadedInternal();
        }

        public override Task<Result> Validate()
        {
            if (this.SelectedLanguage == null)
            {
                return Task.FromResult(new Result(MixItUp.Base.Resources.TranslationActionMissingLanguage));
            }

            if (string.IsNullOrEmpty(this.Text))
            {
                return Task.FromResult(new Result(MixItUp.Base.Resources.TranslationActionMissingText));
            }

            return Task.FromResult(new Result());
        }

        public override Task<ActionModelBase> GetAction() { return Task.FromResult<ActionModelBase>(new TranslationActionModel(this.SelectedLanguage, this.AllowProfanity, this.Text)); }
    }
}