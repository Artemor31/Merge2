using System.Collections.Generic;
using Services.Infrastructure;
using UI;

namespace Services
{
    public class TutorialService : IService
    {
        private const string SavePath = "TutorData";

        public bool InTutor { get; set; }
        public bool SeenTutor => _tutorData.Seen;
        
        private readonly Dictionary<string, TutorView> _views = new();
        private readonly SaveService _saveService;
        private readonly TutorData _tutorData;

        public TutorialService(SaveService saveService)
        {
            _saveService = saveService;
            _tutorData = _saveService.Restore<TutorData>(SavePath);
        }

        public void StartTutor()
        {
            _tutorData.Seen = true;
            _saveService.Save(SavePath, _tutorData);
        }
        
        public void AddItem(TutorView view) => _views.Add(view.Id2, view);
        public TutorView GetItem(string id) => _views[id];
    }
    
    public class TutorData : SaveData
    {
        public bool Seen;
    }
}