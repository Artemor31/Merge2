using System.Collections.Generic;
using Services.Infrastructure;
using UI;

namespace Services
{
    public class TutorialService : IService
    {
        private const string SavePath = "TutorData";
        public static TutorialService Instance => _instance ??= new TutorialService();
        private static TutorialService _instance;
        //public bool NeedTutor => !_tutorData.Seen;
        public bool NeedTutor { get; set; } = true;
        public bool EndedTutor { get; set; } = false;
        public void AddItem(TutorView view) => _views.Add(view.Id2, view);
        public TutorView GetItem(string id) => _views[id];
        private readonly Dictionary<string, TutorView> _views = new();
        private readonly TutorData _tutorData;
        private TutorialService() => _tutorData = new TutorData();
    }
    
    public class TutorData : SaveData
    {
        public bool Seen = false;
        public bool Ended = false;
    }
}