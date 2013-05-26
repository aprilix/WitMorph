﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WitMorph.UI
{
    public class HubViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _currentCollectionUri;
        private string _currentProjectName;

        private string _goalCollectionUri;
        private string _goalProjectName;
        private string _processMapFile;
        private string _outputActionsFile;
        private string _inputActionsFile;

        public string CurrentCollectionUri
        {
            get { return _currentCollectionUri; }
            set
            {
                _currentCollectionUri = value;
                OnPropertyChanged();
            }
        }

        public string CurrentProjectName
        {
            get { return _currentProjectName; }
            set
            {
                _currentProjectName = value;
                OnPropertyChanged();
            }
        }

        public string GoalCollectionUri
        {
            get { return _goalCollectionUri; }
            set
            {
                _goalCollectionUri = value;
                OnPropertyChanged();
            }
        }

        public string GoalProjectName
        {
            get { return _goalProjectName; }
            set
            {
                _goalProjectName = value;
                OnPropertyChanged();
            }
        }

        public string ProcessMapFile
        {
            get { return _processMapFile; }
            set
            {
                _processMapFile = value;
                OnPropertyChanged();
            }
        }

        public string OutputActionsFile
        {
            get { return _outputActionsFile; }
            set
            {
                _outputActionsFile = value;
                OnPropertyChanged();
            }
        }

        public string InputActionsFile
        {
            get { return _inputActionsFile; }
            set
            {
                _inputActionsFile = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}