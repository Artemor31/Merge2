using System.Collections.Generic;
using Services.Infrastructure;
using UI;

namespace Services
{
    public class TutorialService : IService
    {
        private const string SavePath = "TutorData";
        
        //public bool NeedTutor => !_tutorData.Seen;
        public bool NeedTutor { get; set; } = true;
        public bool EndedTutor { get; set; } = false;
        
        private readonly Dictionary<string, TutorView> _views = new();
        private readonly TutorData _tutorData = new();

        public void AddItem(TutorView view) => _views.Add(view.Id2, view);
        public TutorView GetItem(string id) => _views[id];
    }
    
    public class TutorData : SaveData
    {
        public bool Seen = false;
        public bool Ended = false;
    }
}