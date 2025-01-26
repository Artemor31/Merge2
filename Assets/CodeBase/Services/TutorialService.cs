using System.Collections.Generic;
using Services.Infrastructure;
using UI;

namespace Services
{
    public class TutorialService : IService
    {
        public static TutorialService Instance => _instance ??= new TutorialService();
        private static TutorialService _instance;
        public bool NeedTutor => !_tutorData.Seen;
        public void AddItem(TutorView view) => _views.Add(view.Id, view);
        public TutorView GetItem(int id) => _views[id];
        private readonly Dictionary<int, TutorView> _views = new();
        private readonly TutorData _tutorData;
        private TutorialService() => _tutorData = new TutorData();
    }
    
    public class TutorData : SaveData
    {
        public int Step = 0;
        public bool Seen = false;
    }
}