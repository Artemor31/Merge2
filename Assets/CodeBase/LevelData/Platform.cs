using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.LevelData
{
    public class Platform : MonoBehaviour
    {
        public bool Pressing;
        public Actor Actor;
        public Vector2Int Index;
        private Camera _camera;
        public GridView _gridView;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void OnMouseDown()
        {
            Pressing = true;
            _gridView.SetSelected(Index);
        }

        private void OnMouseUp()
        {
            Pressing = false;
            _gridView.SetSelected(false);
            if (Actor == null) return;
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] raycastHits = Physics.RaycastAll(ray, 1000);

            foreach (RaycastHit hit in raycastHits)
            {
                var platform = hit.transform.GetComponent<Platform>();
                if (platform != null)
                {
                    Actor.transform.position = platform.transform.position;
                    platform.Actor = Actor;                   
                    Actor = null;
                }
            }
        }

        private void Update()
        {
            if (Pressing && Actor != null)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] raycastHits = Physics.RaycastAll(ray, 1000);
                foreach (RaycastHit hit in raycastHits)
                {
                    var platform = hit.transform.GetComponent<Platform>();
                    if (platform != null)
                    {
                        platform.Select();
                        PointFollower(hit, ray);
                        _gridView.SetSelected(Index);
                        return;
                    }
                }
            }
        }

        public void Select()
        {
        }

        private void PointFollower(RaycastHit hit, Ray ray)
        {
            var plane = new Plane(Vector3.up, hit.transform.position);
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                //hitPoint.y += 1;
                Actor.transform.position = hitPoint;
            }
        }

        private void OnMouseEnter()
        {
            Debug.LogError("hover " + Index);
        }
    }
}